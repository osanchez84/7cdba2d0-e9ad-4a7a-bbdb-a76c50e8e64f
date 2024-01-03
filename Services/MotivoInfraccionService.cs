﻿using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using GuanajuatoAdminUsuarios.Services;
using System.Collections.Generic;
using System.Data;
using System;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GuanajuatoAdminUsuarios.Services
{
    public class MotivoInfraccionService : IMotivoInfraccionService
    {
        private readonly ISqlClientConnectionBD _sqlClientConnectionBD;

        public MotivoInfraccionService(ISqlClientConnectionBD sqlClientConnectionBD)
        {
            _sqlClientConnectionBD = sqlClientConnectionBD;
        }

        public CatMotivosInfraccionModel GetMotivoByID(int IdCatMotivoInfraccion, int idDependencia)
        {
            List<CatMotivosInfraccionModel> motivos = new List<CatMotivosInfraccionModel>();
            string query = @"SELECT 
                                cmi.fechaInicio,
								cmi.fechaFinVigencia,
	                            cmi.idCatMotivoInfraccion ,
	                            cmi.nombre,
	                            cmi.fundamento , 
	                            cmi.calificacionMinima , 
	                            cmi.calificacionMaxima , 
	                            cmi.IdConcepto , 
	                            c.concepto ,
                                cmi.idSubConcepto,
	                            sc.subConcepto ,
	                            e.estatusDesc as ValorEstatusMotivosInfraccion
                            FROM 
                            catMotivosInfraccion cmi
                            JOIN estatus e ON cmi.estatus = e.estatus
                            JOIN catConceptoInfraccion c ON cmi.idConcepto = c.idConcepto
                            JOIN catSubConceptoInfraccion sc ON cmi.idSubConcepto = sc.idSubConcepto
                            WHERE idCatMotivoInfraccion  = @idCatMotivoInfraccion AND transito = @idDependencia";


            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@idDependencia", idDependencia);
					command.Parameters.Add(new SqlParameter("@idCatMotivoInfraccion", SqlDbType.Int)).Value = (object)IdCatMotivoInfraccion ?? DBNull.Value;
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            CatMotivosInfraccionModel model = new CatMotivosInfraccionModel();
                            model.IdCatMotivoInfraccion = reader["idCatMotivoInfraccion"] == System.DBNull.Value ? default(int) : Convert.ToInt32(reader["idCatMotivoInfraccion"].ToString());
                            model.Nombre = reader["nombre"].ToString();
                            model.Fundamento = reader["fundamento"].ToString();
                            model.CalificacionMinima = reader["calificacionMinima"] == System.DBNull.Value ? default(int) : Convert.ToInt32(reader["calificacionMinima"].ToString());
                            model.CalificacionMaxima = reader["calificacionMaxima"] == System.DBNull.Value ? default(int) : Convert.ToInt32(reader["calificacionMaxima"].ToString());
                            model.idConcepto = reader["IdConcepto"] == System.DBNull.Value ? default(int) : Convert.ToInt32(reader["IdConcepto"].ToString());
                            model.concepto = reader["concepto"].ToString();
                            model.idSubConcepto = reader["IdSubConcepto"] == System.DBNull.Value ? default(int) : Convert.ToInt32(reader["IdSubConcepto"].ToString());
                            model.subConcepto = reader["subConcepto"].ToString();
                            model.ValorEstatusMotivosInfraccion = reader["ValorEstatusMotivosInfraccion"].ToString() == "inactivo" ? false :true;
                            model.fechaInicioVigencia = reader["fechaInicio"] == System.DBNull.Value ? default(DateTime) : Convert.ToDateTime(reader["fechaInicio"]);
                            model.fechaFinVigencia = reader["fechaFinVigencia"] == System.DBNull.Value ? default(DateTime) : Convert.ToDateTime(reader["fechaFinVigencia"]);

                            motivos.Add(model);
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
            }
            return motivos.FirstOrDefault();
        }

        public List<CatMotivosInfraccionModel> GetMotivos(int idDependencia)
        {
            List<CatMotivosInfraccionModel> motivos = new List<CatMotivosInfraccionModel>();
            string query = @"SELECT 
	                            cmi.idCatMotivoInfraccion ,
	                            cmi.nombre,
	                            cmi.fundamento , 
	                            cmi.calificacionMinima , 
	                            cmi.calificacionMaxima , 
	                            cmi.IdConcepto ,
                                cmi.fechaInicio ,
                                cmi.fechaFinVigencia,
	                            c.concepto ,
                                cmi.idSubConcepto,
	                            sc.subConcepto ,
	                            e.estatusDesc
                            FROM 
                            catMotivosInfraccion cmi
                            LEFT JOIN estatus e ON cmi.estatus = e.estatus
                            LEFT JOIN catConceptoInfraccion c ON cmi.idConcepto = c.idConcepto
                            LEFT JOIN catSubConceptoInfraccion sc ON cmi.idSubConcepto = sc.idSubConcepto 
                            WHERE transito = @idDependencia";


            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@idDependencia", idDependencia);
					using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            CatMotivosInfraccionModel model = new CatMotivosInfraccionModel();
                            model.IdCatMotivoInfraccion = reader["idCatMotivoInfraccion"] == System.DBNull.Value ? default(int) : Convert.ToInt32(reader["idCatMotivoInfraccion"].ToString());
                            model.Nombre = reader["nombre"].ToString();
                            model.Fundamento = reader["fundamento"].ToString();
                            model.CalificacionMinima = reader["calificacionMinima"] == System.DBNull.Value ? default(int) : Convert.ToInt32(reader["calificacionMinima"].ToString());
                            model.CalificacionMaxima = reader["calificacionMaxima"] == System.DBNull.Value ? default(int) : Convert.ToInt32(reader["calificacionMaxima"].ToString());
                            model.idConcepto = reader["IdConcepto"] == System.DBNull.Value ? default(int) : Convert.ToInt32(reader["IdConcepto"].ToString());
                            model.concepto = reader["concepto"].ToString();
                            model.idSubConcepto = reader["IdSubConcepto"] == System.DBNull.Value ? default(int) : Convert.ToInt32(reader["IdSubConcepto"].ToString());
                            model.subConcepto = reader["subConcepto"].ToString();
                            model.estatusDesc = reader["estatusDesc"] is DBNull ? string.Empty : reader["estatusDesc"].ToString();
                            model.fechaInicioVigencia = reader["fechaInicio"] == System.DBNull.Value ? default(DateTime) : Convert.ToDateTime(reader["fechaInicio"]);
                            model.fechaFinVigencia = reader["fechaFinVigencia"] == System.DBNull.Value ? default(DateTime) : Convert.ToDateTime(reader["fechaFinVigencia"]);

                            motivos.Add(model);
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
            }
            return motivos;
        }


        public List<CatMotivosInfraccionModel> GetCatMotivos(int idDependencia)
        {
            List<CatMotivosInfraccionModel> motivos = new List<CatMotivosInfraccionModel>();
            string query = @"SELECT 
                            idCatMotivoInfraccion , nombre 
                            FROM 
                            catMotivosInfraccion cmi WHERE transito = @idDependencia";


            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@idDependencia", idDependencia);
					using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            CatMotivosInfraccionModel model = new CatMotivosInfraccionModel();
                            model.IdCatMotivoInfraccion = reader["idCatMotivoInfraccion"] == System.DBNull.Value ? default(int) : Convert.ToInt32(reader["idCatMotivoInfraccion"].ToString());
                            model.Nombre = reader["nombre"].ToString();
                            
                            motivos.Add(model);
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
            }
            return motivos;
        }

        public int CrearMotivo(CatMotivosInfraccionModel motivo, int idDependencia)
        {
            int result = 0;

            string query = @"INSERT INTO catMotivosInfraccion
                            (nombre
                                , IdSubConcepto
                                , fechaActualizacion
                                , actualizadoPor
                                , estatus
                                , IdConcepto
                                , calificacionMinima
                                , calificacionMaxima
                                , fundamento
                                , fechaInicio
                                , fechaFinVigencia)
                            VALUES(@nombre
                                , @IdSubConcepto
                                , @fechaActualizacion
                                , @actualizadoPor
                                , @estatus
                                , @IdConcepto
                                , @calificacionMinima
                                , @calificacionMaxima
                                , @fundamento
                                , @fechaInicioVigencia
                                , @fechaFinalVigencia
                                , @idDependencia)";

            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandType = CommandType.Text;
                    command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@idDependencia", idDependencia);
					command.Parameters.AddWithValue("@nombre", motivo.Nombre);
                    command.Parameters.AddWithValue("@IdSubConcepto", motivo.idSubConcepto);
                    command.Parameters.AddWithValue("@IdConcepto", motivo.idConcepto);
                    command.Parameters.AddWithValue("@calificacionMinima", motivo.CalificacionMinima);
                    command.Parameters.AddWithValue("@calificacionMaxima", motivo.CalificacionMaxima);
                    command.Parameters.AddWithValue("@fundamento", motivo.Fundamento);
                    command.Parameters.AddWithValue("@fechaInicioVigencia", motivo.fechaInicioVigencia);
                    command.Parameters.AddWithValue("@fechaFinalVigencia", motivo.fechaFinVigencia);

                    command.Parameters.AddWithValue("@fechaActualizacion", DateTime.Now);
                    command.Parameters.AddWithValue("@actualizadoPor", 1);
                    command.Parameters.AddWithValue("@estatus", 1);


                    result = Convert.ToInt32(command.ExecuteScalar());
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


        public int UpdateMotivo(CatMotivosInfraccionModel motivo, int idDependencia)
        {
            int result = 0;
            string strQuery = @"UPDATE catMotivosInfraccion
                                SET nombre=@nombre
                                , IdSubConcepto=@IdSubConcepto
                                , fechaActualizacion=@fechaActualizacion
                                , actualizadoPor=@actualizadoPor
                                , estatus=@estatus
                                , IdConcepto=@IdConcepto
                                , calificacionMinima=@calificacionMinima
                                , calificacionMaxima=@calificacionMaxima
                                , fundamento=@fundamento
                                , transito=@idDependencia
                                WHERE idCatMotivoInfraccion=@idCatMotivoInfraccion";

            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(strQuery, connection);
                    command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@idDependencia", idDependencia);
					command.Parameters.AddWithValue("@idCatMotivoInfraccion", motivo.IdCatMotivoInfraccion);
                    command.Parameters.AddWithValue("@nombre", motivo.Nombre);
                    command.Parameters.AddWithValue("@IdSubConcepto",motivo.idSubConcepto);
                    command.Parameters.AddWithValue("@IdConcepto",motivo.idConcepto);
                    command.Parameters.AddWithValue("@calificacionMinima",motivo.CalificacionMinima);
                    command.Parameters.AddWithValue("@calificacionMaxima", motivo.CalificacionMaxima);
                    command.Parameters.AddWithValue("@fundamento", motivo.Fundamento);

                    command.Parameters.AddWithValue("@fechaActualizacion", DateTime.Now);
                    command.Parameters.AddWithValue("@actualizadoPor", 1);
                    command.Parameters.AddWithValue("@estatus", 1);

                    result = command.ExecuteNonQuery();
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



        public int DeleteMotivo(CatMotivosInfraccionModel model)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("DELETE FROM catMotivosInfraccion WHERE idCatMotivoInfraccion=@idCatMotivo", connection);
                    command.Parameters.Add(new SqlParameter("@idCatMotivo", SqlDbType.Int)).Value = model.IdCatMotivoInfraccion;
                    command.CommandType = CommandType.Text;
                    result = command.ExecuteNonQuery();

                }
                catch (Exception ex)
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
    }
}