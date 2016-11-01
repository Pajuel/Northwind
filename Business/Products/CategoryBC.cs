using Business.Entities;
using Data.DataAccessComponents;
using Data.DataUtilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Products
{
    public class CategoryBC
    {
        internal static CategoryBE Get(DbDataRecord row)
        {
            var category = new CategoryBE();
            
            category.CategoryID = DataUtilities.GetInt32(row["CategoryID"]).Value;
            category.CategoryName = DataUtilities.GetString(row["CategoryName"]);
            category.Description = DataUtilities.GetString(row["Description"]);

            var dbCommand = Provider.GetDbCommand("SELECT Picture FROM Categories WHERE CategoryID = ?");
            Data.DataUtilities.Utilities.SetValue(dbCommand, category.CategoryID);

            category.Picture = DAC.EjecutarEscalar(dbCommand) as byte[];

            return category;
        }
    }
}
