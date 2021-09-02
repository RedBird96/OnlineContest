using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegisterCompanyID
{
    public partial class CollectionForm : Form
    {
        public CollectionForm()
        {
            InitializeComponent();
        }

        private void CollectionForm_Load(object sender, EventArgs e)
        {

        }

        public void startDownload()
        {
            if (progressBar.InvokeRequired)
            {
                Action safeWrite = delegate { startDownload(); };
                progressBar.Invoke(safeWrite);
            }
            else
            {
                List<string> ltName = new List<string>(lstName.Items.OfType<string>().ToArray());
                string name = txtName.Text;
                string symbol = cmbSymbol.SelectedItem.ToString();
                DateTime fromdate = Convert.ToDateTime(dtFrom.Text);
                DateTime todate = Convert.ToDateTime(dtTo.Text).AddDays(1);
                string filepath = "";
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 0;
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    filepath = saveFileDialog1.FileName;
                }
                else
                    return;

                progressBar.Value = 0;

                if (txtName.Text.Length != 0 && !ltName.Exists(lname => lname == name))
                {
                    ltName.Add(name);
                }

                if (ltName.Count == 0)
                {
                    var csv = new StringBuilder();
                    string csvHeader = "", csvBody = "";
                    csvHeader = "Symbol, Start Date, End Date, Total Question, Correct Percent";
                    csvBody = symbol + "," + fromdate.ToString("yyyy-MM-dd") + "," + todate.ToString("yyyy-MM-dd") + ",";
                    int totalCount = 0, correctCount = 0;

                    string strSQL2 = "select * from Judgement where Date between '" + fromdate.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + todate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                    DataTable dTable = SQL.GetDataTable(strSQL2);
                    int nProgressBarTotal = dTable.Rows.Count;
                    int nProgressBarIndex = 0;

                    foreach (DataRow row in dTable.Rows)
                    {
                        nProgressBarIndex++;
                        int nProgressVal = (int)((float)nProgressBarIndex / (float)nProgressBarTotal * 100);
                        progressBar.Value = nProgressVal;

                        string questionCorrect = row["TotalCorrect"].ToString();
                        if (questionCorrect.Length == 0)
                            continue;

                        int col_cnt = (row.Table.Columns.Count - 8) / 4;
                        for (int question_index = 0; question_index < col_cnt; question_index++)
                        {
                            int QIndex = (question_index * 4) + 8;
                            if (row.ItemArray.ElementAt(QIndex).ToString().Length == 0)
                            {
                                continue;
                            }

                            string RcolumnHeaderName = row.Table.Columns[QIndex + 3].ColumnName;

                            string[] parameter_arr;
                            int question_type;
                            string question_result;
                            string question_answer;

                            if (question_index > 4)
                            {
                                question_answer = row.ItemArray.ElementAt(QIndex).ToString();
                                parameter_arr = row.ItemArray.ElementAt(QIndex + 1).ToString().Split(',');// row[Ptag_Name].ToString().Split(',');
                                question_type = int.Parse(row.ItemArray.ElementAt(QIndex + 2).ToString());// int.Parse(row[Ttag_Name].ToString());
                                question_result = row.ItemArray.ElementAt(QIndex + 3).ToString().ToLower();// int.Parse(row[Ttag_Name].ToString());
                            }
                            else
                            {
                                question_answer = row.ItemArray.ElementAt(QIndex).ToString();
                                question_type = int.Parse(row.ItemArray.ElementAt(QIndex + 1).ToString());// int.Parse(row[Ttag_Name].ToString());
                                parameter_arr = row.ItemArray.ElementAt(QIndex + 2).ToString().Split(',');// row[Ptag_Name].ToString().Split(',');
                                question_result = row.ItemArray.ElementAt(QIndex + 3).ToString().ToLower();// int.Parse(row[Ttag_Name].ToString());
                            }

                            if (parameter_arr[0] != symbol)
                            {
                                if (question_type != 5 || (question_type == 5 && parameter_arr[1] != symbol))
                                    continue;
                            }

                            totalCount++;
                            if (question_type != 5)
                            {
                                if (question_result == "true")
                                    correctCount++;
                            }
                            else
                            {
                                if ((question_result == "true" && question_answer == symbol) ||
                                    (question_result == "false" && question_answer != symbol))
                                    correctCount++;
                            }

                        }

                    }

                    if (totalCount != 0)
                    {

                        float percent = (float)correctCount / (float)totalCount * 100;
                        csvBody = csvBody + totalCount + "," + ((int)percent).ToString() + "% correct";
                    }


                    csv.AppendLine(csvHeader);
                    csv.AppendLine(csvBody);

                    File.WriteAllText(filepath, csv.ToString());
                }

                else
                {
                    var csv = new StringBuilder();
                    string csvHeader = "", csvBody = "";
                    csvHeader = "UserName, Total Correct, Total Question, Total Accuracy, SPY Correct, SPY Question, SPY Accuracy, QQQ Correct, QQQ Question, QQQ Accuracy, IWM Correct, IWM Question, IWM Accuracy, TLT Correct, TLT Question, TLT Accuracy, TSLA Correct, TSLA Question, TSLA Accuracy, NFLX Correct, NFLX Question, NFLX Accuracy, AAPL Correct, AAPL Question, AAPL Accuracy, AMZN Correct, AMZN Question, AMZN Accuracy, FB Correct, FB Question, FB Accuracy, GOOGL Correct, GOOGL Question, GOOGL Accuracy, NVDA Correct, NVDA Question, NVDA Accuracy";
                    csv.AppendLine(csvHeader);

                    Dictionary<string, int> symbolcorrectCount = new Dictionary<string, int>();
                    Dictionary<string, int> symboltotalCount = new Dictionary<string, int>();

                    symbolcorrectCount.Add("SPY", 0); symboltotalCount.Add("SPY", 0);
                    symbolcorrectCount.Add("QQQ", 0); symboltotalCount.Add("QQQ", 0);
                    symbolcorrectCount.Add("IWM", 0); symboltotalCount.Add("IWM", 0);
                    symbolcorrectCount.Add("TLT", 0); symboltotalCount.Add("TLT", 0);
                    symbolcorrectCount.Add("TSLA", 0); symboltotalCount.Add("TSLA", 0);
                    symbolcorrectCount.Add("NFLX", 0); symboltotalCount.Add("NFLX", 0);
                    symbolcorrectCount.Add("AAPL", 0); symboltotalCount.Add("AAPL", 0);
                    symbolcorrectCount.Add("AMZN", 0); symboltotalCount.Add("AMZN", 0);
                    symbolcorrectCount.Add("FB", 0); symboltotalCount.Add("FB", 0);
                    symbolcorrectCount.Add("GOOGL", 0); symboltotalCount.Add("GOOGL", 0);
                    symbolcorrectCount.Add("NVDA", 0); symboltotalCount.Add("NVDA", 0);

                    int nProgressBarTotal = ltName.Count;
                    int nProgressBarIndex = 0;

                    foreach (string nameOne in ltName)
                    {

                        csvBody = "";
                        string strSQL1 = "select sum(ProblemNo) as totalcontest, sum(TotalCorrect) as totalcorrect from Judgement where Name='" + nameOne + "' and Date between '" + fromdate.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + todate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        DataTable dTable = SQL.GetDataTable(strSQL1);
                        string contestCount = dTable.Rows[0]["totalcontest"].ToString();
                        string correctCount = dTable.Rows[0]["totalcorrect"].ToString();

                        csvBody = nameOne + "," + correctCount + "," + contestCount + "," + (int)(( double.Parse(correctCount) / double.Parse(contestCount)) * 100) + "%,";
                        string strSQL2 = "select * from Judgement where Name='" + nameOne + "' and Date between '" + fromdate.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + todate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        dTable = SQL.GetDataTable(strSQL2);
                        foreach (DataRow row in dTable.Rows)
                        {
                            string questionCorrect = row["TotalCorrect"].ToString();
                            if (questionCorrect.Length == 0)
                                continue;

                            int col_cnt = (row.Table.Columns.Count - 8) / 4;

                            for (int question_index = 0; question_index < col_cnt; question_index++)
                            {
                                int QIndex = (question_index * 4) + 8;
                                if (row.ItemArray.ElementAt(QIndex).ToString().Length == 0)
                                {
                                    continue;
                                }
                                string RcolumnHeaderName = row.Table.Columns[QIndex + 3].ColumnName;

                                string[] parameter_arr;
                                int question_type;
                                string question_result;
                                string question_answer;

                                if (question_index > 4)
                                {
                                    question_answer = row.ItemArray.ElementAt(QIndex).ToString();
                                    parameter_arr = row.ItemArray.ElementAt(QIndex + 1).ToString().Split(',');// row[Ptag_Name].ToString().Split(',');
                                    question_type = int.Parse(row.ItemArray.ElementAt(QIndex + 2).ToString());// int.Parse(row[Ttag_Name].ToString());
                                    question_result = row.ItemArray.ElementAt(QIndex + 3).ToString().ToLower();// int.Parse(row[Ttag_Name].ToString());
                                }
                                else
                                {
                                    question_answer = row.ItemArray.ElementAt(QIndex).ToString();
                                    question_type = int.Parse(row.ItemArray.ElementAt(QIndex + 1).ToString());// int.Parse(row[Ttag_Name].ToString());
                                    parameter_arr = row.ItemArray.ElementAt(QIndex + 2).ToString().Split(',');// row[Ptag_Name].ToString().Split(',');
                                    question_result = row.ItemArray.ElementAt(QIndex + 3).ToString().ToLower();// int.Parse(row[Ttag_Name].ToString());
                                }
                                symboltotalCount[parameter_arr[0]]++;
                                if (question_type == 5)
                                {
                                    symboltotalCount[parameter_arr[1]]++;
                                    if (question_result == "true")
                                        symbolcorrectCount[question_answer]++;
                                    else
                                    {
                                        if (question_answer == parameter_arr[0])
                                            symbolcorrectCount[parameter_arr[1]]++;
                                        else
                                            symbolcorrectCount[parameter_arr[0]]++;
                                    }
                                    continue;
                                }

                                if (question_result == "true")
                                {
                                    symbolcorrectCount[parameter_arr[0]]++;
                                }
                            }
                        }

                        csvBody += symbolcorrectCount["SPY"] + "," + symboltotalCount["SPY"] + "," + (int)((double)symbolcorrectCount["SPY"] / (symboltotalCount["SPY"] == 0 ? 1 : symboltotalCount["SPY"]) * 100) + 
                            "%," + symbolcorrectCount["QQQ"] + "," + symboltotalCount["QQQ"] + "," + (int)((double)symbolcorrectCount["QQQ"] / (symboltotalCount["QQQ"] == 0 ? 1 : symboltotalCount["QQQ"]) * 100) +
                            "%," + symbolcorrectCount["IWM"] + "," + symboltotalCount["IWM"] + "," + (int)((double)symbolcorrectCount["IWM"] / (symboltotalCount["IWM"] == 0 ? 1 : symboltotalCount["IWM"]) * 100) +
                            "%," + symbolcorrectCount["TLT"] + "," + symboltotalCount["TLT"] + "," + (int)((double)symbolcorrectCount["TLT"] / (symboltotalCount["TLT"] == 0 ? 1 : symboltotalCount["TLT"]) * 100) +
                            "%," + symbolcorrectCount["TSLA"] + "," + symboltotalCount["TSLA"] + "," + (int)((double)symbolcorrectCount["TSLA"] / (symboltotalCount["TSLA"] == 0 ? 1 : symboltotalCount["TSLA"]) * 100) +
                            "%," + symbolcorrectCount["NFLX"] + "," + symboltotalCount["NFLX"] + "," + (int)((double)symbolcorrectCount["NFLX"] / (symboltotalCount["NFLX"] == 0 ? 1 : symboltotalCount["NFLX"]) * 100) +
                            "%," + symbolcorrectCount["AAPL"] + "," + symboltotalCount["AAPL"] + "," + (int)((double)symbolcorrectCount["AAPL"] / (symboltotalCount["AAPL"] == 0 ? 1 : symboltotalCount["AAPL"]) * 100) +
                            "%," + symbolcorrectCount["AMZN"] + "," + symboltotalCount["AMZN"] + "," + (int)((double)symbolcorrectCount["AMZN"] / (symboltotalCount["AMZN"] == 0 ? 1 : symboltotalCount["AMZN"]) * 100) +
                            "%," + symbolcorrectCount["FB"] + "," + symboltotalCount["FB"] + "," + (int)((double)symbolcorrectCount["FB"] / (symboltotalCount["FB"] == 0 ? 1 : symboltotalCount["FB"]) * 100) +
                            "%," + symbolcorrectCount["GOOGL"] + "," + symboltotalCount["GOOGL"] + "," + (int)((double)symbolcorrectCount["GOOGL"] / (symboltotalCount["GOOGL"] == 0 ? 1 : symboltotalCount["GOOGL"]) * 100) +
                            "%," + symbolcorrectCount["NVDA"] + "," + symboltotalCount["NVDA"] + "," + (int)((double)symbolcorrectCount["NVDA"] / (symboltotalCount["NVDA"] == 0 ? 1 : symboltotalCount["NVDA"]) * 100);

                        nProgressBarIndex++;
                        int nProgressVal = (int)((float)nProgressBarIndex / (float)nProgressBarTotal * 100);
                        progressBar.Value = nProgressVal;

                        csv.AppendLine(csvBody);
                    }


                    File.WriteAllText(filepath, csv.ToString());
                }
                MessageBox.Show("Complete download data");
            }


        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(startDownload);
            th.IsBackground = true;
            th.Start();
        }

        private void btnUploadNames_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    lstName.Items.Clear();
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                        string[] strArray = fileContent.Split(',');
                        foreach(string strOne in strArray)
                        {
                            string strOneT = strOne;
                            strOneT = strOneT.Trim();
                            strOneT = strOneT.Replace("\n", "");
                            strOneT = strOneT.Replace("\t", "");

                            lstName.Items.Add(strOneT);
                        }
                    }
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lstName.Items.Clear();
        }
    }
}
