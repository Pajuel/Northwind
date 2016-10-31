using System.Data.Common;
using System.Text;
using System.Data.SqlClient;

namespace Data.DataAccessComponents
{
    //En esta clase se trabaja el provedor de herramientas para la conexión y uso de la base de datos en la aplicación
    public class Provider
    {
        //Este metodo retorna un DbCommand de tipo SqlCommand el cual es una subclase del DbCommand 
        //dirigida a bases de datos Sql Server
        public static DbCommand GetDbCommand()
        {
            return new SqlCommand();
        }
        //Este metodo realiza la misma acción que el anterior con el extra de que resive un string 
        //y crea el SqlCommand con la asignación de la sentencia SQL
        public static DbCommand GetDbCommand(string strCommand)
        {
            return new SqlCommand(ConvertQuery(strCommand));
        }
        //Este metodo retorna un DbConnection de tipo SqlConnection el cual es una subclase del DbConnection 
        //dirigida a bases de datos Sql Server
        public static DbConnection GetDbConnection()
        {
            return new SqlConnection();
        }
        //Este metodo realiza la misma acción que el anterior con el extra de que resive un string 
        //y crea el SqlConnection con la asignación de la cadena de conexión
        public static DbConnection GetDbConnection(string strConnection)
        {
            return new SqlConnection(strConnection);
        }
        //Este metodo resive un string y lo analiza para tensformarlo en nuna sentencia adecuada para los DbCommand,
        //remplasa los ? por @Parameter y un número contador para diferenciarlos entr sí y estos sirven para tener 
        //parametros en la consulta  ejemplo: "SELECT * FROM Products WHERE ProductID = ?" el ? sera remplasado por 
        //la expresión de parametro y este sera usado en la base de datos con el valor de parametro que se asigne en 
        //el DbCommand por medio de la utilidad SetValue de la clase Utilities en este mismo proyecto
        public static string ConvertQuery(string strQuery)
        {
            if (!strQuery.Contains("?")) return strQuery;
            StringBuilder strResult = new StringBuilder();

            int i = 1;
            foreach (var item in strQuery.ToCharArray())
            {
                if (item.Equals('?'))
                {
                    strResult.Append("@Parameter" + i++);
                }
                else
                    strResult.Append(item);
            }

            return strResult.ToString();
        }
    }
}
