using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainReport
{
    public class DailySalesReport
    {
        public List<Transaction> transactions = new();
        public List<TransactionItem> transactionDetails = new();

        public void RunReport(EndOfDayReport eodReport, TransactionDetailsReport transactionsReport)
        {
            transactions = eodReport.GetRecords();
            transactionDetails = transactionsReport.GetRecords();

            foreach (var transaction in transactions)
            {
                AddTransactionToReport(transaction);
            }

            ExportReport();
        }

        private void AddTransactionToReport(Transaction transaction)
        {
            List<TransactionItem> itemsToAdd = new();
            itemsToAdd = FetchAllItemsForTransaction(transaction.TransactionID);
            transaction.items = itemsToAdd;
        }

        private List<TransactionItem> FetchAllItemsForTransaction(int id)
        {
            List<TransactionItem> items = new();

            foreach (TransactionItem item in transactionDetails)
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
            ReportExporter exporter = new();
            exporter.ExportReport(this);
        }
    }
}
