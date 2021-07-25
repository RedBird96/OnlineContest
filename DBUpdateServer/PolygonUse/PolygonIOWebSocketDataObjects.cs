using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TradersToolbox.DataObjects
{
    class PolygonIOWebSocketQuote
    {
        [JsonPropertyName("ev")]
        public string Event { get; set; }

        [JsonPropertyName("sym")]
        public string Symbol { get; set; }

        [JsonPropertyName("bx")]
        public int BidExchange { get; set; }

        [JsonPropertyName("bp")]
        public double BidPrice { get; set; }

        [JsonPropertyName("bs")]
        public int BidSize { get; set; }

        [JsonPropertyName("ax")]
        public int AskExchange { get; set; }

        [JsonPropertyName("ap")]
        public double AskPrice { get; set; }

        [JsonPropertyName("as")]
        public int AskSize { get; set; }

        [JsonPropertyName("c")]
        public int Condition { get; set; }

        [JsonPropertyName("t")]
        public long Timestamp { get; set; }

        [JsonPropertyName("z")]
        public int zTemp { get; set; }
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
