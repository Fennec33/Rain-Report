using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainReport.GenerateReport
{
    public class Transaction
    {
        public List<TransactionItem> items = new();

        public int transactionID;
        public string salesClerk = "";
        public float subTotal;
        public float discount;
        public float tradeInCredit;
        public float tax;
        public float shipping;
        public float totalSales;

        public bool ifMajorItemTransaction;
        public bool ifNonCommisionableTransaction;
    }
}
