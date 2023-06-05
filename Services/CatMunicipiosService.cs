using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using System.Collections.Generic;
using System.Data;
using System;
using System.Data.SqlClient;

namespace GuanajuatoAdminUsuarios.Services
{
    public class CatMunicipiosService : ICatMunicipiosService
    {
        private readonly ISqlClientConnectionBD _sqlClientConnectionBD;
        public CatMunicipiosService(ISqlClientConnectionBD sqlClientConnectionBD)
        {
            _sqlClientConnectionBD = sqlClientConnectionBD;
        }

        public List<CatMunicipiosModel> GetMunicipios()
        {
            //
            List<CatMunicipiosModel> ListaMunicipios = new List<CatMunicipiosModel>();

            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
                try

                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT m.*, e.estatus FROM catMunicipios AS m INNER JOIN estatus AS e ON m.estatus = e.estatus;", connection);
                    command.CommandType = CommandType.Text;
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            CatMunicipiosModel municipio = new CatMunicipiosModel();
                            municipio.IdMunicipio = Convert.ToInt32(reader["IdMunicipio"].ToString());
                            municipio.Municipio = reader["Municipio"].ToString();
                            municipio.estatusDesc = reader["estatus"].ToString();
                            municipio.FechaActualizacion = Convert.ToDateTime(reader["FechaActualizacion"].ToString());
                            municipio.Estatus = Convert.ToInt32(reader["estatus"].ToString());
                            municipio.ActualizadoPor = Convert.ToInt32(reader["ActualizadoPor"].ToString());
                            ListaMunicipios.Add(municipio);

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
            return ListaMunicipios;


        }


    }

}
