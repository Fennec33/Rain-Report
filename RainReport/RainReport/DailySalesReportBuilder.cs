using System;
using System.Collections.Generic;
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

            Export(BuildDepartmentTotalsSubReport());
            Export(BuildSalesRepTotalsSubReport());
            Export(BuildTransactionsSubReport());
            Export(BuildNonCommissionableSunReport());
            Export(BuildLayAwaySubReport());
        }

        private string BuildDepartmentTotalsSubReport()
        {
            return "";
        }

        private string BuildSalesRepTotalsSubReport()
        {
            return "";
        }

        private string BuildTransactionsSubReport()
        {
            return "";
        }

        private string BuildNonCommissionableSunReport()
        {
            return "";
        }

        private string BuildLayAwaySubReport()
        {
            return "";
        }

        private void Export(string str)
        {

        }
    }
}
