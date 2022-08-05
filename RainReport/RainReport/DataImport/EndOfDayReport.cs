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
    public class EndOfDayReport
    {
        private List<EndOfDayReportRow> _records;
        public IReadOnlyCollection<EndOfDayReportRow> Records => _records.AsReadOnly();

        public void ReadFile(string filePath)
        {
            using (var streamReader = new StreamReader(filePath))
            {
                using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    _records = csvReader.GetRecords<EndOfDayReportRow>().ToList();
                }
            }
        }

        public List<EndOfDayReportRow> GetRecords()
        {
            return _records;
        }
    }
}
