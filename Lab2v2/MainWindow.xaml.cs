using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Net;

namespace Lab2v2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<ISThreat> parsedThreatList = new List<ISThreat>();
        public static List<ISThreat> localThreatList = new List<ISThreat>();
        public static List<ChangeLogObhect> changelog = new List<ChangeLogObhect>();
        public static MainWindow mw;
        private PagingCollectionView _cview;
        public MainWindow()
        {
            InitializeComponent();
            if (!File.Exists(@"C:\Users\Sean\source\repos\labs\Lab2v2\localthreatlist.xlsx")) 
            {
                UpdateFromFSTEK(@"C:\Users\Sean\source\repos\labs\Lab2v2\localthreatlist.xlsx");
            }
            else
            {
                ThreatsFromExelToList(@"C:\Users\Sean\source\repos\labs\Lab2v2\localthreatlist.xlsx");
            }
            localThreatList = parsedThreatList;
            parsedThreatList = new List<ISThreat>();
            this._cview = new PagingCollectionView(MainWindow.localThreatList, 15);
            this.DataContext = this._cview;
        }
        private void OnNextClicked(object sender, RoutedEventArgs e)
        {
            this._cview.MoveToNextPage();
        }
        private void OnPreviousClicked(object sender, RoutedEventArgs e)
        {
            this._cview.MoveToPreviousPage();
        }
        private void DataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
        private void OpenThreatInfo(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                ThreatInfo l = new ThreatInfo((ISThreat)Threat_Grid.SelectedItem);
                l.Show();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }
        private void DownloadFSTEK_Click(object sender, RoutedEventArgs e)
        {
            UpdateFromFSTEK(@"C:\Users\Sean\source\repos\labs\Lab2v2\fsteklist.xlsx");
            //this.Threat_Grid.Items.Refresh();


        }
        private void UpdateFromFSTEK(string fileLink)
        {
            //string fileLink = @"C:\Users\Sean\source\repos\labs\Lab2v2\fsteklist.xlsx";
            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFile("https://bdu.fstec.ru/files/documents/thrlist.xlsx", fileLink);
                ThreatsFromExelToList(fileLink);
                CompareLocalAndFSTEK();
                this._cview = new PagingCollectionView(MainWindow.localThreatList, 15);
                this.DataContext = this._cview;

            }
            catch (Exception ex)
            {
                ChangeLogObhect.UpdateStatus = ex.ToString();
            }
            UpdateInfo k = new UpdateInfo();
            k.Show();
        }
        private static void ThreatsFromExelToList(string link)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(link)))
            {
                var myWorksheet = xlPackage.Workbook.Worksheets.First(); //select sheet here
                var totalRows = myWorksheet.Dimension.End.Row;
                var totalColumns = myWorksheet.Dimension.End.Column;
                var sb = new StringBuilder(); //this is your data
                for (int rowNum = 1; rowNum <= totalRows; rowNum++) //select starting row here
                {
                    var row = myWorksheet.Cells[rowNum, 1, rowNum, totalColumns].Select(c => c.Value == null ? string.Empty : c.Value.ToString());
                    sb.AppendLine(string.Join("NEXTCELL", row));
                    sb.AppendLine("ENDROW"); //end of a row
                }
                string[] threatStringsByRows = sb.ToString().Split(new string[] { "ENDROW" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 2; i < threatStringsByRows.Length; i++)
                {
                    var threatString = threatStringsByRows[i].Replace("\n", "").Replace("\r", "").Split(new string[] { "NEXTCELL" }, StringSplitOptions.None);

                    if (threatString.Length == 10)
                    {
                        ISThreat tempoThreat = new ISThreat(Int32.Parse(threatString[0]));
                        tempoThreat.ThreatName = threatString[1].ToString();
                        tempoThreat.ThreatInfo = threatString[2].ToString();
                        tempoThreat.ThreatSource = threatString[3].ToString();
                        tempoThreat.ThreaObj = threatString[4].ToString();
                        tempoThreat.ConfidentialityViolation = Convert.ToBoolean(Int32.Parse(threatString[5]));
                        tempoThreat.IntegrityViolation = Convert.ToBoolean(Int32.Parse(threatString[6]));
                        tempoThreat.AvailabilityViolation = Convert.ToBoolean(Int32.Parse(threatString[7]));
                        parsedThreatList.Add(tempoThreat);
                    }
                }

            }
        }
        private static void CompareLocalAndFSTEK()
        {
            
            int diff = parsedThreatList.Count - localThreatList.Count;
            for (int i = 0; i < localThreatList.Count; i++)
            {
                if (!localThreatList[i].Equals(parsedThreatList[i]))
                {
                    if (localThreatList[i].ThreaObj != parsedThreatList[i].ThreaObj)
                    {
                        ChangeLogObhect newLog = new ChangeLogObhect();
                        newLog.LogType = "Изменение";
                        newLog.LogObjId = localThreatList[i].ThreatId;
                        newLog.LogObjectType = "объект воздействия угрозы";
                        newLog.LogCurState = parsedThreatList[i].ThreaObj;
                        newLog.LogPrevState = localThreatList[i].ThreaObj;
                        ChangeLogObhect.LogCountChng++;
                        changelog.Add(newLog);
                    }
                    if (localThreatList[i].ThreatInfo != parsedThreatList[i].ThreatInfo)
                    {
                        ChangeLogObhect newLog = new ChangeLogObhect();
                        newLog.LogType = "Изменение";
                        newLog.LogObjId = localThreatList[i].ThreatId;
                        newLog.LogObjectType = "описание угрозы";
                        newLog.LogCurState = parsedThreatList[i].ThreatInfo;
                        newLog.LogPrevState = localThreatList[i].ThreatInfo;
                        ChangeLogObhect.LogCountChng++;
                        changelog.Add(newLog);
                    }
                    if (localThreatList[i].ThreatName != parsedThreatList[i].ThreatName)
                    {
                        ChangeLogObhect newLog = new ChangeLogObhect();
                        newLog.LogType = "Изменение";
                        newLog.LogObjId = localThreatList[i].ThreatId;
                        newLog.LogObjectType = "наименование угрозы";
                        newLog.LogCurState = parsedThreatList[i].ThreatName;
                        newLog.LogPrevState = localThreatList[i].ThreatName;
                        ChangeLogObhect.LogCountChng++;
                        changelog.Add(newLog);
                    }
                    if (localThreatList[i].ThreatSource != parsedThreatList[i].ThreatSource)
                    {
                        ChangeLogObhect newLog = new ChangeLogObhect();
                        newLog.LogType = "Изменение";
                        newLog.LogObjId = localThreatList[i].ThreatId;
                        newLog.LogObjectType = "источник угрозы";
                        newLog.LogCurState = parsedThreatList[i].ThreatSource;
                        newLog.LogPrevState = localThreatList[i].ThreatSource;
                        ChangeLogObhect.LogCountChng++;
                        changelog.Add(newLog);
                    }
                    if (localThreatList[i].IntegrityViolation != parsedThreatList[i].IntegrityViolation)
                    {
                        ChangeLogObhect newLog = new ChangeLogObhect();
                        newLog.LogType = "Изменение";
                        newLog.LogObjId = localThreatList[i].ThreatId;
                        newLog.LogObjectType = "значение нарушения целостности";
                        newLog.LogCurState = parsedThreatList[i].IntegrityViolation ? "Да" : "Нет";
                        newLog.LogPrevState = localThreatList[i].IntegrityViolation ? "Да" : "Нет";
                        ChangeLogObhect.LogCountChng++;
                        changelog.Add(newLog);
                    }
                    if (localThreatList[i].AvailabilityViolation != parsedThreatList[i].AvailabilityViolation)
                    {
                        ChangeLogObhect newLog = new ChangeLogObhect();
                        newLog.LogType = "Изменение";
                        newLog.LogObjId = localThreatList[i].ThreatId;
                        newLog.LogObjectType = "значение нарушения доступности";
                        newLog.LogCurState = parsedThreatList[i].AvailabilityViolation ? "Да" : "Нет";
                        newLog.LogPrevState = localThreatList[i].AvailabilityViolation ? "Да" : "Нет";
                        ChangeLogObhect.LogCountChng++;
                        changelog.Add(newLog);
                    }
                    if (localThreatList[i].ConfidentialityViolation != parsedThreatList[i].ConfidentialityViolation)
                    {
                        ChangeLogObhect newLog = new ChangeLogObhect();
                        newLog.LogType = "Изменение";
                        newLog.LogObjId = localThreatList[i].ThreatId;
                        newLog.LogObjectType = "значение нарушения конфиденциальности";
                        newLog.LogCurState = parsedThreatList[i].ConfidentialityViolation ? "Да" : "Нет";
                        newLog.LogPrevState = localThreatList[i].ConfidentialityViolation ? "Да" : "Нет";
                        ChangeLogObhect.LogCountChng++;
                        changelog.Add(newLog);
                    }
                }
            }
            if (diff>0)
            {
                for (int i = localThreatList.Count; i < parsedThreatList.Count; i++)
                {
                    ChangeLogObhect newLog = new ChangeLogObhect();
                    newLog.LogType = "Добавление";
                    newLog.LogObjId = parsedThreatList[i].ThreatId;
                    newLog.LogObjectType = "Следующие поля";
                    newLog.LogCurState = $"Добавлена УБИ{parsedThreatList[i].ThreatId} {parsedThreatList[i].ThreatName}:\nОписание угрозы: {parsedThreatList[i].ThreatInfo}\nИсточник угрозы: {parsedThreatList[i].ThreatSource}\nОбъект воздействия угрозы: {parsedThreatList[i].ThreaObj}\nНарушение конфиденциальности: {(parsedThreatList[i].ConfidentialityViolation ? "Да" : "Нет")}\nНарушение целостности: {(parsedThreatList[i].IntegrityViolation ? "Да" : "Нет")}\nНарушение доступности: {(parsedThreatList[i].AvailabilityViolation ? "Да" : "Нет")}\n";
                    newLog.LogPrevState = "-";
                    ChangeLogObhect.LogCountNew++;
                    changelog.Add(newLog);

                }
            }

            localThreatList = parsedThreatList;
            parsedThreatList = new List<ISThreat>();

        }
        private void ChangeLog_Button_Click(object sender, RoutedEventArgs e)
        {
            ChangeLog l = new ChangeLog();
            l.Show();
        }
    }
}
