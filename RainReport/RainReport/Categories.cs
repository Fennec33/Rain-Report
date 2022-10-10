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
            "Repairs"
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
            "Repairs"
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
    }
}
