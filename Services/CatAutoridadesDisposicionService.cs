using GuanajuatoAdminUsuarios.Entity;
using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace GuanajuatoAdminUsuarios.Services
{
    public class CatAutoridadesDisposicionService : ICatAutoridadesDisposicionService
    {
        private readonly ISqlClientConnectionBD _sqlClientConnectionBD;
        public CatAutoridadesDisposicionService(ISqlClientConnectionBD sqlClientConnectionBD)
        {
            _sqlClientConnectionBD = sqlClientConnectionBD;
        }

        public List<CatAutoridadesDisposicionModel> ObtenerAutoridadesActivas()
        {
            //
            List<CatAutoridadesDisposicionModel> ListaAutoridades = new List<CatAutoridadesDisposicionModel>();

            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
                try

                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT catAutoridadesDisposicion.*, estatus.estatusdesc FROM catAutoridadesDisposicion JOIN estatus ON catAutoridadesDisposicion.estatus = estatus.estatus" +
                                                        " WHERE catAutoridadesDisposicion.estatus = 1  ORDER BY NombreAutoridadDisposicion ASC", connection);
                    command.CommandType = CommandType.Text;
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            CatAutoridadesDisposicionModel autoridad = new CatAutoridadesDisposicionModel();
                            autoridad.IdAutoridadDisposicion = Convert.ToInt32(reader["IdAutoridadDisposicion"].ToString());
                            autoridad.NombreAutoridadDisposicion = reader["NombreAutoridadDisposicion"].ToString();
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
