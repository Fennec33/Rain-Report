using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainReport
{
    public class TableFormatter
    {
        public const int maxWidth = 85;
        private int[] _colWidth;

        //Total of all colWidth must be under maxWidth;
        public TableFormatter(int[] colWidth)
        {
            _colWidth = colWidth;
        }

        public string Format(string[] row)
        {
            string result = "";
            int dif = 0;

            for (int i = 0; i < row.Length; i++)
            {
                if (i > _colWidth.Length)
                    continue;

                dif = _colWidth[i] - row[i].Length;

                if (dif > 0)
                {
                    row[i] = row[i].PadRight(_colWidth[i]);
                }
                else if (dif < 0)
                {
                    row[i] = row[i].Remove(_colWidth[i] - 1);
                    row[i] += ' ';
                }
                else
                {
                    row[i] = row[i].Remove(_colWidth[i] - 1);
                    row[i] += ' ';
                }
                
                result += row[i];
            }

            if (result.Length > maxWidth)
                result = result.Remove(maxWidth - 1);

            return result;
        }

    }
}
