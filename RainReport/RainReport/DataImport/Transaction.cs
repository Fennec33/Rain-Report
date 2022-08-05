﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace RainReport.DataImport
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
        public float SubTotal { get; set; }

        [Name("Discount")]
        public float Discount { get; set; }

        [Name("Trade-In Credit")]
        public float TradeInCredit { get; set; }

        [Name("Tax")]
        public float Tax { get; set; }

        [Name("Shipping")]
        public float Shipping { get; set; }

        [Name("Total")]
        public float Total { get; set; }

        public bool ifMajorItemTransaction;
        public bool ifNonCommisionableTransaction;

        public void DoAThing()
        {
            //TODO
        }
    }
}