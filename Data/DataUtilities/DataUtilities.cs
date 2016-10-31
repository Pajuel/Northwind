using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataUtilities
{
    //En esta clase se encuentran los metodos que sirven para obtener la información
    //que viene de un datos de la base de datos segun el tipo que se necesite.
    public static class DataUtilities
    {
        //A partir de C# 6.0 se tiene la posibilidad de convertir en nulleable cualquier
        //tipo pormedio de el operador ?, ejemplo : int? a;  a.HasValue es bool y a.Value 
        //devuelve el int si es diferente de null. Este metodo hace uso de esto y permite 
        //gracias a la declaracion del tipo dinamico de clase T en el metodo usarlo para 
        //obtener un resultado nulleable de la clase saignada y convertido del string que 
        //resive como parametro ejemplo GetValueOrNull<int>("5") devielve un int? donde 
        //HasValue = true y Value = 5
        public static T? GetValueOrNull<T>(this string valueAsString)
        where T : struct
        {
            if (string.IsNullOrEmpty(valueAsString))
                return null;
            return (T)Convert.ChangeType(valueAsString, typeof(T));
        }
        //Valida el string que resive como parametro y devuelve un string con el valor acorde
        //a la validación
        public static string GetString(object value)
        {
            if (value == null || value == DBNull.Value) return null;
            else
                return value.ToString();

        }
        //Valida el string que resive como parametro y devuelve un int? con el valor acorde
        //a la validación
        public static int? GetInt32(object value)
        {
            if (value == null || value == DBNull.Value) return null;
            else
                return Convert.ToInt32(value);

        }
        //Valida el string que resive como parametro y devuelve un decimal? con el valor acorde
        //a la validación
        public static decimal? GetDecimal(object value)
        {
            if (value == null || value == DBNull.Value) return null;
            else
                return Convert.ToDecimal(value);

        }
        //Valida el string que resive como parametro y devuelve un long? con el valor acorde
        //a la validación
        public static long? GetInt64(object value)
        {
            if (value == null || value == DBNull.Value) return null;
            else
                return Convert.ToInt64(value);

        }
        //Valida el string que resive como parametro y devuelve un bool con el valor acorde
        //a la validación
        public static bool GetBoolean(object strValue)
        {
            if (strValue == null) return false;
            if (strValue.ToString().Equals("1")) return true;
            if (strValue.ToString().Equals("S")) return true;
            if (strValue.ToString().ToUpper().Equals("SI")) return true;
            if (strValue.GetType() == typeof(bool))
                return Convert.ToBoolean(strValue);
            return false;
        }
        //Valida si el object es de un tipo numerico y devuelve true o false
        public static bool IsNumeric(object Expression)
        {
            if (Expression == null || Expression is DateTime)
                return false;

            if (Expression is short || Expression is int || Expression is long || Expression is decimal || Expression is float || Expression is double)
                return true;

            //Esto es una instrucción que sirve para controlar errores durante 
            //la ejecución del codigo dentro de la instrucción, cuando ocurre 
            //un error se ejecuta el codigo que se encuentre dentro del catch
            //en este caso sirve para intentar convertir el object en doble, 
            //si ocurre un error salta hasta el retorno del false
            try
            {
                if (Expression is string)
                    double.Parse(Expression as string);
                else
                    double.Parse(Expression.ToString());
                return true;
            }
            catch { }
            return false;
        }
    }
}
