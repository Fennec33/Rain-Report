using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace RainReport.DataImport
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
        public float Retail { get; set; }

        [Name("Discount")]
        public float Discount { get; set; }

        [Name("Tax Collected")]
        public float TaxCollected { get; set; }

        [Name("Cost")]
        public float Cost { get; set; }

        [Name("Liability")]
        public float Liability { get; set; }

        [Name("Profit")]
        public float Profit { get; set; }

        [Name("Margin")]
        public float Margin { get; set; }

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

        public float salesTotal;
        public bool isCommisonable;
        public bool isMajorItem;
    }
}