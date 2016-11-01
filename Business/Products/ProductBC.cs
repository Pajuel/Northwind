using Business.Entities;
using Data.DataAccessComponents;
using Data.DataUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace Business.Products
{
    //Esta es una clase estatica y este tipo de clases solo permiten atributos y metodos estaticos
    public static class ProductBC
    {
        //Este metodo es estatico y sirve para obtener un objeto de tipo ProductBE donde sus atributos
        //contienen los valores que se encuentran en la base de datos correspondientes al ProductID que
        //se pasa por parametro al metodo
        public static ProductBE GetProduct(int productID)
        {
            //esta es una de las formas de usar el metodo GetDbCommand, en esta forma se pasa por parametro 
            //la consulta y en el metodo se le asigna la consulta convertida a la propiedad del DbCommand, 
            //CommandText
            var dbCommand = Provider.GetDbCommand("SELECT * FROM Products WHERE ProductID = ?");
            //De esta forma se usa el metodo SetValue, se pasa por parametro el DbCommand y el valor para 
            //agregar un valor de parametro al DbCommand
            Data.DataUtilities.Utilities.SetValue(dbCommand, productID);
            //En el retorno del metodo se usa el metodo Get de esta misma clase el cual sirve para asignar 
            //todos los valores en sus respectivos atributos en la clase ProductBE
            return Get(DAC.EjecutarConsulta(dbCommand).FirstOrDefault());
        }
        //Este metodo sirve para traer varios productos de la base de datos usando un Dctionary de string y object,
        //el Dictionary es una colección de KeyParValue el cual consta de Key y de Value donde el Key es del tipo que
        //se ubica primero en la declaración y el Value el que le sigue (<string, object>). en este caso el Key sera
        //la expresión que complemente el string query para armar la consulta y el Value sera el valor de el parametro 
        //que se use en el Key, ejemplo: Key = "Discontinued = ?" y Value = 0
        public static List<ProductBE> GetProducts(Dictionary<string, object> parameters)
        {
            var query = "SELECT * FROM Products ";
            //Esta es otra forma de usar este metodo, se invoca sin parametros para mas adelante asignar la consulta
            var dbCommand = Provider.GetDbCommand();
            //Si el codigo que se usará dentro de una validación if solo consta de una linea se pueden omitir los corchetes
            if (parameters != null && parameters.Count > 0)
                query += "WHERE ";
            //Aca se recorren los parametros para construit la consulta
            foreach (var param in parameters)
            {
                query += param.Key + " ";
                Data.DataUtilities.Utilities.SetValue(dbCommand, param.Value);
            }
            //Ahora se asigna la consulta convertida (Para su correcto uso en la base de datos), a la propiedad CommandText
            dbCommand.CommandText = Provider.ConvertQuery(query);
            //Aca se crea la lista de ProductBE
            var lstProducts = new List<ProductBE>();
            //En este ciclo se recorre la lista de los DbDataRecord resultantes de la consulta para obtener el ProductBE y 
            //agregarlo a la lista
            foreach (var row in DAC.EjecutarConsulta(dbCommand))
            {
                lstProducts.Add(Get(row));
            }

            return lstProducts;
        }
        //Este metodo es de tipo internal que perimte que solo se pueda acceder a el desde el mismo ensamblado, resive 
        //un DbDataRecord y crea un objeto ProductBE para asignarle los valores en el DbDataRecord
        internal static ProductBE Get(DbDataRecord row)
        {
            var product = new ProductBE();
            //Para evitar errores de conversión se usan los metodos de DataUtilities para convertir los valores del
            //DbDataRecord y asignarselos a los atributos del ProductBE
            product.ProductID = DataUtilities.GetInt32(row["ProductID"]).Value;
            product.ProductName = DataUtilities.GetString(row["ProductName"]);
            product.SupplierID = DataUtilities.GetInt32(row["SupplierID"]);
            product.CategoryID = DataUtilities.GetInt32(row["CategoryID"]);
            product.QuantityPerUnit = DataUtilities.GetString(row["QuantityPerUnit"]);
            product.UnitPrice = DataUtilities.GetDecimal(row["UnitPrice"]);
            product.UnitsInStock = DataUtilities.GetInt32(row["UnitsInStock"]);
            product.UnitsOnOrder = DataUtilities.GetInt32(row["UnitsOnOrder"]);
            product.ReorderLevel = DataUtilities.GetInt32(row["ReorderLevel"]);
            product.Discontinued = DataUtilities.GetBoolean(row["Discontinued"]);

            //Ahora se buscaran los objetos relacionados con este
            var dbCommand = Provider.GetDbCommand("SELECT * FROM Categories WHERE CategoryID = ?");
            Data.DataUtilities.Utilities.SetValue(dbCommand, product.CategoryID);

            product.Category = CategoryBC.Get(DAC.EjecutarConsulta(dbCommand).FirstOrDefault());

            dbCommand = Provider.GetDbCommand("SELECT * FROM Suppliers WHERE SupplierID = ?");
            Data.DataUtilities.Utilities.SetValue(dbCommand, product.SupplierID);

            product.Supplier = Contacts.SupplierBC.Get(DAC.EjecutarConsulta(dbCommand).FirstOrDefault());

            return product;
        }
    }
}
