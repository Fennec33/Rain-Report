namespace RainReport;

using CsvHelper;
using System.IO;
using System.Globalization;
using System.Linq;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using RainReport.DataImport;

public partial class MainForm : Form
{
    private List<string> _endOfDayReportHeaders;
    private List<string> _transactionDetailsHeaders;

    public MainForm()
    {
        InitializeComponent();
        SetupHeaderLists();

        this.listBox1.DragDrop += new DragEventHandler(this.ListBox1_DragDrop);
        this.listBox1.DragEnter += new DragEventHandler(this.ListBox1_DragEnter);
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

        headers3.Add("asdf");

        using (var streamReader = new StreamReader(s[0]))
        {
            using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
            {
                csvReader.Read();
                csvReader.ReadHeader();

                headers2 = csvReader.HeaderRecord.ToList();
            }
        }

        if (headers2.Equals(headers3))
        {
            EndOfDayReport report = new EndOfDayReport();
            report.ReadFile(s[0]);
        }
    }

    private bool IsAnEndOfDayReport(string filePath)
    {
        using (var streamReader = new StreamReader(s[0]))
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