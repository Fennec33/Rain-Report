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
    public class TransactionItem
    {
        [Name("id")]
        public int ID { get; set; }

        [Name("Date")]
        public DateTime Date { get; set; }

        [Name("Item Name")]
        public string ItemName { get; set; }

        [Name("Department")]
        public string Department { get; set; }

        [Name("SKU")]
        public string SKU { get; set; }

        [Name("Qty")]
        public int Qty { get; set; }

        [Name("Retail")]
        public string Retail { private get; set; }

        [Name("Discount")]
        public string Discount { private get; set; }

        [Name("Tax Collected")]
        public string TaxCollected { private get; set; }

        [Name("Cost")]
        public string Cost { private get; set; }

        [Name("Liability")]
        public string Liability { private get; set; }

        [Name("Profit")]
        public string Profit { private get; set; }

        [Name("Margin")]
        public string Margin { get; set; }

        [Name("Customer")]
        public string Customer { get; set; }

        [Name("Company")]
        public string Company { get; set; }

        [Name("Email")]
        public string Email { get; set; }

        [Name("Phone")]
        public string Phone { get; set; }

        [Name("Address")]
        public string Address { get; set; }

        [Name("City")]
        public string City { get; set; }

        [Name("State")]
        public string State { get; set; }

        [Name("Zip")]
        public string Zip { get; set; }

        [Name("Country")]
        public string Country { get; set; }

        [Name("Serial Number")]
        public string SerialNumber { get; set; }

        [Name("Store Location")]
        public string StoreLocation { get; set; }

        [Name("Sales Person")]
        public string SalesPerson { get; set; }

        [Name("Note")]
        public string Note { get; set; }

        public int tradeInCredit = 0;

        public int GetSalesTotal()
        {
            return GetRetailInCents() + GetDiscountInCents() - tradeInCredit;
        }

        public bool IsCommisonable()
        {
            return Categories.IsThisCommisonable(this);
        }

        public bool IsMajorItem()
        {
            return Categories.IsThisAMajorItem(this);
        }

        public bool IsAccessoryItem()
        {
            return Categories.IsThisAnAccessory(this);
        }

        public int GetRetailInCents() => Categories.ToCents(Retail);
        public int GetDiscountInCents() => Categories.ToCents(Discount);
        public int GetTaxCollectedInCents() => Categories.ToCents(TaxCollected);
        public int GetCostInCents() => Categories.ToCents(Cost);
        public int GetLiabilityInCents() => Categories.ToCents(Liability);
        public int GetProfitInCents() => Categories.ToCents(Profit);

    }
}