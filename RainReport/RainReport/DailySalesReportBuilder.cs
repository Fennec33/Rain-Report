using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainReport
{
    public class DailySalesReportBuilder
    {
        private SalesData _data;
        private ReportExporter _exporter = new();

        public void BuildReport(SalesData data)
        {
            _data = data;

            _exporter.QueLines(BuildDepartmentTotalsSubReport());
            _exporter.QueBreak();
            _exporter.QueLines(BuildSalesRepTotalsSubReport());
            _exporter.QueBreak();
            _exporter.QueLines(BuildTotalSalesSubReport());
            _exporter.QueBreak();
            _exporter.QueLines(BuildTransactionsSubReport());
            _exporter.QueBreak();
            //_exporter.QueLines(BuildNonCommissionableSunReport());
            _exporter.QueBreak();
            //_exporter.QueLines(BuildLayAwaySubReport());

            _exporter.ExportReport();
        }

        private string[] BuildDepartmentTotalsSubReport()
        {
            int[] tableWidths = { 50, 30 };
            TableFormatter formatter = new TableFormatter(tableWidths);

            IDictionary<string, float> departmentTotals = new Dictionary<string, float>();

            //Go through each transaction item and create a running total of every departments sales numbers
            foreach(Transaction transaction in _data.transactions)
            {
                foreach(TransactionItem item in transaction.items)
                {
                    string key = item.Department;
                    float value = item.GetSalesTotal();

                    if(departmentTotals.ContainsKey(key))
                        departmentTotals[key] += value;
                    else
                        departmentTotals.Add(key, value);
                }
            }

            string[] result = new string[departmentTotals.Count + 2];
            result[0] = "Department Totals";
            result[result.Length-1] = "Lay-Away Payments";

            for (int i =1; i < result.Length - 1; i++)
            {
                KeyValuePair<string, float> pair = departmentTotals.ElementAt(i-1);
                string[] line = { pair.Key, pair.Value.ToString() };
                result[i] = formatter.Format(line);
            }

            return result;
        }

        private string[] BuildSalesRepTotalsSubReport()
        {
            int[] tableWidths = { 30, 10, 10, 10, 10, 10 };
            TableFormatter formatter = new TableFormatter(tableWidths);

            List<string> salesReps = new List<string>();
            List<float> majorSales = new List<float>();
            List<float> majorItems = new List<float>();
            List<float> majorItemsTransactions = new List<float>();
            List<float> accessorySales = new List<float>();
            List<float> accessoryTransactions = new List<float>();
            bool isMajorItemTransaction = false;
            string salesRep;
            int index;

            foreach (Transaction transaction in _data.transactions)
            {
                salesRep = transaction.Employee;
                if (!salesReps.Contains(salesRep))
                {
                    salesReps.Add(salesRep);
                    majorSales.Add(0);
                    majorItems.Add(0);
                    majorItemsTransactions.Add(0);
                    accessorySales.Add(0);
                    accessoryTransactions.Add(0);
                }

                index = salesReps.IndexOf(salesRep);

                foreach (TransactionItem item in transaction.items)
                {
                    if(!Categories.IsThisCommisonable(item))
                        continue;

                    if (Categories.IsThisAMajorItem(item))
                    {
                        isMajorItemTransaction = true;
                        majorSales[index] += item.GetSalesTotal();
                        majorItems[index]++;
                    }
                    else
                    {
                        accessorySales[index] += item.GetSalesTotal();
                    }
                }
                
                if (isMajorItemTransaction)
                {
                    isMajorItemTransaction = false;
                    majorItemsTransactions[index] += 1;
                }
                else
                {
                    accessoryTransactions[index]++;
                }
            }

            string[] result = new string[salesReps.Count + 1];
            string[] headers = { "Sales Rep", "Major Sales", "Major Items", "Major Item Transactions",
                "Acc Sales", "Acc Trans" };
            result[0] = formatter.Format(headers);

            for (int i = 0; i < result.Length - 1; i++)
            {
                string[] str = { salesReps[i], majorSales[i].ToString(), majorItems[i].ToString(),
                    majorItemsTransactions[i].ToString(), accessorySales[i].ToString(), accessoryTransactions[i].ToString() };
                result[i + 1] = formatter.Format(str);
            }

            return result;
        }

        private string[] BuildTotalSalesSubReport()
        {
            int[] tableWidths = { 30, 15 };
            TableFormatter formatter = new TableFormatter(tableWidths);

            string[] result = new string[3];
            string[] total = {"Total: ", ""};
            string[] cTotal = { "Commissionable Total: ", "" };
            string[] nCTotal = { "Non-Commissionable Total: ", "" };

            float commissionSales = 0;
            float nonCommissionSales = 0;

            foreach (Transaction transaction in _data.transactions)
            {
                foreach (TransactionItem item in transaction.items)
                {
                    if (Categories.IsThisCommisonable(item))
                        commissionSales += item.GetSalesTotal();
                    else
                        nonCommissionSales += item.GetSalesTotal();
                }
            }

            total[1] = (commissionSales + nonCommissionSales).ToString();
            cTotal[1] = commissionSales.ToString();
            nCTotal[1] = nonCommissionSales.ToString();

            result[0] = formatter.Format(total);
            result[1] = formatter.Format(cTotal);
            result[2] = formatter.Format(nCTotal);

            return result;
        }

        private string[] BuildTransactionsSubReport()
        {
            int[] tableWidths = { 20, 30, 10, 10, 10 };
            TableFormatter formatter = new TableFormatter(tableWidths);
            
            List<string> salesClerk = new List<string>();
            List<string> itemName = new List<string>();
            List<string> qty = new List<string>();
            List<string> salesTotal = new List<string>();
            List<string> ID = new List<string>();

            salesClerk.Add("Sales Clerk");
            itemName.Add("Item Name");
            qty.Add("Qty");
            salesTotal.Add("Sales Total");
            ID.Add("ID");

            foreach (Transaction transaction in _data.transactions)
            {
                foreach (TransactionItem item in transaction.items)
                {
                    salesClerk.Add(item.SalesPerson);
                    itemName.Add(item.ItemName);
                    qty.Add(item.Qty.ToString());
                    salesTotal.Add(item.GetSalesTotal().ToString());
                    ID.Add(item.ID.ToString());
                }
            }

            string[] result = new string[salesClerk.Count];

            for (int i = 0; i < salesClerk.Count; i++)
            {
                string[] str = { salesClerk[i], itemName[i], qty[i], salesTotal[i], ID[i] };

                if (i > 0)
                {
                    if (ID[i] == ID[i - 1])
                    {
                        str[0] = "";
                        str[4] = "";
                    }
                }

                result[i] = formatter.Format(str);
            }
            
            return result;
        }

        private string[] BuildNonCommissionableSunReport()
        {
            int[] tableWidths = { 85 };
            TableFormatter formatter = new TableFormatter(tableWidths);

            return new string[] { "" };
        }

        private string[] BuildLayAwaySubReport()
        {
            int[] tableWidths = { 85 };
            TableFormatter formatter = new TableFormatter(tableWidths);

            return new string[] { "" };
        }
    }
}
