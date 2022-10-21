using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainReport
{
    public static class Categories
    {
        private static string[] _commisonableDepartments = new string[] {
            "Accessories",
            "Acoustic Guitars",
            "Amplifiers",
            "Band Instruments",
            "Drums",
            "Electric Guitars",
            "Keyboards",
            "Live Sound & Recording",
            "Orchestra Instruments",
            "Print Music",
            "Repairs",
            "Service"
        };
        private static string[] _majorItemDepartments = new string[] {
            "Acoustic Guitars",
            "Amplifiers",
            "Band Instruments",
            "Drums",
            "Electric Guitars",
            "Keyboards",
            "Live Sound & Recording",
            "Orchestra Instruments"
        };
        private static string[] _accessoryItemDepartments = new string[] {
            "Accessories",
            "Print Music",
            "Repairs",
            "Service"
        };

        public static bool IsThisCommisonable(TransactionItem item)
        {
            if (item.SKU == "012N") // if item is a new rental payment
                return true;

            string department = item.Department;

            for (int i = 0; i < _commisonableDepartments.Length; i++)
            {
                if (department == _commisonableDepartments[i])
                    return true;
            }
            return false;
        }

        public static bool IsThisAnAccessory(TransactionItem item)
        {
            if (item.SKU == "012N") // if item is a new rental payment
                return true;

            string department = item.Department;

            for (int i = 0; i < _accessoryItemDepartments.Length; i++)
            {
                if (department == _accessoryItemDepartments[i])
                    return true;
            }
            return false;
        }

        public static bool IsThisAMajorItem(TransactionItem item)
        {
            string department = item.Department;

            for (int i = 0; i < _majorItemDepartments.Length; i++)
            {
                if (department == _majorItemDepartments[i])
                    return true;
            }
            return false;
        }

        public static string ToDollars(int cents)
        {
            string result = $"{cents}";

            if (result.Length == 2)
                return $"0.{cents}";
            if (result.Length == 1)
                return $"0.0{cents}";

            result = result.Insert(result.Length - 2, ".");
            return result;
        }

        public static int ToCents(string dollars)
        {
            string d = "0";
            string c = "00";
            int negitive = 1;

            if (dollars[0] == '-')
            {
                negitive = -1;
                dollars = dollars.Remove(0, 1);
            }

            int decimalIndex = dollars.IndexOf('.');

            if (decimalIndex != -1)
            {
                dollars += "0";
                d = dollars.Substring(0, decimalIndex);
                c = dollars.Substring(decimalIndex + 1, 2);
            }
            else
            {
                d = dollars;
                c = "00";
            }

            return int.Parse(d + c) * negitive;
        }
    }
}
