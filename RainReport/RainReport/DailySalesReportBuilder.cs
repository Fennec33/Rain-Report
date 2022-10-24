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
            _exporter.QueHorizonThick();
            _exporter.QueLines(BuildTransactionsSubReport());
            _exporter.QueHorizonThick();
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
            int[] tableWidths = { 25, 12 };
            TableFormatter formatter = new TableFormatter(tableWidths);
            formatter.RightAlignCol(1);

            IDictionary<string, int> departmentTotals = new Dictionary<string, int>();
            int partialPaymentTotal = 0;

            departmentTotals.Add("Accessories", 0);
            departmentTotals.Add("Print Music", 0);
            departmentTotals.Add("Repairs", 0);
            departmentTotals.Add("Band Instruments", 0);
            departmentTotals.Add("Orchestra Instruments", 0);
            departmentTotals.Add("Acoustic Guitars", 0);
            departmentTotals.Add("Electric Guitars", 0);
            departmentTotals.Add("Amplifiers", 0);
            departmentTotals.Add("Live Sound & Recording", 0);
            departmentTotals.Add("Keyboards", 0);
            departmentTotals.Add("Drums", 0);
            departmentTotals.Add("Rental Payment", 0);
            departmentTotals.Add("Studio Rent", 0);
            departmentTotals.Add("Shipping", 0);
            departmentTotals.Add("Promotions", 0);
            departmentTotals.Add("Food Bank", 0);

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

            int departmentSum = 0;

            foreach (var kvp in departmentTotals)
            {
                departmentSum += kvp.Value;
            }

            departmentTotals.Add("Partial Payments", partialPaymentTotal);
            departmentTotals.Add("Returns", returnTotal);
            departmentTotals.Add("Total", departmentSum);


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
            int[] tableWidths = { 24, 16, 9, 9, 16, 9 };
            TableFormatter formatter = new TableFormatter(tableWidths);
            formatter.RightAlignCol(1);
            formatter.RightAlignCol(2);
            formatter.RightAlignCol(3);
            formatter.RightAlignCol(4);
            formatter.RightAlignCol(5);

            List<string> salesReps = new List<string>();
            List<int> majorSales = new List<int>();
            List<int> majorItems = new List<int>();
            List<int> majorItemsTransactions = new List<int>();
            List<int> accessorySales = new List<int>();
            List<int> accessoryTransactions = new List<int>();
            bool isMajorItemTransaction = false;
            int index;

            foreach (Transaction transaction in _data.transactions)
            {
                string salesRep = transaction.Employee;
                if (!salesReps.Contains(salesRep))
                {
                    salesReps.Add(salesRep);
                    majorSales.Add(0);
                    majorItems.Add(0);
                    majorItemsTransactions.Add(0);
                    accessorySales.Add(0);
                    accessoryTransactions.Add(0);
                }

                salesReps.Sort();
            }

            foreach (Transaction transaction in _data.transactions)
            {
                if (!transaction.IsCommisionableTransaction())
                    continue;

                index = salesReps.IndexOf(transaction.Employee);

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
            string[] headers = { " Sales Rep", " Major Sales", " M Items", " M Trans",
                " Acc Sales", " A Trans" };
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
            formatter.RightAlignCol(1);

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
            int[] tableWidths = { 23, 31, 5, 13, 10, 2 };
            TableFormatter formatter = new TableFormatter(tableWidths);
            formatter.RightAlignCol(2);
            formatter.RightAlignCol(3);
            formatter.RightAlignCol(4);

            List<string> salesClerk = new List<string>();
            List<string> itemName = new List<string>();
            List<string> qty = new List<string>();
            List<string> salesTotal = new List<string>();
            List<string> ID = new List<string>();
            List<string> PPFlag = new List<string>();

            salesClerk.Add("Sales Clerk");
            itemName.Add("Item Name");
            qty.Add("Qty");
            salesTotal.Add("Sales Total");
            ID.Add("ID      ");
            PPFlag.Add("");

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
                    PPFlag.Add(transaction.ContainsPartialPayments() ? "*" : "");
                }
            }

            int distinct = ID.Distinct().Count();
            string[] result = new string[salesClerk.Count + distinct - 1];

            int count = 0;
            for (int i = 0; i < salesClerk.Count; i++)
            {
                if (i > 0 && ID[i] != ID[i - 1])
                {
                    result[count] = "------------------------------------------------------------------------------------";
                    count++;
                }
                
                string[] str = { salesClerk[i], itemName[i], qty[i], salesTotal[i], ID[i], PPFlag[i] };

                if (i > 0)
                {
                    if (ID[i] == ID[i - 1])
                    {
                        str[0] = "";
                        str[4] = "";
                        str[5] = "";
                    }
                }

                result[count] = formatter.Format(str);
                count++;
            }
            
            return result;
        }

        private string[] BuildNonCommissionableSubReport()
        {
            _exporter.QueLine("Non-Commissionable Items");

            int[] tableWidths = { 23, 31, 5, 13, 10, 2 };
            TableFormatter formatter = new TableFormatter(tableWidths);
            formatter.RightAlignCol(2);
            formatter.RightAlignCol(3);
            formatter.RightAlignCol(4);

            List<string> salesClerk = new List<string>();
            List<string> itemName = new List<string>();
            List<string> qty = new List<string>();
            List<string> salesTotal = new List<string>();
            List<string> ID = new List<string>();
            List<string> PPFlag = new List<string>();

            salesClerk.Add("Sales Clerk");
            itemName.Add("Item Name");
            qty.Add("Qty");
            salesTotal.Add("Sales Total");
            ID.Add("ID      ");
            PPFlag.Add("");

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
                    PPFlag.Add(transaction.ContainsPartialPayments() ? "*" : "");
                }
            }

            int distinct = ID.Distinct().Count();
            string[] result = new string[salesClerk.Count + distinct - 1];

            int count = 0;
            for (int i = 0; i < salesClerk.Count; i++)
            {
                if (i > 0 && ID[i] != ID[i - 1])
                {
                    result[count] = "------------------------------------------------------------------------------------";
                    count++;
                }

                string[] str = { salesClerk[i], itemName[i], qty[i], salesTotal[i], ID[i], PPFlag[i] };

                if (i > 0)
                {
                    if (ID[i] == ID[i - 1])
                    {
                        str[0] = "";
                        str[4] = "";
                        str[5] = "";
                    }
                }

                result[count] = formatter.Format(str);
                count++;
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
