using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataUtilities
{
    public class Utilities
    {
        //Este metodo agrega o actualiza un valor en la lista de parametros de un DbCommand el 
        //cual resive como parametro, tambien resive el número de la posición en la lista por 
        //si se desea actualizar un valor, si resive -1 lo agrega, el valor es el que se encuentra
        //en el parametro object
        public static void SetValue(DbCommand command, int parameterIndex, object parameter)
        {
            if (parameterIndex == -1)
            {
                command.Parameters.Add(command.CreateParameter());
                parameterIndex = command.Parameters.Count;
            }
            else
            {
                if (command.Parameters.Count < parameterIndex)
                    command.Parameters.Add(command.CreateParameter());
            }
            if (parameter == null)
                command.Parameters[parameterIndex - 1].Value = DBNull.Value;
            else if (typeof(string) == parameter.GetType())
                command.Parameters[parameterIndex - 1].Value = ((string)parameter).Trim();
            else
                command.Parameters[parameterIndex - 1].Value = parameter;

        }
        //Este metodo usa el anterior y solo sirve para agregar un valor en la lista de parametros
        //de el DbCommand
        public static void SetValue(DbCommand command, object parameter)
        {
            SetValue(command, -1, parameter);
        }        
    }
}
