using GuanajuatoAdminUsuarios.Entity;
using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
namespace GuanajuatoAdminUsuarios.Services
{
    public class CatAgenciasMinisterioService : ICatAgenciasMinisterioService
    {
        
            private readonly ISqlClientConnectionBD _sqlClientConnectionBD;
            public CatAgenciasMinisterioService(ISqlClientConnectionBD sqlClientConnectionBD)
            {
                _sqlClientConnectionBD = sqlClientConnectionBD;
            }

            public List<CatAgenciasMinisterioModel> ObtenerAgenciasActivas()
            {
                //
                List<CatAgenciasMinisterioModel> ListaAgencias = new List<CatAgenciasMinisterioModel>();

                using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
                    try

                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(@"SELECT cAm.*, estatus.estatusdesc,ctd.nombreOficina
                                                              FROM catAgenciasMinisterio cAm
                                                             JOIN estatus ON cAm.estatus = estatus.estatus 
															 LEFT JOIN catDelegacionesOficinasTransporte ctd ON ctd.idOficinaTransporte = cAm.idDelegacion
                                                            ORDER BY NombreAgencia;", connection);
                        command.CommandType = CommandType.Text;
                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (reader.Read())
                            {
                                CatAgenciasMinisterioModel agencia = new CatAgenciasMinisterioModel();
                            agencia.IdAgenciaMinisterio = Convert.ToInt32(reader["IdAgenciaMinisterio"].ToString());
                            agencia.IdDelegacion = Convert.ToInt32(reader["IdDelegacion"].ToString());
                            agencia.NombreAgencia = reader["NombreAgencia"].ToString();
							agencia.DelegacionDesc = reader["nombreOficina"].ToString();

							//marcasVehiculo.FechaActualizacion = Convert.ToDateTime(reader["FechaActualizacion"].ToString());
							//marcasVehiculo.ActualizadoPor = Convert.ToInt32(reader["ActualizadoPor"].ToString());
							agencia.Estatus = Convert.ToInt32(reader["Estatus"].ToString());
                            agencia.estatusDesc = reader["estatusDesc"].ToString();
                            ListaAgencias.Add(agencia);
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
                return ListaAgencias;


            }

        public List<CatAgenciasMinisterioModel> ObtenerAgenciasActivasPorDelegacion(int idOficina)
        {
            //
            List<CatAgenciasMinisterioModel> ListaAgencias = new List<CatAgenciasMinisterioModel>();

            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
                try

                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(@"SELECT cAM.*, estatus.estatusdesc 
                                                        FROM catAgenciasMinisterio cAM JOIN estatus ON cAM.estatus = estatus.estatus
                                                        WHERE cAM.estatus = 1 AND cAM.idDelegacion = @idOficina    
                                                        ORDER BY cAM.NombreAgencia ASC;", connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("@idOficina", SqlDbType.Int)).Value = (object)idOficina ?? DBNull.Value;

                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            CatAgenciasMinisterioModel agencia = new CatAgenciasMinisterioModel();
                            agencia.IdAgenciaMinisterio = Convert.ToInt32(reader["IdAgenciaMinisterio"].ToString());
                            agencia.IdDelegacion = Convert.ToInt32(reader["IdDelegacion"].ToString());
                            agencia.NombreAgencia = reader["NombreAgencia"].ToString();
                            //marcasVehiculo.FechaActualizacion = Convert.ToDateTime(reader["FechaActualizacion"].ToString());
                            //marcasVehiculo.ActualizadoPor = Convert.ToInt32(reader["ActualizadoPor"].ToString());
                            agencia.Estatus = Convert.ToInt32(reader["Estatus"].ToString());
                            agencia.estatusDesc = reader["estatusDesc"].ToString();
                            ListaAgencias.Add(agencia);
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
            return ListaAgencias;


        }


    }
    }

