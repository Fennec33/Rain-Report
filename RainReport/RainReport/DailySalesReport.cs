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
        public List<Transaction> endOfDayReports = new();
        public List<TransactionItem> transactionDetails = new();

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

        private void AddTransactionToReport(Transaction transaction)
        {
            Transaction newT = new Transaction();

            List<TransactionItem> itemsToAdd = new();
            //itemsToAdd = FetchAllItemsForTransaction(newT.transactionID);

            foreach (var item in itemsToAdd)
            {
                TransactionItem newItem = new();

                newT.items.Add(newItem);
            }

            transactions.Add(newT);
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
            //TODO
        }
    }
}
