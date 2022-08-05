using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RainReport.DataImport;

namespace RainReport.GenerateReport
{
    public class DailySalesReport
    {
        public List<Transaction> transactions = new();
        public List<EndOfDayReportRow> endOfDayReports = new();
        public List<TransactionDetailsReportRow> transactionDetails = new();

        public void RunReport(EndOfDayReport eodReport, TransactionDetailsReport transactionsReport)
        {
            endOfDayReports = eodReport.GetRecords();
            transactionDetails = transactionsReport.GetRecords();

            foreach (var transaction in endOfDayReports)
            {
                AddTransactionToReport(transaction);
            }

            ExportReport();
        }

        private void AddTransactionToReport(EndOfDayReportRow transaction)
        {
            Transaction newT = new Transaction();
            newT.transactionID = transaction.TransactionID;
            newT.salesClerk = transaction.Employee;
            newT.subTotal = transaction.SubTotal;
            newT.discount = transaction.Discount;
            newT.tradeInCredit = transaction.TradeInCredit;
            newT.tax = transaction.Tax;
            newT.shipping = transaction.Shipping;
            newT.totalSales = transaction.Total;

            List<TransactionDetailsReportRow> itemsToAdd = new();
            itemsToAdd = FetchAllItemsForTransaction(newT.transactionID);

            foreach (var item in itemsToAdd)
            {
                TransactionItem newItem = new();

                newItem.itemName = item.ItemName;
                newItem.department = item.Department;
                newItem.qty = item.Qty;
                newItem.retail = item.Retail;
                newItem.discount = item.Discount;

                newT.items.Add(newItem);
            }

            transactions.Add(newT);
        }

        private List<TransactionDetailsReportRow> FetchAllItemsForTransaction(int id)
        {
            List<TransactionDetailsReportRow> items = new();

            foreach (TransactionDetailsReportRow item in transactionDetails)
            {
                if (item.ID == id)
                {
                    items.Add(item);
                }
            }

            return items;
        }

        private void ExportReport()
        {
            //TODO
        }
    }
}
