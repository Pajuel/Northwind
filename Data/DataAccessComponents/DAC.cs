using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;


namespace Data.DataAccessComponents
{
    //En esta clase se encuentran los componentes de acceso a datos (Data Access Components - DAC) los cuales
    //se usan para realisar consultas o acciones de alteración en los datos de la base de datos
    public class DAC
    {
        //Esta variable es para la cadena de conexión
        public static string StringConnection { get; set; }
        //Este metodo sirve para realizar una consulta en la base de datos y resive como parametros un DbCommand 
        //con el cual se realiza la consulta, y los siguientes parametros son opcionales lo que significa que si al invocar el 
        //metodo no usan esos parametros estos toman el valor con el que se estan igualando en la declaración del metodo
        //ejemplo DbConnection = null, si no se pasa por parametro un valor este toma el valor null.
        //Este metodo devuelve una lista de DbDataRecord, estos contienen un array de valores de una fila del resultado
        //de la consulta llevando como identificador de cada espacio del array el nombre de la columna del SELECT de la consulta
        public static List<DbDataRecord> EjecutarConsulta(DbCommand dbCommand, DbConnection connection = null, int intMax = -1)
        {
            var lstDbDataRecord = new List<DbDataRecord>();
            using (var dbConnection = connection == null ? Provider.GetDbConnection(StringConnection) : connection)
            {
                dbCommand.Connection = dbConnection;
                if (dbCommand.Connection.State == ConnectionState.Closed) dbCommand.Connection.Open();
                using (var dbDataReader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    DbEnumerator dbEnumerator = (DbEnumerator)dbDataReader.GetEnumerator();
                    var intCount = 0;
                    while ((intMax == -1 || intCount < intMax) && dbEnumerator.MoveNext())
                    {
                        lstDbDataRecord.Add((DbDataRecord)dbEnumerator.Current);
                        intCount++;
                    }
                }
                dbCommand.Dispose();
            }
            return lstDbDataRecord;
        }
        //Este metodo sirve para ejecutar una sentencia de acción en la base de datos como Adicionar, actualizar o borrar información 
        //en una tabla, resive como parametros un DbCommand, y una DbConnection opcional, devuelve un valor entero que representa el
        //número de filas afectadas por la acción.
        public static int EjecutarSentenciaUnitaria(DbCommand dbCommand, DbConnection connection = null)
        {
            using (var dbConnection = connection == null ? Provider.GetDbConnection(StringConnection) : connection)
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                using (var dbTransaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        dbCommand.Connection = dbConnection;
                        dbCommand.Transaction = dbTransaction;
                        var intResult = dbCommand.ExecuteNonQuery();
                        dbCommand.Dispose();
                        dbTransaction.Commit();
                        return intResult;
                    }
                    catch (Exception)
                    {
                        dbTransaction.Rollback();
                        throw;
                    }

                }
            }
        }
        //Este metodo funciona similar al anterio solo que este resive una lista de DbCommands para realizar mulitples sentencias
        // y devuelve un lista de enteros donde cada uno es el número de filas afectadas por cada sentencia en el orden en el que
        //venian los DbCommand
        public static List<int> EjecutarSentenciaMultiple(List<DbCommand> lstDbCommand, DbConnection connection = null)
        {
            var lstResult = new List<int>();
            using (var dbConnection = connection == null ? Provider.GetDbConnection(StringConnection) : connection)
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                using (var dbTransaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in lstDbCommand)
                        {
                            item.Connection = dbConnection;
                            item.Transaction = dbTransaction;
                            lstResult.Add(item.ExecuteNonQuery());
                            item.Dispose();
                        }
                        dbTransaction.Commit();
                        return lstResult;
                    }
                    catch
                    {
                        dbTransaction.Rollback();
                        throw;
                    }

                }
            }

        }
        public static object EjecutarEscalar(DbCommand dbCommand, DbConnection connection = null)
        {
            using (var dbConnection = connection == null ? Provider.GetDbConnection(StringConnection) : connection)
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                using (var dbTransaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        dbCommand.Connection = dbConnection;
                        dbCommand.Transaction = dbTransaction;
                        var objResult = dbCommand.ExecuteScalar();
                        dbCommand.Dispose();
                        return objResult;
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }
            }
        }
    }
}
