using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SisVenttas.Datos;

namespace SistemaVentas.Datos
{
   public class FCliente
    {
        public static DataSet GetAll()
        {
            SqlParameter[] dbParams = new SqlParameter[]
                {
                    
                };
            return FDBHelper.ExecuteDataSet("usp_Data_FCliente_GetAll", dbParams);

        }
    }
}
