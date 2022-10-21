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
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string date = DateTime.Now.ToString("M-d-yyyy");
            string fileName = path + "\\" + date + " Daily Sales Rep Report.txt";

            await File.WriteAllLinesAsync(fileName, linesToPrint);
            System.Diagnostics.Process.Start("notepad.exe", fileName);
            Application.Exit();
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
