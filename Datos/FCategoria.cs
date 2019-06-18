using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SisVenttas.Datos;
using SistemaVentas.Entidades;

namespace SistemaVentas.Datos
{
   public class FCategoria
    {
        public static System.Data.DataSet GetAll()
        {
            SqlParameter[] dbParams = new SqlParameter[]
                {

                };
            return FDBHelper.ExecuteDataSet("usp_Data_FCategoria_GetAll", dbParams);

        }
        public static int Insertar(Categoria categoria)
        {
            SqlParameter[] dbParams = new SqlParameter[]
                {
                    FDBHelper.MakeParam("@Descripcion", SqlDbType.VarChar, 0, categoria.Descripcion),
                };
            return Convert.ToInt32(FDBHelper.ExecuteScalar("usp_Data_FCategoria_Insertar", dbParams));

        }

        public static int Actualizar(Categoria categoria)
        {
            SqlParameter[] dbParams = new SqlParameter[]
                {
                    FDBHelper.MakeParam("@Id", SqlDbType.Int, 0, categoria.Id),
                    FDBHelper.MakeParam("@Descripcion", SqlDbType.VarChar, 0, categoria.Descripcion),
                };
            return Convert.ToInt32(FDBHelper.ExecuteScalar("usp_Data_FCategoria_Actualizar", dbParams));

        }

        public static int Eliminar(Categoria categoria)
        {
            SqlParameter[] dbParams = new SqlParameter[]
                {
                    FDBHelper.MakeParam("@Id", SqlDbType.Int, 0, categoria.Id),

                };
            return Convert.ToInt32(FDBHelper.ExecuteScalar("usp_Data_FCategoria_Eliminar", dbParams));

        }
    }
}
