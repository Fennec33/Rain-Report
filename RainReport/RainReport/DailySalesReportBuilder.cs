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

            foreach (var transaction in _data.transactions)
                transaction.DistriputeTradeInCredit();

            _exporter.QueLines(BuildDateSupReport());
            _exporter.QueBreak();
            _exporter.QueLines(BuildDepartmentTotalsSubReport());
            _exporter.QueBreak();
            _exporter.QueLines(BuildSalesRepTotalsSubReport());
            _exporter.QueBreak();
            _exporter.QueLines(BuildTotalSalesSubReport());
            _exporter.QueHorizon();
            _exporter.QueLines(BuildTransactionsSubReport());
            _exporter.QueHorizon();
            _exporter.QueLines(BuildNonCommissionableSubReport());
            _exporter.QueBreak();
            _exporter.QueLines(BuildLayAwaySubReport());

            _exporter.ExportReport();
        }

        private string[] BuildDateSupReport()
        {
            return new string[] { DateTime.Now.ToString("M / d / yyyy") };
        }

        private string[] BuildDepartmentTotalsSubReport()
        {
            int[] tableWidths = { 25, 30 };
            TableFormatter formatter = new TableFormatter(tableWidths);

            IDictionary<string, int> departmentTotals = new Dictionary<string, int>();
            int partialPaymentTotal = 0;

            int returnTotal = 0;

            //Go through each transaction item and create a running total of every departments sales numbers
            foreach(Transaction transaction in _data.transactions)
            {
                int partialPaymentAmount = transaction.PartialPaymentAmount();
                if (partialPaymentAmount > 0)
                    partialPaymentTotal += partialPaymentAmount;
                else
                    returnTotal += partialPaymentAmount;

                foreach(TransactionItem item in transaction.items)
                {
                    string key = item.Department;
                    int value = item.GetSalesTotal();

                    if (key == "Service") key = "Repairs"; // count service and repairs together

                    if (value > 0)
                    {
                        if (departmentTotals.ContainsKey(key))
                            departmentTotals[key] += value;
                        else
                            departmentTotals.Add(key, value);
                    }
                    else
                    {
                        returnTotal += value;
                    }
                }
            }

            departmentTotals.Add("Partial Payments", partialPaymentTotal);
            departmentTotals.Add("Returns", returnTotal);


            string[] result = new string[departmentTotals.Count + 1];
            result[0] = "Department Totals";

            for (int i =1; i < result.Length; i++)
            {
                KeyValuePair<string, int> pair = departmentTotals.ElementAt(i-1);
                string[] line = { pair.Key, Categories.ToDollars(pair.Value) };
                result[i] = formatter.Format(line);
            }

            return result;
        }

        private string[] BuildSalesRepTotalsSubReport()
        {
            int[] tableWidths = { 25, 12, 12, 12, 10, 10 };
            TableFormatter formatter = new TableFormatter(tableWidths);

            List<string> salesReps = new List<string>();
            List<int> majorSales = new List<int>();
            List<int> majorItems = new List<int>();
            List<int> majorItemsTransactions = new List<int>();
            List<int> accessorySales = new List<int>();
            List<int> accessoryTransactions = new List<int>();
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
            string[] headers = { "Sales Rep", "Major Sales", "Major Items", "Major Trans",
                "Acc Sales", "Acc Trans" };
            result[0] = formatter.Format(headers);

            for (int i = 0; i < result.Length - 1; i++)
            {
                string[] str = { salesReps[i], Categories.ToDollars(majorSales[i]), majorItems[i].ToString(),
                    majorItemsTransactions[i].ToString(), Categories.ToDollars(accessorySales[i]), accessoryTransactions[i].ToString() };
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

            int commissionSales = 0;
            int nonCommissionSales = 0;

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

            total[1] = Categories.ToDollars(commissionSales + nonCommissionSales);
            cTotal[1] = Categories.ToDollars(commissionSales);
            nCTotal[1] = Categories.ToDollars(nonCommissionSales);

            result[0] = formatter.Format(total);
            result[1] = formatter.Format(cTotal);
            result[2] = formatter.Format(nCTotal);

            return result;
        }

        private string[] BuildTransactionsSubReport()
        {
            int[] tableWidths = { 25, 32, 5, 13, 10 };
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
                    if (!item.IsCommisonable()) continue;

                    salesClerk.Add(item.SalesPerson);
                    itemName.Add(item.ItemName);
                    qty.Add(item.Qty.ToString());
                    salesTotal.Add(Categories.ToDollars(item.GetSalesTotal()));
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

        private string[] BuildNonCommissionableSubReport()
        {
            _exporter.QueLine("Non-Commissionable Items");

            int[] tableWidths = { 25, 32, 5, 13, 10 };
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
                    if (item.IsCommisonable()) continue;

                    salesClerk.Add(item.SalesPerson);
                    itemName.Add(item.ItemName);
                    qty.Add(item.Qty.ToString());
                    salesTotal.Add(Categories.ToDollars(item.GetSalesTotal()));
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

        private string[] BuildLayAwaySubReport()
        {
            int[] tableWidths = { 85 };
            TableFormatter formatter = new TableFormatter(tableWidths);
            List<string> result = new List<string>();

            result.Add("Partial Payment Transactions");

            foreach (Transaction transaction in _data.transactions)
            {
                if (transaction.ContainsPartialPayments())
                {
                    result.Add(transaction.TransactionID.ToString());
                }
            }

            return result.ToArray();
        }
    }
}
