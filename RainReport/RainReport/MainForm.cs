namespace RainReport;

using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System.IO;
using System.Globalization;
using System.Linq;

public partial class MainForm : Form
{
    private EndOfDayData _endOfDayReport = new();
    private TransactionDetailsData _transactionDetailsReport = new();
    private SalesData _salesData = new();

    private List<string> _endOfDayReportHeaders = new List<string>();
    private List<string> _transactionDetailsHeaders = new List<string>();

    private bool _haveEndOfDayReport = false;
    private bool _haveTransactionDetailsReport = false;

    public MainForm()
    {
        InitializeComponent();
        SetupHeaderLists();

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
        {
            listBox1.Items.Add(s[i]);
            AddReport(s[i]);
        }
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

        if (headers.SequenceEqual(_endOfDayReportHeaders))
        {
            _endOfDayReport.ReadFile(filePath);
            _haveEndOfDayReport = true;
        }
        else if (headers.SequenceEqual(_transactionDetailsHeaders))
        {
            _transactionDetailsReport.ReadFile(filePath);
            _haveTransactionDetailsReport = true;
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
        if (_haveEndOfDayReport && _haveTransactionDetailsReport)
        {
            _salesData.AsembleDataFrom(_endOfDayReport, _transactionDetailsReport);
            DailySalesReportBuilder report = new();
            report.BuildReport(_salesData);
        }
    }

    private void label1_Click(object sender, EventArgs e)
    {
        //Do Nothing
    }

    private void label2_Click(object sender, EventArgs e)
    {
        //Do Nothing
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