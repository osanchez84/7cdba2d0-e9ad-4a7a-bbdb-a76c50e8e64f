using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using System.Collections.Generic;
using System.Data;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Drawing;
using static GuanajuatoAdminUsuarios.Utils.CatalogosEnums;

namespace GuanajuatoAdminUsuarios.Services
{
    public class VehiculosService : IVehiculosService
    {
        private readonly ISqlClientConnectionBD _sqlClientConnectionBD;

        public VehiculosService(ISqlClientConnectionBD sqlClientConnectionBD)
        {
            _sqlClientConnectionBD = sqlClientConnectionBD;
        }

        public IEnumerable<VehiculoModel> GetAllVehiculos()
        {
            List<VehiculoModel> modelList = new List<VehiculoModel>();
            string strQuery = @"SELECT 
                                 v.idVehiculo
                                ,v.placas
                                ,v.serie
                                ,v.tarjeta
                                ,v.vigenciaTarjeta
                                ,v.idMarcaVehiculo
                                ,v.idSubmarca
                                ,v.idTipoVehiculo
                                ,v.modelo
                                ,v.idColor
                                ,v.idEntidad
                                ,v.idCatTipoServicio
                                ,v.propietario
                                ,v.numeroEconomico
                                ,v.paisMaufactura
                                ,v.idPersona
                                ,v.fechaActualizacion
                                ,v.actualizadoPor
                                ,v.estatus
                                ,p.idPersona
                                ,p.numeroLicencia
                                ,p.CURP
                                ,p.RFC
                                ,p.nombre
                                ,p.apellidoPaterno
                                ,p.apellidoMaterno
                                ,p.fechaActualizacion
                                ,p.actualizadoPor
                                ,p.estatus
                                ,p.idCatTipoPersona
								,cmv.marcaVehiculo
								,csv.nombreSubmarca
								,ce.nombreEntidad
								,cv.tipoVehiculo
								,cc.color
                                FROM vehiculos v
								LEFT JOIN catColores cc
								on v.idColor = cc.idColor AND cc.estatus = 1
								LEFT JOIN catTiposVehiculo cv
								on v.idTipoVehiculo = cv.idTipoVehiculo AND cv.estatus = 1
								LEFT JOIN catEntidades ce
								on v.idEntidad = ce.idEntidad AND ce.estatus = 1
                                LEFT JOIN personas p
                                on v.idPersona = p.idPersona AND p.estatus = 1
								LEFT JOIN catMarcasVehiculos cmv
								on v.idMarcaVehiculo = cmv.idMarcaVehiculo and cmv.estatus = 1
								LEFT JOIN catSubmarcasVehiculos csv
								on v.idSubmarca = csv.idSubmarca and csv.estatus = 1
                                WHERE v.estatus = 1";
            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(strQuery, connection);
                    command.CommandType = CommandType.Text;
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            VehiculoModel model = new VehiculoModel();
                            model.Persona = new PersonaModel();
                            model.idVehiculo = Convert.ToInt32(reader["idVehiculo"]);
                            model.placas = reader["placas"].ToString();
                            model.serie = reader["serie"].ToString();
                            model.tarjeta = reader["tarjeta"].ToString();
                            model.vigenciaTarjeta = Convert.ToDateTime(reader["vigenciaTarjeta"].ToString());
                            model.idMarcaVehiculo = Convert.ToInt32(reader["idMarcaVehiculo"].ToString());
                            model.idSubmarca = Convert.ToInt32(reader["idSubmarca"].ToString());
                            model.idTipoVehiculo = Convert.ToInt32(reader["idTipoVehiculo"].ToString());
                            model.modelo = reader["modelo"].ToString();
                            model.idColor = Convert.ToInt32(reader["idColor"].ToString());
                            model.idEntidad = Convert.ToInt32(reader["idEntidad"].ToString());
                            model.idCatTipoServicio = Convert.ToInt32(reader["idCatTipoServicio"].ToString());
                            model.numeroEconomico = reader["numeroEconomico"].ToString();
                            model.fechaActualizacion = Convert.ToDateTime(reader["fechaActualizacion"].ToString());
                            model.actualizadoPor = Convert.ToInt32(reader["actualizadoPor"].ToString());
                            model.estatus = Convert.ToInt32(reader["estatus"].ToString());
                            model.idPersona = reader["idPersona"] == System.DBNull.Value ? default(int?) : (int?)reader["idPersona"];
                            model.Persona.idPersona = reader["idPersona"] == System.DBNull.Value ? default(int) : (int)reader["idPersona"];
                            model.Persona.numeroLicencia = reader["numeroLicencia"].ToString();
                            model.Persona.CURP = reader["CURP"].ToString();
                            model.Persona.RFC = reader["RFC"].ToString();
                            model.Persona.nombre = reader["nombre"].ToString();
                            model.Persona.apellidoPaterno = reader["apellidoPaterno"].ToString();
                            model.Persona.apellidoMaterno = reader["apellidoMaterno"].ToString();
                            model.Persona.fechaActualizacion = reader["fechaActualizacion"] == System.DBNull.Value ? default(DateTime) : Convert.ToDateTime(reader["fechaActualizacion"].ToString());
                            model.Persona.actualizadoPor = reader["actualizadoPor"] == System.DBNull.Value ? default(int) : Convert.ToInt32(reader["actualizadoPor"].ToString());
                            model.Persona.estatus = reader["estatus"] == System.DBNull.Value ? default(int) : Convert.ToInt32(reader["estatus"].ToString());
                            model.Persona.idCatTipoPersona = reader["idCatTipoPersona"] == System.DBNull.Value ? default(int) : Convert.ToInt32(reader["idCatTipoPersona"].ToString());
                            model.marca = reader["marcaVehiculo"].ToString();
                            model.submarca = reader["nombreSubmarca"].ToString();
                            model.tipoVehiculo = reader["tipoVehiculo"].ToString();
                            model.entidadRegistro = reader["nombreEntidad"].ToString();
                            model.color = reader["color"].ToString();
                            model.propietario = model.Persona.nombre + " " + model.Persona.apellidoPaterno + " " + model.Persona.apellidoMaterno;
                            modelList.Add(model);
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
            return modelList;
        }

        public VehiculoModel GetVehiculoById(int idVehiculo)
        {
            List<VehiculoModel> modelList = new List<VehiculoModel>();
            string strQuery = @"SELECT 
                                 v.idVehiculo
                                ,v.placas
                                ,v.serie
                                ,v.tarjeta
                                ,v.vigenciaTarjeta
                                ,v.idMarcaVehiculo
                                ,v.idSubmarca
                                ,v.idTipoVehiculo
                                ,v.modelo
                                ,v.idColor
                                ,v.idEntidad
                                ,v.idCatTipoServicio
                                ,v.propietario
                                ,v.numeroEconomico
                                ,v.paisMaufactura
                                ,v.idPersona
                                ,v.fechaActualizacion
                                ,v.actualizadoPor
                                ,v.estatus
                                ,p.idPersona
                                ,p.numeroLicencia
                                ,p.CURP
                                ,p.RFC
                                ,p.nombre
                                ,p.apellidoPaterno
                                ,p.apellidoMaterno
                                ,p.fechaActualizacion
                                ,p.actualizadoPor
                                ,p.estatus
                                ,p.idCatTipoPersona
								,cmv.marcaVehiculo
								,csv.nombreSubmarca
								,ce.nombreEntidad
								,cv.tipoVehiculo
								,cc.color
                                FROM vehiculos v
								LEFT JOIN catColores cc
								on v.idColor = cc.idColor AND cc.estatus = 1
								LEFT JOIN catTiposVehiculo cv
								on v.idTipoVehiculo = cv.idTipoVehiculo AND cv.estatus = 1
								LEFT JOIN catEntidades ce
								on v.idEntidad = ce.idEntidad AND ce.estatus = 1
                                LEFT JOIN personas p
                                on v.idPersona = p.idPersona AND p.estatus = 1
								LEFT JOIN catMarcasVehiculos cmv
								on v.idMarcaVehiculo = cmv.idMarcaVehiculo and cmv.estatus = 1
								LEFT JOIN catSubmarcasVehiculos csv
								on v.idSubmarca = csv.idSubmarca and csv.estatus = 1
                                WHERE v.estatus = 1
                                AND v.idVehiculo = @idVehiculo";
            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(strQuery, connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("@idVehiculo", SqlDbType.Int)).Value = (object)idVehiculo ?? DBNull.Value;
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            VehiculoModel model = new VehiculoModel();
                            model.Persona = new PersonaModel();
                            model.idVehiculo = Convert.ToInt32(reader["idVehiculo"]);
                            model.placas = reader["placas"].ToString();
                            model.serie = reader["serie"].ToString();
                            model.tarjeta = reader["tarjeta"].ToString();
                            model.vigenciaTarjeta = Convert.ToDateTime(reader["vigenciaTarjeta"].ToString());
                            model.idMarcaVehiculo = Convert.ToInt32(reader["idMarcaVehiculo"].ToString());
                            model.idSubmarca = Convert.ToInt32(reader["idSubmarca"].ToString());
                            model.idTipoVehiculo = Convert.ToInt32(reader["idTipoVehiculo"].ToString());
                            model.modelo = reader["modelo"].ToString();
                            model.idColor = Convert.ToInt32(reader["idColor"].ToString());
                            model.idEntidad = Convert.ToInt32(reader["idEntidad"].ToString());
                            model.idCatTipoServicio = Convert.ToInt32(reader["idCatTipoServicio"].ToString());
                            model.numeroEconomico = reader["numeroEconomico"].ToString();
                            model.fechaActualizacion = Convert.ToDateTime(reader["fechaActualizacion"].ToString());
                            model.actualizadoPor = Convert.ToInt32(reader["actualizadoPor"].ToString());
                            model.estatus = Convert.ToInt32(reader["estatus"].ToString());
                            model.idPersona = reader["idPersona"] == System.DBNull.Value ? default(int?) : (int?)reader["idPersona"];
                            model.Persona.idPersona = reader["idPersona"] == System.DBNull.Value ? default(int) : (int)reader["idPersona"];
                            model.Persona.numeroLicencia = reader["numeroLicencia"].ToString();
                            model.Persona.CURP = reader["CURP"].ToString();
                            model.Persona.RFC = reader["RFC"].ToString();
                            model.Persona.nombre = reader["nombre"].ToString();
                            model.Persona.apellidoPaterno = reader["apellidoPaterno"].ToString();
                            model.Persona.apellidoMaterno = reader["apellidoMaterno"].ToString();
                            model.Persona.fechaActualizacion = reader["fechaActualizacion"] == System.DBNull.Value ? default(DateTime) : Convert.ToDateTime(reader["fechaActualizacion"].ToString());
                            model.Persona.actualizadoPor = reader["actualizadoPor"] == System.DBNull.Value ? default(int) : Convert.ToInt32(reader["actualizadoPor"].ToString());
                            model.Persona.estatus = reader["estatus"] == System.DBNull.Value ? default(int) : Convert.ToInt32(reader["estatus"].ToString());
                            model.Persona.idCatTipoPersona = reader["idCatTipoPersona"] == System.DBNull.Value ? default(int) : Convert.ToInt32(reader["idCatTipoPersona"].ToString());
                            model.marca = reader["marcaVehiculo"].ToString();
                            model.submarca = reader["nombreSubmarca"].ToString();
                            model.tipoVehiculo = reader["tipoVehiculo"].ToString();
                            model.entidadRegistro = reader["nombreEntidad"].ToString();
                            model.color = reader["color"].ToString();
                            model.propietario = model.Persona.nombre + " " + model.Persona.apellidoPaterno + " " + model.Persona.apellidoMaterno;
                            modelList.Add(model);
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
            return modelList.FirstOrDefault();
        }


        public VehiculoModel GetVehiculoToAnexo(VehiculoBusquedaModel modelSearch)
        {
            VehiculoModel model = new VehiculoModel();
            //ToDo: Logica para buscar primero en el registro estatal
            //Busqueda en Sitteg:
            bool encontradoEnRegistroEstatal = false;

            string strQuery = @"SELECT
                                v.idVehiculo, v.placas, v.serie, v.tarjeta, v.vigenciaTarjeta, v.idMarcaVehiculo
                                ,v.idSubmarca, v.idTipoVehiculo, v.modelo, v.idColor, v.idEntidad, v.idCatTipoServicio
                                ,v.propietario, v.numeroEconomico, v.paisMaufactura, v.idPersona
                                ,catMV.marcaVehiculo, catTV.tipoVehiculo, catSV.nombreSubmarca, catTS.tipoServicio
                                ,catE.nombreEntidad, catC.color  
                                FROM vehiculos v
                                INNER JOIN catMarcasVehiculos catMV on v.idMarcaVehiculo = catMV.idMarcaVehiculo 
                                INNER JOIN catTiposVehiculo catTV on v.idTipoVehiculo = catTV.idTipoVehiculo 
                                INNER JOIN catSubmarcasVehiculos catSV on v.idSubmarca = catSV.idSubmarca 
                                INNER JOIN catTipoServicio catTS on v.idCatTipoServicio = catTS.idCatTipoServicio 
                                INNER JOIN catEntidades catE on v.idEntidad = catE.idEntidad  
                                INNER JOIN catColores catC on v.idColor = catC.idColor  
                                WHERE v.estatus = 1
                                AND (v.idEntidad = @idEntidad 
                                AND v.serie= @Serie 
                                OR v.placas= @Placas 
                                )";
            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(strQuery, connection);                    
                    command.Parameters.Add(new SqlParameter("@idEntidad", SqlDbType.Int)).Value = (object)modelSearch.IdEntidadBusqueda ?? DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@Placas", SqlDbType.NVarChar)).Value = (object)modelSearch.PlacasBusqueda != null ? modelSearch.PlacasBusqueda.ToUpper() : DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@Serie", SqlDbType.NVarChar)).Value = (object)modelSearch.SerieBusqueda != null ? modelSearch.SerieBusqueda.ToUpper() : DBNull.Value;
                    command.CommandType = CommandType.Text;
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            model.idVehiculo = Convert.ToInt32(reader["idVehiculo"]);
                            model.placas = reader["placas"].ToString();

                            model.serie = reader["serie"].ToString();
                            model.tarjeta = reader["tarjeta"].ToString();
                            model.vigenciaTarjeta = Convert.ToDateTime(reader["vigenciaTarjeta"].ToString());
                            model.idMarcaVehiculo = Convert.ToInt32(reader["idMarcaVehiculo"]);
                            model.idSubmarca = Convert.ToInt32(reader["idSubmarca"]);
                            model.idTipoVehiculo = Convert.ToInt32(reader["idTipoVehiculo"]);

                            model.modelo = reader["modelo"].ToString();
                            model.idColor = Convert.ToInt32(reader["idColor"]);
                            model.idEntidad = Convert.ToInt32(reader["idEntidad"]);
                            model.idCatTipoServicio = Convert.ToInt32(reader["idCatTipoServicio"]);
                            model.propietario = reader["propietario"].ToString();
                            model.numeroEconomico = reader["numeroEconomico"].ToString();
                            model.idPersona = reader["idPersona"] == System.DBNull.Value ? default(int?) : (int?)reader["idPersona"];
                            model.paisManufactura = reader["paisMaufactura"].ToString();
                            model.numeroEconomico = reader["numeroEconomico"].ToString();

                            model.marca = reader["marcaVehiculo"].ToString();
                            model.submarca = reader["nombreSubmarca"].ToString();
                            model.tipoVehiculo = reader["tipoVehiculo"].ToString();
                            model.color = reader["color"].ToString();
                            model.entidadRegistro = reader["nombreEntidad"].ToString();
                            model.tipoServicio = reader["tipoServicio"].ToString();
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

            if (encontradoEnRegistroEstatal)
            {
                model.encontradoEn = (int)EstatusBusquedaVehiculo.RegistroEstatal;
            }
            else
            if (model.idVehiculo != 0)
            {
                model.encontradoEn = (int)EstatusBusquedaVehiculo.Sitteg;
            }
            else
            {
                model.encontradoEn = (int)EstatusBusquedaVehiculo.NoEncontrado;
                model.serie = modelSearch.SerieBusqueda;
            }
            return model;
        }
    }
}
