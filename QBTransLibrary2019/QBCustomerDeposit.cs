using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QBTransactions;
using QBTransactions.ViewModels;
using System.Xml;
using QBTransLibrary2019;

namespace QBTransactions.ViewModels
{
    public class QBCustomerDeposit
    {
        private CSIAutomationEntities context = new CSIAutomationEntities();
        private List<Transaction> Transactions = new List<Transaction>();
        private decimal closingBalance = 0;
        private decimal openingBalance = 0;

        public void CreateExcelReport(string customer, DateTime AsOfDate)
        {
            ParseTransaction(customer, AsOfDate);
            openingBalance = GetOpeningBalance(customer, AsOfDate);
            closingBalance = GetClosingBalance();
            new ExcelTemplate(Transactions, customer, openingBalance, closingBalance, AsOfDate);
        }

        private decimal GetClosingBalance()
        {
            return Transactions.Sum(p => p.Amount);
        }

        private void ParseTransaction(string customer, DateTime dateBegin)
        {
            Transactions.Clear();

            var temp = context.QBTransactions.Count();


            List<QBTransaction> transactions = (from s in context.QBTransactions
                                                where s.Date > dateBegin && s.CustomerName.Contains(customer)
                                                orderby s.Date ascending
                                                select s).ToList();

            foreach (QBTransaction qbTransaction in transactions)
            {
                Transaction transaction = new Transaction();
                transaction.Amount = qbTransaction.Amount;
                transaction.TransactionDate = qbTransaction.Date;
                transaction.Type = qbTransaction.Type;
                transaction.Memo = qbTransaction.Memo;
                transaction.TransactionNumber = qbTransaction.Num;
                if (transaction.Type == "Invoice")
                {
                    // Get the line items
                    transaction.Items = GetInvoiceItems(qbTransaction.theXML);
                }
                else
                {
                    transaction.Items = new List<string>();
                }
                Transactions.Add(transaction);
            }

        }

        private List<string> GetInvoiceItems(string theXml)
        {
            List<string> result = new List<string>();
            string y = "";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(theXml);

            if (doc.DocumentElement != null)
            {
                XmlNodeList invNodes = doc.DocumentElement.SelectNodes("/InvoiceRet/InvoiceLineGroupRet/InvoiceLineRet");

                if (invNodes != null)
                    foreach (XmlNode node in invNodes)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            if (child.Name == "Desc")
                            {
                                y = child.InnerText + "  TransactionAmount: $";

                            }
                            if (child.Name == "Amount")
                            {
                                y += child.InnerText;
                                result.Add(y);
                            }
                        }
                    }
            }


            if (result.Count == 0)
            {
                if (doc.DocumentElement != null)
                {
                    XmlNodeList invNodeList = doc.DocumentElement.SelectNodes("/InvoiceRet/InvoiceLineRet");


                    if (invNodeList != null)
                        foreach (XmlNode first in invNodeList)
                        {
                            foreach (XmlNode child in first.ChildNodes)
                            {
                                if (child.Name == "Desc")
                                {
                                    y = child.InnerText + "  TransactionAmount: $";

                                }
                                if (child.Name == "Amount")
                                {
                                    y += child.InnerText;
                                    if (!y.ToUpper().Contains("DEDUCT FROM CUSTOMER DEPOSIT")) result.Add(y);
                                }
                            }
                        }
                }
            }
            return result;
        }

        private decimal GetOpeningBalance(string customer, DateTime dateBegin)
        {
            try
            {
                CustomerDepositBalanceStatic initBalance = (from b in context.CustomerDepositBalanceStatics
                                                            where b.CustomerName.Equals(customer)
                                                            select b).FirstOrDefault();
                DateTime aiDate = initBalance.AsOfDate;
                decimal balance = initBalance.Balance;
                List<QBTransaction> transactions = (from s in context.QBTransactions
                                                    where s.Date < dateBegin && s.Date > aiDate && s.CustomerName.Contains(customer)
                                                    select s).ToList();
                decimal newTransactions = (transactions.Sum(p => p.Amount));
                return balance + newTransactions;
            }
            catch (Exception) { }
            return 0;
        }

        public CustomerDepositBalanceStatic GetStaticDateAndBalance(string customerName)
        {
            return (from b in context.CustomerDepositBalanceStatics
                    where b.CustomerName.Equals(customerName)
                    select b).FirstOrDefault();
        }

        public string GetDateOfLastPull(string custName)
        {
            try
            {
                return (from s in context.QBTransactions
                        where s.CustomerName.Contains(custName)
                        select s.Date).Max().ToShortDateString();
            }
            catch (Exception)
            {
                return "";
            }
        }

    }
}
