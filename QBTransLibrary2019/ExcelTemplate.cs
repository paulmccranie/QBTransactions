using QBTransactions.ViewModels;
using QBTransLibrary2019;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace QBTransactions
{
    public class ExcelImportBalances
    {
        ExcelEngine excelEngine = new ExcelEngine();
        private IApplication xlApplication;
        IWorkbook workbook;
        private IWorksheet worksheet;
        CSIAutomationEntities context = new CSIAutomationEntities();
        private DateTime AsOfDate;


        // Code that I use to get benchmark balances from Quickbooks -
        // Currently I'm using 1/1/14 as benchmark
        public ExcelImportBalances()
        {
            string custName = "";
            decimal balance = 0;
            AsOfDate = DateTime.Parse("1/1/2014");

            xlApplication = excelEngine.Excel;
            workbook = excelEngine.Excel.Workbooks.Open(@"C:\Users\pmccranie\Desktop\cb.xlsx");  //C:\Users\pmccranie\Desktop
            worksheet = workbook.Worksheets[1];
            int rowIndex = 2;

            while (true)
            {
                custName = worksheet.Range["B" + rowIndex].Text;
                balance = decimal.Parse(worksheet.Range["J" + rowIndex].Value);
                AddCustomerBalance(custName, balance);
                rowIndex += 2;
                try
                {
                    if (worksheet.Range["A" + rowIndex].Text.ToString().Contains("TOTAL")) break;
                }
                catch (Exception)
                {


                }

            }

        }

        private void AddCustomerBalance(string custName, decimal balance)
        {
            // Add to database
            CustomerDepositBalanceStatic bal = new CustomerDepositBalanceStatic
            {
                AsOfDate = AsOfDate,
                Balance = balance,
                CustomerName = custName
            };
            context.CustomerDepositBalanceStatics.Add(bal);
            context.SaveChanges();
        }
    }

    public class ExcelTemplate
    {
        ExcelEngine excelEngine = new ExcelEngine();
        private IApplication xlApplication;
        IWorkbook workbook;
        private IWorksheet worksheet;


        private string TheCustomerName = "";
        private string fileName = "";
        private int TheCustomerID = 2001;
        private int rowCounter = 0;
        private string footerText = "";
        private int finalRowCounter = 0;
        private string reportTitle = "Transaction List for ";

        private string[] columnCodes;
        private decimal openingBalance;
        private decimal closingBalance;
        private DateTime AsOfDate;

        public ExcelTemplate(List<Transaction> allTransactions, string companyName, decimal openBalance, decimal closeBalance, DateTime asofdate)
        {
            try
            {
                reportTitle += companyName;
                TheCustomerName = companyName;
                openingBalance = openBalance;
                closingBalance = closeBalance;
                AsOfDate = asofdate;
                CreateSpreadsheet(1, "Transactions");
                PopulateWorksheets(allTransactions);
                SaveSpreadSheet();
                StartExcelFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }


        public ExcelTemplate(int custID, string worksheetName, string theReportTitle, string customFooter)
        {


        }


        public void SaveSpreadSheet()
        {
            NameTheFile();
            workbook.Version = ExcelVersion.Excel2010;
            try
            {
                workbook.SaveAs(fileName);
            }
            catch (Exception)
            {

                SaveFileOverride(1);
            }

            workbook.Close();
            excelEngine.Dispose();
        }

        private void SaveFileOverride(int counter)
        {
            if (counter > 25)
            {
                MessageBox.Show("Unable to save the spreadsheet.");
                return;
            }


            try
            {
                NameTheFile(counter);
                workbook.SaveAs(fileName);
            }

            catch (Exception)
            {
                SaveFileOverride(counter+1);
            }
        }

        public void StartExcelFile()
        {
            try
            {
                //Launching the Excel file using the default Application.[MS Excel Or Free ExcelViewer]
                System.Diagnostics.Process.Start(fileName);
            }
            catch (Win32Exception ex)
            {
                MessageBox.Show("MS Excel is not installed in this system");
                Console.WriteLine(ex.ToString());
            }
        }

        private void NameTheFile(int optionVal = 0)
        {
            string path = @"\\csi-app\shared\Software Staging\QB Transactions App\Spreadsheets\";
            if (optionVal > 0)
            {
                fileName = path + reportTitle + " Transactions " + "Copy " + optionVal + ".xlsx";
                return;
            }

            fileName = path + reportTitle + " Transactions "  + ".xlsx";

        }

        public void CreateSpreadsheet(int numberOfWorksheets, string wName)
        {
            xlApplication = excelEngine.Excel;
            workbook = excelEngine.Excel.Workbooks.Create();
            //workbook = excelEngine.Excel.Workbooks.Add(@"\\csi-app\shared\Software Staging\CompassGamma\Docs\InitialStatusReportTemplate.xltx");//xlApplication.Workbooks.Create(numberOfWorksheets);

            worksheet = workbook.Worksheets[0];
            worksheet.Name = wName;
        }

        private void PopulateWorksheets(List<Transaction> transactions)
        {
            CreateHeaders();
            CreateBody(transactions);
            Format();
        }

        private void Format()
        {
            worksheet.Rows[0].RowHeight = 30;

            IRange usedRange = worksheet.UsedRange;
            usedRange.CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
            usedRange.RowHeight = 30;

            // Create a Table out of the data
            IListObject theTable = worksheet.ListObjects.Create("theTable", worksheet["A1:E" + finalRowCounter]);
            theTable.BuiltInTableStyle = TableBuiltInStyles.TableStyleLight11;
            worksheet.UsedRange.AutofitColumns();

            // set the footer Row high
            worksheet.Range["A" + rowCounter].RowHeight = 175;
            worksheet.HPageBreaks.Clear();
            worksheet.VPageBreaks.Clear();


        }

        private void CreateBody(List<Transaction> transactions)
        {
            DateTime tempDate;
            string[] marker = new string[] { "TransactionAmount:" };
            worksheet.Range["D2"].HorizontalAlignment = ExcelHAlign.HAlignRight;
            worksheet.Range["D2"].CellStyle.Font.Bold = true;
            worksheet.Range["D2"].Text = "Opening Balance (As of " + AsOfDate.ToShortDateString() + ")    ";

            worksheet.Range["E2"].Value = openingBalance.ToString();
            worksheet.Range["E2"].CellStyle.Font.Bold = true;

            rowCounter = 3;

            try
            {
                foreach (Transaction transaction in transactions)
                {
                    if (transaction.Amount > 15000)
                    {
                        int i = 1;
                    }
                    worksheet.Range["B" + rowCounter].Text = transaction.TransactionNumber;
                    worksheet.Range["C" + rowCounter].Text = transaction.TransactionDate.ToShortDateString();
                    worksheet.Range["C" + rowCounter].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;

                    worksheet.Range["D" + rowCounter].Text = transaction.Memo + " Amount: $" + transaction.Amount.ToString();
                    worksheet.Range["D" + rowCounter].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                    worksheet.Range["D" + rowCounter].CellStyle.Font.Bold = true;
                    worksheet.Range["D" + rowCounter].CellStyle.Font.Size = 10;

                    if (transaction.Type != "Invoice")
                    {
                        worksheet.Range["A" + rowCounter].Text = transaction.Type;
                        worksheet.Range["E" + rowCounter].Value = transaction.Amount.ToString();
                        worksheet.Range["E" + rowCounter].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                        worksheet.Range["E" + rowCounter].CellStyle.Font.Bold = true;
                        worksheet.Range["E" + rowCounter].CellStyle.Font.Size = 10;
                        worksheet.Range["E" + rowCounter].CellStyle.NumberFormat = "$#,##0.00"; 
                    }
                    rowCounter += 1;

                    if (transaction.Items.Count > 0)
                    {
                        foreach (string detail in transaction.Items)
                        {
                            worksheet.Range["A" + rowCounter].Text = transaction.Type;

                            worksheet.Range["C" + rowCounter].Text = transaction.TransactionDate.ToShortDateString();
                            worksheet.Range["C" + rowCounter].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;


                            string[] detailParts = detailParts = detail.Split(marker, StringSplitOptions.None);

                            worksheet.Range["D" + rowCounter].Text = "     " + detailParts[0];
                            worksheet.Range["E" + rowCounter].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                            worksheet.Range["E" + rowCounter].CellStyle.Font.Bold = true;
                            worksheet.Range["E" + rowCounter].CellStyle.Font.Size = 10;
                            worksheet.Range["E" + rowCounter].CellStyle.Font.Color = ExcelKnownColors.Red2;
                            worksheet.Range["E" + rowCounter].CellStyle.NumberFormat = "$#,##0.00";
                            decimal amount = (detailParts[0].Contains("Customer Deposit")
                                ? decimal.Parse(detailParts[1].Replace("$", ""))
                                : decimal.Parse(detailParts[1].Replace("$", "")) * -1);
                            worksheet.Range["E" + rowCounter].Value = amount.ToString();
                            if (amount < 0)
                            {
                                worksheet.Range["E" + rowCounter].CellStyle.Font.Color = ExcelKnownColors.Red2;
                            }
                            else
                            {
                                worksheet.Range["A" + rowCounter].Text = "Deposit";
                                worksheet.Range["E" + rowCounter].CellStyle.Font.Color = ExcelKnownColors.Black;
                            }
                            
                            rowCounter += 1;
                        }
                        }
                    else
                    {
                        
                    }
                }
                worksheet.Range["D" + rowCounter].HorizontalAlignment = ExcelHAlign.HAlignRight;
                worksheet.Range["D" + rowCounter].CellStyle.Font.Bold = true;
                worksheet.Range["D" + rowCounter].Text = "Total    ";

                worksheet.Range["E" + rowCounter].Formula = "=SUBTOTAL(109,E4:E" +
                                                                       (rowCounter - 1).ToString() + ")";
                worksheet.Range["E" + rowCounter].CellStyle.Font.Bold = true;
                worksheet.Range["E" + rowCounter].CellStyle.NumberFormat = "$#,##0.00";
                rowCounter += 1;
                worksheet.Range["D" + rowCounter].HorizontalAlignment = ExcelHAlign.HAlignRight;
                worksheet.Range["D" + rowCounter].CellStyle.Font.Bold = true;
                worksheet.Range["D" + rowCounter].Text = "Closing Balance    ";

                worksheet.Range["E" + rowCounter].Formula = "=E2+E" + (rowCounter - 1).ToString();
                worksheet.Range["E" + rowCounter].CellStyle.Font.Bold = true;
                worksheet.Range["E" + rowCounter].CellStyle.NumberFormat = "$#,##0.00";
                
                // temp show actual deposit balance
                using (CSIAutomationEntities context = new CSIAutomationEntities())
                {
                    decimal? temp = (from b in context.QBCustomerNames
                        where b.CustomerName == TheCustomerName
                        select b.DepositAmount).FirstOrDefault();
                    worksheet.Range["F" + rowCounter].Value = temp.ToString();
                }

                rowCounter += 1;


                finalRowCounter = rowCounter;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void CreateHeaders()
        {
            int rowValue = 1;

            worksheet.Range["A" + rowValue].Text = "TYPE";
            worksheet.Range["B" + rowValue].Text = "TXN #";
            worksheet.Range["C" + rowValue].Text = "SERVICE DATE";
            worksheet.Range["D" + rowValue].Text = "DESC";
            worksheet.Range["E" + rowValue].Text = "AMOUNT";


            worksheet.Range["A" + rowValue + ":I" + rowValue].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
            worksheet.Range["A" + rowValue + ":I" + rowValue].CellStyle.Font.Bold = true;

        }






    }
}




//    //worksheet.Range["F" + rowCounter].Text = GetDaysInCurrentStatus(theServiceItem);

//    worksheet.Range["E" + rowCounter].Text = (theServiceItem.AtState == true) ? "X (" + GetDaysInCurrentStatus(theServiceItem) + ")" : "";
//    worksheet.Range["E" + rowCounter].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
//    worksheet.Range["E" + rowCounter].CellStyle.Font.Bold = true;
//    worksheet.Range["E" + rowCounter].CellStyle.Font.Size = 10;

//    // worksheet.Range["H" + rowCounter].Text = GetDaysInCurrentStatus(theServiceItem);

//    worksheet.Range["F" + rowCounter].Text = GetEstimatedStateProcessingTime(theServiceItem);
//    worksheet.Range["F" + rowCounter].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;

//    if (!string.IsNullOrEmpty(worksheet.Range["F" + rowCounter].Text))
//    {
//        worksheet.Range["F" + rowCounter].Text += " days";
//    }

//    if (!string.IsNullOrEmpty(theServiceItem.LicenseNumber))
//    {
//        worksheet.Range["G" + rowCounter].Text = "YES";
//        worksheet.Range["G" + rowCounter].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
//        worksheet.Range["H" + rowCounter].Text = theServiceItem.LicenseNumber;
//        worksheet.Range["H" + rowCounter].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
//    }

//    try
//    {
//        DateTime.TryParse(theServiceItem.DateExpiration.ToString(), out tempDate);

//        if (tempDate > DateTime.Now.AddYears(-20))
//        {
//            worksheet.Range["I" + rowCounter].Text = tempDate.ToShortDateString();
//            worksheet.Range["I" + rowCounter].CellStyle.NumberFormat = "m/d/yyyy";
//            worksheet.Range["I" + rowCounter].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
//        }

//    }

//    catch (Exception)
//    {

//    }


//    // Make row font bold for licenses
//    if (theServiceItem.Name.Contains("License"))
//    {
//        worksheet.Range["A" + rowCounter + ":I" + rowCounter].CellStyle.Font.Bold = true;
//    }

//    rowCounter += 1;
//    // Highlight Green if all jurisdictional requirements are met

//}
//finalRowCounter = rowCounter - 1;

//rowCounter += 1;
//worksheet.Range["A" + rowCounter].WrapText = true;
//worksheet.Range["A" + rowCounter].RowHeight = 175;
//worksheet.Range["A" + rowCounter].Text = footerText;