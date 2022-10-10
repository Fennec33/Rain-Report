using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using System.IO;
using System.Globalization;

namespace RainReport
{
    public class TransactionDetailsData
    {
        private List<TransactionItem> _records;
        public List<TransactionItem> GetRecords() => _records;

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
    }
}
