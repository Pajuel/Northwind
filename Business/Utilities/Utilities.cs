using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Utilities
{
    public class Utilities
    {
        public static void IniciarVariables()
        {
            Data.DataAccessComponents.DAC.StringConnection = System.Configuration.ConfigurationManager.ConnectionStrings["CadenaConexion"].ConnectionString;
        }
    }
}
