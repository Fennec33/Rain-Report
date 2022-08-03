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
    internal class EndOfDayReport
    {
        private List<EndOfDayReportRow> _records;
        public IReadOnlyCollection<EndOfDayReportRow> Records => _records.AsReadOnly();

        public void ReadFile(string filePath)
        {
            using (var streamREader = new StreamReader(filePath))
            {
                using (var csvReader = new CsvReader(streamREader, CultureInfo.InvariantCulture))
                {
                    _records = csvReader.GetRecords<EndOfDayReportRow>().ToList();
                }
            }
        }
    }
}