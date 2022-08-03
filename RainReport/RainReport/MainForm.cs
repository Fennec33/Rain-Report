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

        EndOfDayReport report = new EndOfDayReport();
        report.ReadFile(s[0]);
    }

    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
    {

    }
}