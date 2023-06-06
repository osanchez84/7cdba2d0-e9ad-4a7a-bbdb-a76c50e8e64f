using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GuanajuatoAdminUsuarios.Services
{
    public class CatTramosService : ICatTramosService
    {
        private readonly ISqlClientConnectionBD _sqlClientConnectionBD;
        public CatTramosService(ISqlClientConnectionBD sqlClientConnectionBD)
        {
            _sqlClientConnectionBD = sqlClientConnectionBD;
        }

        public List<CatTramosModel> ObtenerTamosPorCarretera(int carreteraDDValue)
        {
            //
            List<CatTramosModel> ListaTramos = new List<CatTramosModel>();

            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
                try

                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT t.*, c.*, e.estatus FROM catTramos AS t INNER JOIN catCarreteras AS c ON t.idCarretera = c.idCarretera INNER JOIN estatus AS e ON t.estatus = e.estatus WHERE t.idCarretera = @IdCarretera;\r\n", connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("@IdCarretera", SqlDbType.Int)).Value = (object)carreteraDDValue ?? DBNull.Value;

                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            CatTramosModel tramo = new CatTramosModel();
                            tramo.IdTramo = Convert.ToInt32(reader["IdTramo"].ToString());
                            tramo.IdCarretera = Convert.ToInt32(reader["IdCarretera"].ToString());
                            tramo.Tramo = reader["Tramo"].ToString();
                            tramo.estatusDesc = reader["estatus"].ToString();
                            tramo.FechaActualizacion = Convert.ToDateTime(reader["FechaActualizacion"].ToString());
                            tramo.Estatus = Convert.ToInt32(reader["estatus"].ToString());
                            tramo.ActualizadoPor = Convert.ToInt32(reader["ActualizadoPor"].ToString());
                            ListaTramos.Add(tramo);

                        }

                    }

                }
                catch (SqlException ex)
                {
                    //Guardar la excepcion en algun log de errores
                    //ex
                }
                finally
                {
                    connection.Close();
                }
            return ListaTramos;


        }


    }

}
