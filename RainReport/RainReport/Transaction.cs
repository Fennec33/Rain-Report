using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace RainReport
{
    public class Transaction
    {
        public List<TransactionItem> items;

        [Name("Transaction ID")]
        public int TransactionID { get; set; }

        [Name("Employee")]
        public string Employee { get; set; }

        [Name("Date")]
        public DateTime Date { get; set; }

        [Name("Customer")]
        public string Customer { get; set; }

        [Name("Payment Type")]
        public string PaymentType { get; set; }

        [Name("Sub Total")]
        public string SubTotal { private get; set; }

        [Name("Discount")]
        public string Discount { private get; set; }

        [Name("Trade-In Credit")]
        public string TradeInCredit { private get; set; }

        [Name("Tax")]
        public string Tax { private get; set; }

        [Name("Shipping")]
        public string Shipping { private get; set; }

        [Name("Total")]
        public string Total { private get; set; }

        public bool IsMajorItemTransaction()
        {
            foreach (var item in items)
            {
                if (Categories.IsThisAMajorItem(item))
                    return true;
            }
            return false;
        }

        public bool IsCommisionableTransaction()
        {
            foreach (var item in items)
            {
                if (Categories.IsThisCommisonable(item))
                    return true;
            }
            return false;
        }

        public bool ContainsPartialPayments()
        {
            if (PartialPaymentAmount() != 0)
                return true;
            else
                return false;
        }

        public int PartialPaymentAmount()
        {
            int itemSales = 0;

            foreach (var item in items)
                itemSales += item.GetSalesTotal();

            return GetSalesTotal() - itemSales;
        }

        public int GetSalesTotal()
        {
            return GetSubTotalInCents() + GetDiscountInCents() + GetTradeInCreditInCents();
        }

        public void DistriputeTradeInCredit()
        {
            int creditToDistripute = Math.Abs(GetTradeInCreditInCents());

            //reset credits before reassign
            foreach (var item in items)
                item.tradeInCredit = 0;

            //assign to major items
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].IsAccessoryItem())
                    continue;

                int tempSalesTotal = items[i].GetSalesTotal();

                if (tempSalesTotal >= creditToDistripute)
                {
                    items[i].tradeInCredit = creditToDistripute;
                    return;
                }
                else
                {
                    items[i].tradeInCredit = tempSalesTotal;
                    creditToDistripute -= tempSalesTotal;
                }
            }

            //assign to accessoroy items
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].IsMajorItem())
                    continue;

                int tempSalesTotal = items[i].GetSalesTotal();

                if (tempSalesTotal >= creditToDistripute)
                {
                    items[i].tradeInCredit = creditToDistripute;
                    return;
                }
                else
                {
                    items[i].tradeInCredit = tempSalesTotal;
                    creditToDistripute -= tempSalesTotal;
                }
            }
        }

        public int GetSubTotalInCents() => Categories.ToCents(SubTotal);
        public int GetDiscountInCents() => Categories.ToCents(Discount);
        public int GetTradeInCreditInCents() => Categories.ToCents(TradeInCredit);
        public int GetTaxInCents() => Categories.ToCents(Tax);
        public int GetShippingInCents() => Categories.ToCents(Shipping);
        public int GetTotalInCents() => Categories.ToCents(Total);
    }
}