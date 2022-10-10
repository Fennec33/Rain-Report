using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainReport
{
    public class SalesData
    {
        public List<Transaction> transactions;
        private List<TransactionItem> _items;

        public void AsembleDataFrom(EndOfDayData eodReport, TransactionDetailsData transactionsReport)
        {
            transactions = eodReport.GetRecords();
            _items = transactionsReport.GetRecords();

            foreach (var transaction in transactions)
            {
                AddItemsToTransaction(transaction);
            }
        }

        private void AddItemsToTransaction(Transaction transaction)
        {
            List<TransactionItem> itemsToAdd = new();
            itemsToAdd = FetchAllItemsForTransaction(transaction.TransactionID);
            transaction.items = itemsToAdd;
        }

        private List<TransactionItem> FetchAllItemsForTransaction(int id)
        {
            List<TransactionItem> items = new();

            foreach (TransactionItem item in _items)
            {
                if (item.ID == id)
                {
                    items.Add(item);
                }
            }

            return items;
        }
    }
}
