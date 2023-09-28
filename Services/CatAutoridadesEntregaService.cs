using GuanajuatoAdminUsuarios.Entity;
using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace GuanajuatoAdminUsuarios.Services
{
    public class CatAutoridadesEntregaService : ICatAutoridadesEntregaService
    {
        private readonly ISqlClientConnectionBD _sqlClientConnectionBD;
        public CatAutoridadesEntregaService(ISqlClientConnectionBD sqlClientConnectionBD)
        {
            _sqlClientConnectionBD = sqlClientConnectionBD;
        }

        public List<CatAutoridadesEntregaModel> ObtenerAutoridadesActivas()
        {
            //
            List<CatAutoridadesEntregaModel> ListaAutoridades = new List<CatAutoridadesEntregaModel>();

            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
                try

                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT catAutoridadesEntrega.*, estatus.estatusdesc FROM catAutoridadesEntrega JOIN estatus ON catAutoridadesEntrega.estatus = estatus.estatus" +
                                                        " WHERE catAutoridadesEntrega.estatus = 1  ORDER BY AutoridadEntrega ASC", connection);
                    command.CommandType = CommandType.Text;
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            CatAutoridadesEntregaModel autoridad = new CatAutoridadesEntregaModel();
                            autoridad.IdAutoridadEntrega = Convert.ToInt32(reader["IdAutoridadEntrega"].ToString());
                            autoridad.AutoridadEntrega = reader["AutoridadEntrega"].ToString();
                            //marcasVehiculo.FechaActualizacion = Convert.ToDateTime(reader["FechaActualizacion"].ToString());
                            //marcasVehiculo.ActualizadoPor = Convert.ToInt32(reader["ActualizadoPor"].ToString());
                            autoridad.Estatus = Convert.ToInt32(reader["Estatus"].ToString());
                            autoridad.estatusDesc = reader["estatusDesc"].ToString();
                            ListaAutoridades.Add(autoridad);
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
            return ListaAutoridades;


        }

    }
}

