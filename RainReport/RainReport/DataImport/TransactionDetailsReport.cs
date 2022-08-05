﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using System.IO;
using System.Globalization;

namespace RainReport.DataImport
{
    public class TransactionDetailsReport
    {
        private List<TransactionItem> _records;
        public IReadOnlyCollection<TransactionItem> Records => _records.AsReadOnly();

        public void ReadFile(string filePath)
        {
            using (var streamReader = new StreamReader(filePath))
            {
                using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    _records = csvReader.GetRecords<TransactionItem>().ToList();
                }
            }
        }

        public List<TransactionItem> GetRecords()
        {
            return _records;
        }
    }
}
