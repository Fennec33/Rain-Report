using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainReport.GenerateReport
{
    public class TransactionItem
    {
        public string itemName = "";
        public string department = "";
        public float qty;
        public float retail;
        public float discount;
        
        
        public float totalSales;

        public bool isCommisonable;
        public bool isMajorItem;
    }
}
