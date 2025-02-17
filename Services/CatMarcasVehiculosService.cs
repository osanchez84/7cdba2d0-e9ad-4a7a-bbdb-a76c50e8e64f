﻿using GuanajuatoAdminUsuarios.Entity;
using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace GuanajuatoAdminUsuarios.Services
{
    public class CatMarcasVehiculosService : ICatMarcasVehiculosService
    {
        private readonly ISqlClientConnectionBD _sqlClientConnectionBD;
        public CatMarcasVehiculosService(ISqlClientConnectionBD sqlClientConnectionBD)
        {
            _sqlClientConnectionBD = sqlClientConnectionBD;
        }

        /// <summary>
        /// Consulta de Categorias con SQLClient
        /// </summary>
        /// <returns></returns>
        public List<CatMarcasVehiculosModel> ObtenerMarcas()
        {
            //
            List<CatMarcasVehiculosModel> marcas = new List<CatMarcasVehiculosModel>();

            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
                try

                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT catMarcasVehiculos.*, estatus.estatusdesc FROM catMarcasVehiculos JOIN estatus ON catMarcasVehiculos.estatus = estatus.estatus ORDER BY MarcaVehiculo ASC", connection);
                    command.CommandType = CommandType.Text;
                    //sqlData Reader sirve para la obtencion de datos 
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            CatMarcasVehiculosModel marcasVehiculo = new CatMarcasVehiculosModel();
                            marcasVehiculo.IdMarcaVehiculo = Convert.ToInt32(reader["IdMarcaVehiculo"].ToString());
                            marcasVehiculo.MarcaVehiculo = reader["MarcaVehiculo"].ToString();
                            //marcasVehiculo.FechaActualizacion = Convert.ToDateTime(reader["FechaActualizacion"].ToString());
                            //marcasVehiculo.ActualizadoPor = Convert.ToInt32(reader["ActualizadoPor"].ToString());
                            marcasVehiculo.Estatus = Convert.ToInt32(reader["Estatus"].ToString());
                            marcasVehiculo.estatusDesc = reader["estatusDesc"].ToString();
                            marcas.Add(marcasVehiculo);
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
            return marcas;


        }

        public CatMarcasVehiculosModel GetMarcaByID(int IdMarcaVehiculo)
        {
            CatMarcasVehiculosModel marca = new CatMarcasVehiculosModel();
            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("Select * from catMarcasVehiculos where idMarcaVehiculo=@idMarcaVehiculo", connection);
                    command.Parameters.Add(new SqlParameter("@idMarcaVehiculo", SqlDbType.Int)).Value = IdMarcaVehiculo;
                    command.CommandType = CommandType.Text;
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            marca.IdMarcaVehiculo = Convert.ToInt32(reader["IdMarcaVehiculo"].ToString());
                            marca.MarcaVehiculo = reader["MarcaVehiculo"].ToString();
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

            return marca;
        }

        public int GuardarMarca(CatMarcasVehiculosModel marca)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
            {
                try
                {
                    connection.Open();
                    SqlCommand sqlCommand = new SqlCommand("Insert into catMarcasVehiculos(marcaVehiculo,estatus,fechaActualizacion) values(@MarcaVehiculo,@Estatus,@FechaActualizacion)", connection);
                    sqlCommand.Parameters.Add(new SqlParameter("@MarcaVehiculo", SqlDbType.VarChar)).Value = marca.MarcaVehiculo;
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
        public int UpdateMarca(CatMarcasVehiculosModel marca)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
            {
                try
                {
                    connection.Open();
                    SqlCommand sqlCommand = new
                        SqlCommand("Update catMarcasVehiculos set MarcaVehiculo=@MarcaVehiculo, estatus = @Estatus where idMarcaVehiculo=@idMarcaVehiculo",
                        connection);
                    sqlCommand.Parameters.Add(new SqlParameter("@idMarcaVehiculo", SqlDbType.Int)).Value = marca.IdMarcaVehiculo;
                    sqlCommand.Parameters.Add(new SqlParameter("@MarcaVehiculo", SqlDbType.VarChar)).Value = marca.MarcaVehiculo;
                    sqlCommand.Parameters.Add(new SqlParameter("@Estatus", SqlDbType.Int)).Value = marca.Estatus;

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
        public int obtenerIdPorMarca(string marcaLimpio)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
            {
                try
                {
                    connection.Open();
                    SqlCommand sqlCommand = new SqlCommand("SELECT idMarcaVehiculo FROM catMarcasVehiculos WHERE marcaVehiculo = @marca", connection);
                    sqlCommand.Parameters.Add(new SqlParameter("@marca", SqlDbType.NVarChar)).Value = marcaLimpio;
                    sqlCommand.CommandType = CommandType.Text;

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.Read()) // Intenta leer un registro del resultado
                        {
                            result = Convert.ToInt32(reader["idMarcaVehiculo"]);
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
