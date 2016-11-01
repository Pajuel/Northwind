using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Entities;
using Data.DataAccessComponents;
using Data.DataUtilities;
using System.Data.Common;

namespace Business.Contacts
{
    public class SupplierBC
    {
        internal static SupplierBE Get(DbDataRecord row)
        {
            var supplier = new SupplierBE();

            supplier.SupplierID = DataUtilities.GetInt32(row["SupplierID"]).Value;
            supplier.CompanyName = DataUtilities.GetString(row["CompanyName"]);
            supplier.ContactName = DataUtilities.GetString(row["ContactName"]);
            supplier.ContactTitle = DataUtilities.GetString(row["ContactTitle"]);
            supplier.Address = DataUtilities.GetString(row["Address"]);
            supplier.City = DataUtilities.GetString(row["City"]);
            supplier.Region = DataUtilities.GetString(row["Region"]);
            supplier.PostalCode = DataUtilities.GetString(row["PostalCode"]);
            supplier.Country = DataUtilities.GetString(row["Country"]);
            supplier.Phone = DataUtilities.GetString(row["Phone"]);
            supplier.Fax = DataUtilities.GetString(row["Fax"]);
            supplier.HomePage = DataUtilities.GetString(row["HomePage"]);

            return supplier;
        }
    }
}
