using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainReport
{
    public class ReportExporter
    {
        List<string> linesToPrint = new List<string>();

        public async Task ExportReport()
        {
            await File.WriteAllLinesAsync("RainReportOutput.txt", linesToPrint);
        }

        public void QueLines(string[] lines)
        {
            foreach(string line in lines)
                linesToPrint.Add(line);
        }

        public void QueLine(string line)
        {
            linesToPrint.Add(line);
        }

        public void QueBreak()
        {
            linesToPrint.Add("");
        }

        public void QueBreak(int num)
        {
            for (int i = 0; i < num; i++)
                linesToPrint.Add("");
        }

        public void QueHorizon()
        {
            linesToPrint.Add("".PadRight(TableFormatter.maxWidth, '-'));
        }
    }
}
