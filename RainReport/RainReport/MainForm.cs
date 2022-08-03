namespace RainReport;

using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System.IO;
using System.Globalization;
using System.Linq;
using RainReport.DataImport;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();

        this.listBox1.DragDrop += new DragEventHandler(this.ListBox1_DragDrop);
        this.listBox1.DragEnter += new DragEventHandler(this.ListBox1_DragEnter);
    }

    private void ListBox1_DragEnter(object? sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
            e.Effect = DragDropEffects.All;
        else
            e.Effect = DragDropEffects.None;
    }

    private void ListBox1_DragDrop(object? sender, DragEventArgs e)
    {
        string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

        for (int i = 0; i < s.Length; i++)
            listBox1.Items.Add(s[i]);

        List<string> headers2;
        List<string> headers3 = new List<string>();

        using (var streamReader = new StreamReader(s[0]))
        {
            using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
            {
                csvReader.Read();
                csvReader.ReadHeader();

                headers2 = csvReader.HeaderRecord.ToList();
            }
        }

        if (headers2.Equals(_endOfDayReportHeaders))
        {
            EndOfDayReport report = new EndOfDayReport();
            report.ReadFile(s[0]);
        }
    }

    private bool IsAnEndOfDayReport(string filePath)
    {
        using (var streamReader = new StreamReader(filePath))
        {
            using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
            {
                csvReader.Read();
                csvReader.ReadHeader();

                List<string> headers = csvReader.HeaderRecord.ToList();

                if (headers.Equals(_endOfDayReportHeaders))
                    return true;
                else
                    return false;
            }
        }
    }

    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Do Nothing
    }

    private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
    {
        //Do Nothing
    }

    private void button1_Click(object sender, EventArgs e)
    {
        //Do Nothing
    }

    private void label1_Click(object sender, EventArgs e)
    {
        //Do Nothing
    }

    private void label2_Click(object sender, EventArgs e)
    {
        //Do Nothing
    }
}