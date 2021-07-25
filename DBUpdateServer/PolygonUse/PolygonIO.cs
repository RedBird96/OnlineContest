using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Security.Authentication;
using TradersToolbox;
using System.Threading.Tasks;
using PolygonIO.Api;
using PolygonIO.Client;
using WebSocket4Net;
using System.Threading;
using System.Collections.Concurrent;
using TradersToolbox.DataObjects;
using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Json.Serialization;

namespace PolygonUse
{
    class PolygonIO
    {
        public HttpClient httpClient;

        private readonly string AccessToken = "K6CD972EJqZjLSQ6UNjRIYu78e8_tSLK";

        readonly WebSocket websocket;
        readonly System.Timers.Timer websocketStartTimer;
        CancellationTokenSource messageHandlerCancelationTokenSource;
        ConcurrentQueue<string> messageBuffer = new ConcurrentQueue<string>();
        private StocksEquitiesApi stocksApi;

        ConcurrentQueue<PolygonIOWebSocketQuote> quotesBuffer = new ConcurrentQueue<PolygonIOWebSocketQuote>();

        public PolygonIO()
        {
            httpClient = HttpClientHelper.GetHttpClient();
            websocket = new WebSocket("wss://socket.polygon.io/stocks", sslProtocols: SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls, receiveBufferSize: 1024 * 1024);
            websocket.Opened += Websocket_Opened;
            websocket.Error += Websocket_Error;
            websocket.Closed += Websocket_Closed;
            websocket.MessageReceived += Websocket_MessageReceived;

            websocket.Open();

            Configuration config = new Configuration();
            config.AddApiKey("apiKey", AccessToken);
            stocksApi = new StocksEquitiesApi(config);

            websocketStartTimer = new System.Timers.Timer(300);
            websocketStartTimer.AutoReset = false;
            websocketStartTimer.Elapsed += WebsocketStartTimer_Elapsed;

            GetHistoryBar("AAPL");

        }

        public void GetHistoryBar(string symbol)
        {
            string unit = "minute";
            //You have to set here yesterday date
            DateTime startDT = DateTime.UtcNow.AddDays(-3);
            
            var resp = stocksApi.V2AggsTickerStocksTickerRangeMultiplierTimespanFromToGet(symbol, 30, unit,
                            startDT.ToString("yyyy-MM-dd"), DateTime.UtcNow.ToString("yyyy-MM-dd"));

            if (resp.Status == "OK" && resp.ResultsCount > 0)
            {
                foreach (var item in resp.Results)
                {
                    //You have to use the item.C value for calculate the value.
                    string strOut = item.T.ToString() + ":" + item.C;
                    Console.WriteLine(strOut);
                }

            }
        }
        private void WebsocketStartTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (websocket)
            {
                // Reconnect logic
                if (websocket.State == WebSocketState.Closed)
                {
                    websocket.Open();
                    int i = 0;
                    while (websocket.State == WebSocketState.Closed && i < 20)  //1 sec max
                    {
                        Task.Delay(50).Wait();
                        i++;
                    }
                }
            }
        }
        private void Websocket_Opened(object sender, EventArgs e)
        {
            messageBuffer = new ConcurrentQueue<string>();
            messageHandlerCancelationTokenSource = new CancellationTokenSource();
            Task.Run(async () => await MessageHandlerTask(messageHandlerCancelationTokenSource.Token));

            Console.Write("POLYGON.IO: Websocket connected!\n");
            websocket.Send($"{{\"action\":\"auth\",\"params\":\"{AccessToken}\"}}");

            List<TickerData> tickers = ReadStockTickers().Result;

            string symbolsRequestQ = string.Join(",", tickers.Select(x => $"Q.{x.Ticker}"));
            string symbolsRequestT = string.Join(",", tickers.Select(x => $"T.{x.Ticker}"));

            {
                websocket.Send($"{{\"action\":\"subscribe\",\"params\":\"{symbolsRequestQ}\"}}");

                //string symbolsRequestT = string.Join(",", StreamQuotesTickersList.Select(x => $"T.{x}"));
                websocket.Send($"{{\"action\":\"subscribe\",\"params\":\"{symbolsRequestT}\"}}");
            }
        }
        private void Websocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
        }
        private void Websocket_Closed(object sender, EventArgs e)
        {
            Console.Write("POLYGON.IO: Connection Closed...");

            messageHandlerCancelationTokenSource?.Cancel();

            if (e is ClosedEventArgs arg && arg.Reason == "close")
            {
                //do nothing, just exit (normal situation)
            }
            else
            {
                if (!websocketStartTimer.Enabled)
                    websocketStartTimer.Start();
            }
        }
        private void Websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            messageBuffer.Enqueue(e.Message);
        }

        private async Task MessageHandlerTask(CancellationToken cancellationToken)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (!cancellationToken.IsCancellationRequested)
            {
                if (messageBuffer.Count == 0)
                {
                    await Task.Delay(100);
                    continue;
                }

                do
                {
                    if (messageBuffer.TryDequeue(out string s))
                    {
                        _ = Task.Run(() => HandleMessage(s));   //messages handling order is not guaranteed
                    }
                }
                while (messageBuffer.Count > 0 && !cancellationToken.IsCancellationRequested);

                stopwatch.Stop();
                if (stopwatch.ElapsedMilliseconds > 200)
                {
                    Dictionary<string, PolygonIOWebSocketQuote> quoteGlobalCache = new Dictionary<string, PolygonIOWebSocketQuote>();
                    while (quotesBuffer.TryDequeue(out PolygonIOWebSocketQuote quote))
                    {
                        if (quote.AskPrice != 0 && quote.BidPrice != 0)
                            quoteGlobalCache[quote.Symbol] = quote;


                        Console.WriteLine(quote.Symbol + ":" + quote.AskPrice + "," + quote.BidPrice);
                    }

                    stopwatch.Reset();
                }
                stopwatch.Start();
            }
        }
        private void HandleMessage(string s)
        {
            if (Regex.IsMatch(s, @"""ev"":\s*""Q"""))   //Quotes
            {
                try
                {
                    var quotes = JsonSerializer.Deserialize<PolygonIOWebSocketQuote[]>(s);

                    foreach (var quote in quotes)
                    {
                        //Messenger.Default.Send(new QuoteReceivedMessage(quote.Symbol, ask: (decimal)quote.AskPrice, bid: (decimal)quote.BidPrice,
                        //    lastDateTimeUTC: Utils.ToDateTime(quote.Timestamp / 1000)));
                        quotesBuffer.Enqueue(quote);
                    }
                }
                catch (Exception ex) { }
            }
        }
        public async Task<List<TickerData>> ReadStockTickers()
        {
            // Read all symbols from OATS Reportable Security Daily List
            // https://www.finra.org/filing-reporting/oats/oats-reportable-securities-list

            string resString = null;
            List<TickerData> symbols = new List<TickerData>();

            try
            {
                resString = await httpClient.GetStringAsync("http://oatsreportable.finra.org/OATSReportableSecurities-SOD.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to read stock tickers from FINRA!");
                return symbols;
            }

            return await Task.Run(() =>
            {
                bool skipFirstLine = true;
                using (System.IO.StringReader reader = new System.IO.StringReader(resString))
                {
                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        if (skipFirstLine)
                        {
                            skipFirstLine = false;
                            if (!line.StartsWith("A"))
                                continue;
                        }

                        var cells = line.Split('|');
                        if (cells.Length == 3)
                        {
                            bool success = true;

                            // Filter by exchange
                            //Exchange ex = Exchange.NASDAQ;
                            switch (cells[2])
                            {
                                case "NASDAQ": break;// ex = Exchange.NASDAQ; break;
                                case "NYSE": break;// ex = Exchange.NYSE; break;
                                case "ARCA": break;// ex = Exchange.ARCA; break;
                                case "AMEX": break;// ex = Exchange.AMEX; break;
                                case "BATS": break;// ex = Exchange.BATS; break;
                                default:
                                    success = false;
                                    break;
                            }

                            // Filter by ticker
                            if (success && cells[0] == "IGLD" || cells[0].Contains(' '))
                                success = false;

                            // Filter warrants
                            if (success && cells[1].ToLower().Contains("warrant"))
                                success = false;

                            if (success)
                                symbols.Add(new TickerData(cells[0], cells[2], cells[1]));
                        }

                        line = reader.ReadLine();
                    }
                }

                return symbols;
            });
        }
    }
}
