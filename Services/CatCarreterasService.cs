using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GuanajuatoAdminUsuarios.Services
{
    public class CatCarreterasService : ICatCarreterasService
    {
            private readonly ISqlClientConnectionBD _sqlClientConnectionBD;
            public CatCarreterasService(ISqlClientConnectionBD sqlClientConnectionBD)
            {
                _sqlClientConnectionBD = sqlClientConnectionBD;
            }

            public List<CatCarreterasModel> ObtenerCarreteras()
            {
                //
                List<CatCarreterasModel> ListaCarreteras = new List<CatCarreterasModel>();

                using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
                    try

                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("SELECT c.*, e.estatus FROM catCarreteras AS c INNER JOIN estatus AS e ON c.estatus = e.estatus;", connection);
                        command.CommandType = CommandType.Text;
                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (reader.Read())
                            {
                            CatCarreterasModel carretera = new CatCarreterasModel();
                            carretera.IdCarretera = Convert.ToInt32(reader["IdCarretera"].ToString());
                            carretera.IdDelegacion = Convert.ToInt32(reader["IdCarretera"].ToString());
                            carretera.Carretera = reader["Carretera"].ToString();
                            carretera.estatusDesc = reader["estatus"].ToString();
                            carretera.FechaActualizacion = Convert.ToDateTime(reader["FechaActualizacion"].ToString());
                            carretera.Estatus = Convert.ToInt32(reader["estatus"].ToString());
                            carretera.ActualizadoPor = Convert.ToInt32(reader["ActualizadoPor"].ToString());
                            ListaCarreteras.Add(carretera);

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
                return ListaCarreteras;


            }


        }

    }

