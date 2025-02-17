﻿using GuanajuatoAdminUsuarios.Entity;
using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace GuanajuatoAdminUsuarios.Services
{
    public class CatSubmarcasVehiculosService : ICatSubmarcasVehiculosService
    {
            private readonly ISqlClientConnectionBD _sqlClientConnectionBD;
            public CatSubmarcasVehiculosService(ISqlClientConnectionBD sqlClientConnectionBD)
            {
                _sqlClientConnectionBD = sqlClientConnectionBD;
            }

            /// <summary>
            /// Consulta de Categorias con SQLClient
            /// </summary>
            /// <returns></returns>
            public List<CatSubmarcasVehiculosModel> ObtenerSubarcas()
            {
                //
                List<CatSubmarcasVehiculosModel> submarcas = new List<CatSubmarcasVehiculosModel>();

                using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
                    try

                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("SELECT catSubmarcasVehiculos.*, estatus.estatusdesc, catMarcasVehiculos.marcaVehiculo FROM catSubmarcasVehiculos JOIN estatus ON catSubmarcasVehiculos.estatus = estatus.estatus" +
                            " JOIN catMarcasVehiculos ON catSubmarcasVehiculos.idMarcaVehiculo = catMarcasVehiculos.idMarcaVehiculo ORDER BY NombreSubmarca ASC", connection);
                        command.CommandType = CommandType.Text;
                        //sqlData Reader sirve para la obtencion de datos 
                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (reader.Read())
                            {
                            CatSubmarcasVehiculosModel subMarcasVehiculo = new CatSubmarcasVehiculosModel();
                            subMarcasVehiculo.IdSubmarca = Convert.ToInt32(reader["IdSubmarca"].ToString());
                            subMarcasVehiculo.IdMarcaVehiculo = Convert.ToInt32(reader["IdMarcaVehiculo"].ToString());
                            subMarcasVehiculo.MarcaVehiculo = reader["MarcaVehiculo"].ToString();
                            subMarcasVehiculo.NombreSubmarca = reader["NombreSubmarca"].ToString();
                            //marcasVehiculo.FechaActualizacion = Convert.ToDateTime(reader["FechaActualizacion"].ToString());
                            subMarcasVehiculo.ActualizadoPor = 1;
                            subMarcasVehiculo.Estatus = Convert.ToInt32(reader["Estatus"].ToString());
                            subMarcasVehiculo.estatusDesc = reader["estatusDesc"].ToString();
                            submarcas.Add(subMarcasVehiculo);
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
                return submarcas;


            }

            public CatSubmarcasVehiculosModel GetSubmarcaByID(int IdSubmarca)
            {
            CatSubmarcasVehiculosModel submarca = new CatSubmarcasVehiculosModel();
                using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("Select * from catSubmarcasVehiculos where idSubmarca=@IdSubmarca", connection);
                        command.Parameters.Add(new SqlParameter("@IdSubmarca", SqlDbType.Int)).Value = IdSubmarca;
                        command.CommandType = CommandType.Text;
                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (reader.Read())
                            {
                            submarca.IdSubmarca = Convert.ToInt32(reader["IdSubmarca"].ToString());
                            submarca.IdMarcaVehiculo = Convert.ToInt32(reader["IdMarcaVehiculo"].ToString());
                            submarca.NombreSubmarca = reader["NombreSubmarca"].ToString();
                           // submarca.FechaActualizacion = Convert.ToDateTime(reader["FechaActualizacion"].ToString());
                            submarca.ActualizadoPor = 1;
                            submarca.Estatus = Convert.ToInt32(reader["Estatus"].ToString());
                        }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        connection.Close();
                    }

                return submarca;
            }

            public int GuardarSubmarca(CatSubmarcasVehiculosModel submarca)
            {
                int result = 0;
                using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand sqlCommand = new SqlCommand("Insert into catSubmarcasVehiculos(nombreSubmarca,idMarcaVehiculo,estatus,fechaActualizacion) values(@NombreSubmarca,@idMarcaVehiculo,@Estatus,@FechaActualizacion)", connection);
                        sqlCommand.Parameters.Add(new SqlParameter("@NombreSubmarca", SqlDbType.VarChar)).Value = submarca.NombreSubmarca;
                        sqlCommand.Parameters.Add(new SqlParameter("@idMarcaVehiculo", SqlDbType.VarChar)).Value = submarca.IdMarcaVehiculo;
                        sqlCommand.Parameters.Add(new SqlParameter("@Estatus", SqlDbType.Int)).Value = 1;
                        sqlCommand.Parameters.Add(new SqlParameter("@FechaActualizacion", SqlDbType.DateTime)).Value = DateTime.Now;

                        sqlCommand.CommandType = CommandType.Text;
                        result = sqlCommand.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        return result;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                return result;

            }
            public int UpdateSubmarca(CatSubmarcasVehiculosModel submarca)
            {
                int result = 0;
                using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand sqlCommand = new
                            SqlCommand("Update catSubmarcasVehiculos set nombreSubmarca = @NombreSubmarca, idMarcaVehiculo=@idMarcaVehiculo, estatus = @Estatus where idSubmarca=@idSubmarca",
                            connection);
                    sqlCommand.Parameters.Add(new SqlParameter("@IdSubmarca", SqlDbType.Int)).Value = submarca.IdSubmarca;
                    sqlCommand.Parameters.Add(new SqlParameter("@idMarcaVehiculo", SqlDbType.Int)).Value = submarca.IdMarcaVehiculo;
                        sqlCommand.Parameters.Add(new SqlParameter("@NombreSubmarca", SqlDbType.VarChar)).Value = submarca.NombreSubmarca;
                        sqlCommand.Parameters.Add(new SqlParameter("@Estatus", SqlDbType.Int)).Value = submarca.Estatus;

                        sqlCommand.CommandType = CommandType.Text;
                        result = sqlCommand.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        //---Log
                        return result;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                return result;
            }
        public int obtenerIdPorSubmarca(string submarcaLimpio)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
            {
                try
                {
                    connection.Open();
                    SqlCommand sqlCommand = new SqlCommand("SELECT idSubmarca FROM catSubmarcasVehiculos WHERE nombreSubmarca = @submarca", connection);
                    sqlCommand.Parameters.Add(new SqlParameter("@submarca", SqlDbType.NVarChar)).Value = submarcaLimpio;
                    sqlCommand.CommandType = CommandType.Text;

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.Read()) // Intenta leer un registro del resultado
                        {
                            result = Convert.ToInt32(reader["idSubmarca"]);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Manejo de errores y log
                    return result;
                }
                finally
                {
                    connection.Close();
                }
            }
            return result;
        }
    }
    }

