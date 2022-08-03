using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System.IO;
using System.Globalization;
using RainReport.DataImport;

namespace RainReport
{
    internal class ReportImporter
    {
        private List<string> _endOfDayReportHeaders;
        private List<string> _transactionDetailsHeaders;

        public ReportImporter()
        {
            SetupHeaderLists();
        }

        public void AddReport(string filePath)
        {
            List<string> headers;

            using (var streamReader = new StreamReader(filePath))
            {
                using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    csvReader.Read();
                    csvReader.ReadHeader();

                    headers = csvReader.HeaderRecord.ToList();
                }
            }

            if (headers.Equals(_endOfDayReportHeaders))
            {
                EndOfDayReport report = new EndOfDayReport();
                report.ReadFile(filePath);
            }
        }

        private void SetupHeaderLists()
        {
            _endOfDayReportHeaders.Add("Transaction ID");
            _endOfDayReportHeaders.Add("Employee");
            _endOfDayReportHeaders.Add("Date");
            _endOfDayReportHeaders.Add("Customer");
            _endOfDayReportHeaders.Add("Payment Type");
            _endOfDayReportHeaders.Add("Sub Total");
            _endOfDayReportHeaders.Add("Discount");
            _endOfDayReportHeaders.Add("Trade-In Credit");
            _endOfDayReportHeaders.Add("Tax");
            _endOfDayReportHeaders.Add("Shipping");
            _endOfDayReportHeaders.Add("Total");

            _transactionDetailsHeaders.Add("id");
            _transactionDetailsHeaders.Add("Date");
            _transactionDetailsHeaders.Add("Item Name");
            _transactionDetailsHeaders.Add("Department");
            _transactionDetailsHeaders.Add("SKU");
            _transactionDetailsHeaders.Add("Qty");
            _transactionDetailsHeaders.Add("Retail");
            _transactionDetailsHeaders.Add("Discount");
            _transactionDetailsHeaders.Add("Tax Collected");
            _transactionDetailsHeaders.Add("Cost");
            _transactionDetailsHeaders.Add("Liability");
            _transactionDetailsHeaders.Add("Profit");
            _transactionDetailsHeaders.Add("Margin");
            _transactionDetailsHeaders.Add("Customer");
            _transactionDetailsHeaders.Add("Company");
            _transactionDetailsHeaders.Add("Email");
            _transactionDetailsHeaders.Add("Phone");
            _transactionDetailsHeaders.Add("Address");
            _transactionDetailsHeaders.Add("City");
            _transactionDetailsHeaders.Add("State");
            _transactionDetailsHeaders.Add("Zip");
            _transactionDetailsHeaders.Add("Country");
            _transactionDetailsHeaders.Add("Serial Number");
            _transactionDetailsHeaders.Add("Store Location");
            _transactionDetailsHeaders.Add("Sales Person");
            _transactionDetailsHeaders.Add("Note");
        }
    }
}
