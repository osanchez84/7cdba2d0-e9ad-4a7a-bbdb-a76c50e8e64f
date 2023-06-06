using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using System.Collections.Generic;
using System.Data;
using System;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using GuanajuatoAdminUsuarios.Entity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Net.NetworkInformation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GuanajuatoAdminUsuarios.Services
{
    public class InfraccionesService : IInfraccionesService
    {
        private readonly ISqlClientConnectionBD _sqlClientConnectionBD;
        private readonly IVehiculosService _vehiculosService;
        private readonly IPersonasService _personasService;
        public InfraccionesService(ISqlClientConnectionBD sqlClientConnectionBD
                                  , IVehiculosService vehiculosService
                                  , IPersonasService personasService)
        {
            _sqlClientConnectionBD = sqlClientConnectionBD;
            _vehiculosService = vehiculosService;
            _personasService = personasService;
        }

        /// <summary>
        /// Agregar Joiun para Tipo Placa
        /// </summary>
        /// <returns></returns>
        public List<Infracciones1Model> GetAllInfracciones()
        {
            List<Infracciones1Model> InfraccionesList = new List<Infracciones1Model>();
            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
                try
                {
                    connection.Open();
                    const string SqlTransact =
                        @"select inf.actualizadoPor,inf.estatus,inf.folioInfraccion,inf.fechaActualizacion,inf.IdInfraccion,inf.placas
                            ,inf.idOficial,inf.idDependencia,inf.idDelegacion,inf.oficial,inf.municipio,inf.fechaInfraccion
                            ,inf.carretera,inf.tramo,inf.kmCarretera,inf.idVehiculo,inf.idConductor,inf.conductor,inf.propietario
                            ,inf.idAplicacion,inf.infraccionCortesia,inf.observaciones,inf.idGarantia,inf.IdEstatusInfraccion
                            ,del.idDelegacion, del.delegacion,dep.idDependencia,dep.nombreDependencia,gar.idGarantia,gar.garantia
                            ,estIn.idEstatusInfraccion, estIn.estatusInfraccion
                            from infracciones inf 
                            inner join dependencias dep on inf.idDependencia= dep.idDependencia
                            inner join delegaciones	del on inf.idDelegacion = del.idDelegacion
                            inner join catGarantias gar on inf.idGarantia = gar.idGarantia
                            inner join catEstatusInfraccion  estIn on inf.IdEstatusInfraccion = estIn.idEstatusInfraccion
                            where inf.estatus=1";
                    SqlCommand command = new SqlCommand(SqlTransact, connection);
                    command.CommandType = CommandType.Text;
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            Infracciones1Model infraccion = new Infracciones1Model();
                            infraccion.IdInfraccion = Convert.ToInt32(reader["IdInfraccion"].ToString());
                            infraccion.folioInfraccion = reader["folioInfraccion"].ToString();
                            infraccion.placas = reader["placas"].ToString();
                            infraccion.idOficial = Convert.ToInt32(reader["idOficial"].ToString());
                            infraccion.idDependencia = Convert.ToInt32(reader["idDependencia"].ToString());
                            infraccion.idDelegacion = Convert.ToInt32(reader["idDelegacion"].ToString());
                            infraccion.oficial = reader["oficial"].ToString();
                            infraccion.municipio = reader["municipio"].ToString();
                            infraccion.fechaInfraccion = Convert.ToDateTime(reader["fechaInfraccion"].ToString());
                            infraccion.carretera = reader["carretera"].ToString();
                            infraccion.tramo = reader["tramo"].ToString();
                            infraccion.kmCarretera = reader["kmCarretera"].ToString();
                            infraccion.idVehiculo = Convert.ToInt32(reader["idVehiculo"].ToString());
                            infraccion.idConductor = Convert.ToInt32(reader["idConductor"].ToString());
                            infraccion.conductor = reader["conductor"].ToString();
                            infraccion.propietario = reader["propietario"].ToString();
                            infraccion.idAplicacion = Convert.ToInt32(reader["idAplicacion"].ToString());
                            infraccion.infraccionCortesia = Convert.ToBoolean(reader["infraccionCortesia"].ToString());
                            infraccion.observaciones = reader["observaciones"].ToString();
                            infraccion.idGarantia = Convert.ToInt32(reader["idGarantia"].ToString());
                            infraccion.garantia = reader["garantia"].ToString();
                            infraccion.delegacion = reader["delegacion"].ToString();
                            infraccion.idEstatusInfraccion = Convert.ToInt32(reader["idEstatusInfraccion"].ToString());
                            infraccion.fechaActualizacion = Convert.ToDateTime(reader["fechaActualizacion"].ToString());
                            infraccion.actualizadoPor = Convert.ToInt32(reader["actualizadoPor"].ToString());
                            infraccion.estatus = Convert.ToInt32(reader["estatus"].ToString());
                            InfraccionesList.Add(infraccion);
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
            return InfraccionesList;
        }

        public List<Infracciones1Model> GetAllInfracciones(InfraccionesBusquedaModel model)
        {
            List<Infracciones1Model> InfraccionesList = new List<Infracciones1Model>();
            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
                try
                {
                    connection.Open();
                    const string SqlTransact =
                            @"select inf.actualizadoPor,inf.estatus,inf.folioInfraccion,inf.fechaActualizacion,inf.IdInfraccion,inf.placas
                            ,inf.idOficial,inf.idDependencia,inf.idDelegacion,inf.oficial,inf.municipio,inf.fechaInfraccion
                            ,inf.carretera,inf.tramo,inf.kmCarretera,inf.idVehiculo,inf.idConductor,inf.conductor,inf.propietario
                            ,inf.idAplicacion,inf.infraccionCortesia,inf.observaciones,inf.idGarantia,inf.IdEstatusInfraccion
                            ,del.idDelegacion, del.delegacion,dep.idDependencia,dep.nombreDependencia,gar.idGarantia,gar.garantia
                            ,estIn.idEstatusInfraccion, estIn.estatusInfraccion
                            from infracciones inf 
                            inner join dependencias dep on inf.idDependencia= dep.idDependencia
                            inner join delegaciones	del on inf.idDelegacion = del.idDelegacion
                            inner join catGarantias gar on inf.idGarantia = gar.idGarantia
                            inner join catEstatusInfraccion  estIn on inf.IdEstatusInfraccion = estIn.idEstatusInfraccion
                            where estIn.idEstatusInfraccion=@IdEstatus OR dep.idDependencia=@IdDependencia
                            OR gar.idGarantia=@IdGarantia OR del.idDelegacion=@IdDelegacion
                            OR inf.fechaInfraccion between @FechaInicio and  @FechaFin
                            OR UPPER(inf.folioInfraccion)=@FolioInfraccion OR UPPER(inf.placas)=@Placas 
                            OR UPPER(inf.propietario)=@Propietario	OR UPPER(inf.conductor)=@Conductor 	 
                            AND  inf.estatus=1";

                    SqlCommand command = new SqlCommand(SqlTransact, connection);
                    command.Parameters.Add(new SqlParameter("@IdGarantia", SqlDbType.Int)).Value = (object)model.IdGarantia ?? DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@IdDelegacion", SqlDbType.Int)).Value = (object)model.IdDelegacion ?? DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@IdEstatus", SqlDbType.Int)).Value = (object)model.IdEstatus ?? DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@IdDependencia", SqlDbType.Int)).Value = (object)model.IdDependencia ?? DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@FolioInfraccion", SqlDbType.NVarChar)).Value = (object)model.folioInfraccion != null ? model.folioInfraccion.ToUpper() : DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@Placas", SqlDbType.NVarChar)).Value = (object)model.placas != null ? model.placas.ToUpper() : DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@Propietario", SqlDbType.NVarChar)).Value = (object)model.Propietario != null ? model.Propietario.ToUpper() : DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@Conductor", SqlDbType.NVarChar)).Value = (object)model.Conductor != null ? model.Conductor.ToUpper() : DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@FechaInicio", SqlDbType.DateTime)).Value = (object)model.FechaInicio ?? DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@FechaFin", SqlDbType.DateTime)).Value = (object)model.FechaFin ?? DBNull.Value;
                    command.CommandType = CommandType.Text;
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            Infracciones1Model infraccion = new Infracciones1Model();
                            infraccion.IdInfraccion = Convert.ToInt32(reader["IdInfraccion"].ToString());
                            infraccion.folioInfraccion = reader["folioInfraccion"].ToString();
                            infraccion.placas = reader["placas"].ToString();
                            infraccion.idOficial = Convert.ToInt32(reader["idOficial"].ToString());
                            infraccion.idDependencia = Convert.ToInt32(reader["idDependencia"].ToString());
                            infraccion.idDelegacion = Convert.ToInt32(reader["idDelegacion"].ToString());
                            infraccion.oficial = reader["oficial"].ToString();
                            infraccion.municipio = reader["municipio"].ToString();
                            infraccion.fechaInfraccion = Convert.ToDateTime(reader["fechaInfraccion"].ToString());
                            infraccion.carretera = reader["carretera"].ToString();
                            infraccion.tramo = reader["tramo"].ToString();
                            infraccion.kmCarretera = reader["kmCarretera"].ToString();
                            infraccion.idVehiculo = Convert.ToInt32(reader["idVehiculo"].ToString());
                            infraccion.idConductor = Convert.ToInt32(reader["idConductor"].ToString());
                            infraccion.conductor = reader["conductor"].ToString();
                            infraccion.propietario = reader["propietario"].ToString();
                            infraccion.idAplicacion = Convert.ToInt32(reader["idAplicacion"].ToString());
                            infraccion.infraccionCortesia = Convert.ToBoolean(reader["infraccionCortesia"].ToString());
                            infraccion.observaciones = reader["observaciones"].ToString();
                            infraccion.idGarantia = Convert.ToInt32(reader["idGarantia"].ToString());
                            infraccion.idEstatusInfraccion = Convert.ToInt32(reader["idEstatusInfraccion"].ToString());
                            infraccion.fechaActualizacion = Convert.ToDateTime(reader["fechaActualizacion"].ToString());
                            infraccion.actualizadoPor = Convert.ToInt32(reader["actualizadoPor"].ToString());
                            infraccion.estatus = Convert.ToInt32(reader["estatus"].ToString());
                            infraccion.garantia = reader["garantia"].ToString();
                            infraccion.delegacion = reader["delegacion"].ToString();
                            InfraccionesList.Add(infraccion);
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
            return InfraccionesList;
        }


        public Infracciones1Model GetInfraccionById(int IdInfraccion)
        {
            Infracciones1Model infraccion = new Infracciones1Model();
            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
                try
                {
                    connection.Open();
                    const string SqlTransact =
                        @"select inf.actualizadoPor,inf.estatus,inf.folioInfraccion,inf.fechaActualizacion,inf.IdInfraccion,inf.placas
                            ,inf.idOficial,inf.idDependencia,inf.idDelegacion,inf.oficial,inf.municipio,inf.fechaInfraccion
                            ,inf.carretera,inf.tramo,inf.kmCarretera,inf.idVehiculo,inf.idConductor,inf.conductor,inf.propietario
                            ,inf.idAplicacion,inf.infraccionCortesia,inf.observaciones,inf.idGarantia,inf.IdEstatusInfraccion
                            ,del.idDelegacion, del.delegacion,dep.idDependencia,dep.nombreDependencia,gar.idGarantia,gar.garantia
                            ,estIn.idEstatusInfraccion, estIn.estatusInfraccion
                            from infracciones inf 
                            inner join dependencias dep on inf.idDependencia= dep.idDependencia
                            inner join delegaciones	del on inf.idDelegacion = del.idDelegacion
                            inner join catGarantias gar on inf.idGarantia = gar.idGarantia
                            inner join catEstatusInfraccion  estIn on inf.IdEstatusInfraccion = estIn.idEstatusInfraccion
                            where inf.estatus=1 and inf.IdInfraccion=@IdInfraccion";
                    SqlCommand command = new SqlCommand(SqlTransact, connection);
                    command.Parameters.Add(new SqlParameter("@IdInfraccion", SqlDbType.Int)).Value = (object)IdInfraccion ?? DBNull.Value;
                    command.CommandType = CommandType.Text;
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {

                            infraccion.IdInfraccion = Convert.ToInt32(reader["IdInfraccion"].ToString());
                            infraccion.folioInfraccion = reader["folioInfraccion"].ToString();
                            infraccion.placas = reader["placas"].ToString();
                            infraccion.idOficial = Convert.ToInt32(reader["idOficial"].ToString());
                            infraccion.idDependencia = Convert.ToInt32(reader["idDependencia"].ToString());
                            infraccion.idDelegacion = Convert.ToInt32(reader["idDelegacion"].ToString());
                            infraccion.oficial = reader["oficial"].ToString();
                            infraccion.municipio = reader["municipio"].ToString();
                            infraccion.fechaInfraccion = Convert.ToDateTime(reader["fechaInfraccion"].ToString());
                            infraccion.carretera = reader["carretera"].ToString();
                            infraccion.tramo = reader["tramo"].ToString();
                            infraccion.kmCarretera = reader["kmCarretera"].ToString();
                            infraccion.idVehiculo = Convert.ToInt32(reader["idVehiculo"].ToString());
                            infraccion.idConductor = Convert.ToInt32(reader["idConductor"].ToString());
                            infraccion.conductor = reader["conductor"].ToString();
                            infraccion.propietario = reader["propietario"].ToString();
                            infraccion.idAplicacion = Convert.ToInt32(reader["idAplicacion"].ToString());
                            infraccion.infraccionCortesia = Convert.ToBoolean(reader["infraccionCortesia"].ToString());
                            infraccion.observaciones = reader["observaciones"].ToString();
                            infraccion.idGarantia = Convert.ToInt32(reader["idGarantia"].ToString());
                            infraccion.garantia = reader["garantia"].ToString();
                            infraccion.delegacion = reader["delegacion"].ToString();
                            infraccion.idEstatusInfraccion = Convert.ToInt32(reader["idEstatusInfraccion"].ToString());
                            infraccion.fechaActualizacion = Convert.ToDateTime(reader["fechaActualizacion"].ToString());
                            infraccion.actualizadoPor = Convert.ToInt32(reader["actualizadoPor"].ToString());
                            infraccion.estatus = Convert.ToInt32(reader["estatus"].ToString());
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
            return infraccion;
        }


        public List<MotivoInfraccionModel> GetMotivosInfraccionByIdInfraccion(int idInfraccion)
        {
            List<MotivoInfraccionModel> modelList = new List<MotivoInfraccionModel>();
            string strQuery = @"SELECT
                                m.idMotivoInfraccion
                                ,m.nombre
                                ,m.fundamento
                                ,m.calificacionMinima
                                ,m.calificacionMaxima
                                ,m.fechaActualizacion
                                ,m.actualizadoPor
                                ,m.estatus
                                ,m.idCatMotivosInfraccion
                                ,m.idInfraccion
                                ,ci.catMotivo
                                ,ci.IdSubConcepto
                                ,csi.subConcepto
                                ,csi.idConcepto
                                ,cci.concepto
                                FROM motivosInfraccion m
                                INNER JOIN catMotivosInfraccion ci
                                on m.idCatMotivosInfraccion = ci.idMotivoInfraccion
                                AND ci.estatus = 1
                                LEFT JOIN catSubConceptoInfraccion csi
                                on ci.IdSubConcepto = csi.idSubConcepto
                                AND csi.estatus = 1
                                LEFT JOIN catConceptoInfraccion cci
                                on csi.idConcepto = cci.idConcepto
                                AND cci.estatus = 1
                                WHERE m.estatus = 1
                                AND idInfraccion = @idInfraccion";
            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(strQuery, connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("@idInfraccion", SqlDbType.Int)).Value = (object)idInfraccion ?? DBNull.Value;
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            MotivoInfraccionModel model = new MotivoInfraccionModel();
                            model.idMotivoInfraccion = reader["idMotivoInfraccion"] == System.DBNull.Value ? default(int) : Convert.ToInt32(reader["idMotivoInfraccion"].ToString());
                            model.nombre = reader["nombre"].ToString();
                            model.fundamento = reader["fundamento"].ToString();
                            model.calificacionMinima = reader["calificacionMinima"] == System.DBNull.Value ? default(int?) : Convert.ToInt32(reader["calificacionMinima"].ToString());
                            model.calificacionMaxima = reader["calificacionMaxima"] == System.DBNull.Value ? default(int?) : Convert.ToInt32(reader["calificacionMaxima"].ToString());
                            model.idCatMotivosInfraccion = reader["idCatMotivosInfraccion"] == System.DBNull.Value ? default(int?) : Convert.ToInt32(reader["idCatMotivosInfraccion"].ToString());
                            model.idInfraccion = reader["idInfraccion"] == System.DBNull.Value ? default(int?) : Convert.ToInt32(reader["idInfraccion"].ToString());
                            model.IdSubConcepto = reader["IdSubConcepto"] == System.DBNull.Value ? default(int?) : Convert.ToInt32(reader["IdSubConcepto"].ToString());
                            model.idConcepto = reader["idConcepto"] == System.DBNull.Value ? default(int?) : Convert.ToInt32(reader["idConcepto"].ToString());
                            model.catMotivo = reader["catMotivo"].ToString();
                            model.subConcepto = reader["subConcepto"].ToString();
                            model.concepto = reader["concepto"].ToString();
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

        public GarantiaInfraccionModel GetGarantiaById(int idGarantia)
        {
            List<GarantiaInfraccionModel> modelList = new List<GarantiaInfraccionModel>();
            string strQuery = @"SELECT 
                                 g.idGarantia
                                ,g.idCatGarantia
                                ,g.idTipoPlaca
                                ,g.idTipoLicencia
                                ,g.numPlaca
                                ,g.numLicencia
                                ,g.vehiculoDocumento
                                ,g.fechaActualizacion
                                ,g.actualizadoPor
                                ,g.estatus
                                ,cg.garantia
                                ,ctp.tipoPlaca
                                ,ctl.tipoLicencia
                                FROM garantiasInfraccion g
                                INNER JOIN catGarantias cg
                                on g.idCatGarantia = cg.idGarantia
                                AND cg.estatus = 1
                                LEFT JOIN catTipoPlaca ctp
                                on g.idTipoPlaca = ctp.idTipoPlaca
                                AND ctp.estatus = 1
                                LEFT JOIN catTipoLicencia ctl
                                on g.idTipoLicencia = ctl.idTipoLicencia
                                AND ctl.estatus = 1
                                AND g.idGarantia = @idGarantia";

            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(strQuery, connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("@idGarantia", SqlDbType.Int)).Value = (object)idGarantia ?? DBNull.Value;
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            GarantiaInfraccionModel model = new GarantiaInfraccionModel();
                            model.idGarantia = reader["idGarantia"] == System.DBNull.Value ? default(int?) : Convert.ToInt32(reader["idGarantia"].ToString());
                            model.idCatGarantia = reader["idCatGarantia"] == System.DBNull.Value ? default(int?) : Convert.ToInt32(reader["idCatGarantia"].ToString());
                            model.idTipoPlaca = reader["idTipoPlaca"] == System.DBNull.Value ? default(int?) : Convert.ToInt32(reader["idTipoPlaca"].ToString());
                            model.idTipoLicencia = reader["idTipoLicencia"] == System.DBNull.Value ? default(int?) : Convert.ToInt32(reader["idTipoLicencia"].ToString());
                            model.numPlaca = reader["numPlaca"].ToString();
                            model.numLicencia = reader["numPlaca"].ToString();
                            model.vehiculoDocumento = reader["vehiculoDocumento"].ToString();
                            model.garantia = reader["garantia"].ToString();
                            model.tipoPlaca = reader["tipoPlaca"].ToString();
                            model.tipoLicencia = reader["tipoLicencia"].ToString();
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

        public PersonaInfraccionModel GetPersonaInfraccionById(int idPersonaInfraccion)
        {
            List<PersonaInfraccionModel> modelList = new List<PersonaInfraccionModel>();
            string strQuery = @"SELECT
                                 idPersonaInfraccion
                                ,numeroLicencia
                                ,CURP
                                ,RFC
                                ,nombre
                                ,apellidoPaterno
                                ,apellidoMaterno
                                ,fechaActualizacion
                                ,actualizadoPor
                                ,estatus
                                ,idCatTipoPersona
                                FROM personasInfracciones
                                WHERE estatus = 1
                                AND idPersonaInfraccion = @idPersonaInfraccion";


            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(strQuery, connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("@idPersonaInfraccion", SqlDbType.Int)).Value = (object)idPersonaInfraccion ?? DBNull.Value;
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            PersonaInfraccionModel model = new PersonaInfraccionModel();
                            model.idPersonaInfraccion = reader["idPersonaInfraccion"] == System.DBNull.Value ? default(int) : Convert.ToInt32(reader["idPersonaInfraccion"].ToString());
                            model.CURP = reader["numeroLicencia"].ToString();
                            model.CURP = reader["CURP"].ToString();
                            model.RFC = reader["RFC"].ToString();
                            model.nombre = reader["nombre"].ToString();
                            model.apellidoPaterno = reader["apellidoPaterno"].ToString();
                            model.apellidoMaterno = reader["apellidoMaterno"].ToString();
                            model.fechaActualizacion = reader["fechaActualizacion"] == System.DBNull.Value ? default(DateTime) : Convert.ToDateTime(reader["fechaActualizacion"].ToString());
                            model.actualizadoPor = reader["actualizadoPor"] == System.DBNull.Value ? default(int) : Convert.ToInt32(reader["actualizadoPor"].ToString());
                            model.estatus = reader["estatus"] == System.DBNull.Value ? default(int) : Convert.ToInt32(reader["estatus"].ToString());
                            model.idCatTipoPersona = reader["estatus"] == System.DBNull.Value ? default(int) : Convert.ToInt32(reader["idCatTipoPersona"].ToString());
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

        /// <summary>
        /// Captura de persona con registro especifico para infraccion, se crea al final del proceso de creacion parte 1
        /// </summary>
        /// <param name="idPersona"></param>
        /// <returns></returns>
        public int CrearPersonaInfraccion(int idPersona)
        {
            int result = 0;
            string strQuery = @"INSERT INTO personasInfracciones(numeroLicencia, CURP, RFC, nombre,apellidoPaterno, apellidoMaterno, idCatTipoPersona, fechaActualizacion, actualizadoPor, estatus)
                                SELECT numeroLicencia, CURP, RFC, nombre,apellidoPaterno, apellidoMaterno, idCatTipoPersona, @fechaActualizacion, @actualizadoPor, @estatus
                                FROM personas
                                WHERE idPersona = @idPersona;SELECT SCOPE_IDENTITY()";
            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(strQuery, connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("idPersona", SqlDbType.Int)).Value = (object)idPersona;
                    command.Parameters.Add(new SqlParameter("fechaActualizacion", SqlDbType.DateTime)).Value = (object)DateTime.Now;
                    command.Parameters.Add(new SqlParameter("actualizadoPor", SqlDbType.Int)).Value = (object)1;
                    command.Parameters.Add(new SqlParameter("estatus", SqlDbType.NVarChar)).Value = (object)1;
                    result = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (SqlException ex)
                {
                }
                finally
                {
                    connection.Close();
                }
            }
            return result;
        }

        public int CrearGarantiaInfraccion(GarantiaInfraccionModel model)
        {
            int result = 0;
            string strQuery = @"INSERT INTO garantiasInfraccion VALUES(@idCatGarantia
                                                                      ,@idTipoPlaca
                                                                      ,@idTipoLicencia
                                                                      ,@numPlaca
                                                                      ,@numLicencia
                                                                      ,@vehiculoDocumento
                                                                      ,@fechaActualizacion
                                                                      ,@actualizadoPor
                                                                      ,@estatus);SELECT SCOPE_IDENTITY()";
            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(strQuery, connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("idCatGarantia", SqlDbType.Int)).Value = (object)model.idCatGarantia;
                    command.Parameters.Add(new SqlParameter("idTipoPlaca", SqlDbType.Int)).Value = (object)model.idTipoPlaca;
                    command.Parameters.Add(new SqlParameter("idTipoLicencia", SqlDbType.Int)).Value = (object)model.idTipoLicencia;
                    command.Parameters.Add(new SqlParameter("numPlaca", SqlDbType.NVarChar)).Value = (object)model.numPlaca;
                    command.Parameters.Add(new SqlParameter("numLicencia", SqlDbType.NVarChar)).Value = (object)model.numLicencia;
                    command.Parameters.Add(new SqlParameter("vehiculoDocumento", SqlDbType.NVarChar)).Value = (object)model.vehiculoDocumento;
                    command.Parameters.Add(new SqlParameter("fechaActualizacion", SqlDbType.DateTime)).Value = (object)DateTime.Now;
                    command.Parameters.Add(new SqlParameter("actualizadoPor", SqlDbType.Int)).Value = (object)1;
                    command.Parameters.Add(new SqlParameter("estatus", SqlDbType.Int)).Value = (object)1;
                    result = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (SqlException ex)
                {
                }
                finally
                {
                    connection.Close();
                }
            }
            return result;
        }

        public int ModificarGarantiaInfraccion(GarantiaInfraccionModel model)
        {
            int result = 0;
            string strQuery = @"UPDATE garantiasInfraccion SET  idCatGarantia = @idCatGarantia
                                                               ,idTipoPlaca = @idTipoPlaca
                                                               ,idTipoLicencia = @idTipoLicencia
                                                               ,numPlaca = @numPlaca
                                                               ,numLicencia = @numLicencia
                                                               ,vehiculoDocumento = @vehiculoDocumento
                                                               ,fechaActualizacion = @fechaActualizacion
                                                               ,actualizadoPor = @actualizadoPor
                                                               WHERE idGarantia = @idGarantia";

            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(strQuery, connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("idCatGarantia", SqlDbType.Int)).Value = (object)model.idCatGarantia;
                    command.Parameters.Add(new SqlParameter("idTipoPlaca", SqlDbType.Int)).Value = (object)model.idTipoPlaca;
                    command.Parameters.Add(new SqlParameter("idTipoLicencia", SqlDbType.Int)).Value = (object)model.idTipoLicencia;
                    command.Parameters.Add(new SqlParameter("numPlaca", SqlDbType.NVarChar)).Value = (object)model.numPlaca;
                    command.Parameters.Add(new SqlParameter("numLicencia", SqlDbType.NVarChar)).Value = (object)model.numLicencia;
                    command.Parameters.Add(new SqlParameter("vehiculoDocumento", SqlDbType.NVarChar)).Value = (object)model.vehiculoDocumento;
                    command.Parameters.Add(new SqlParameter("fechaActualizacion", SqlDbType.DateTime)).Value = (object)DateTime.Now;
                    command.Parameters.Add(new SqlParameter("actualizadoPor", SqlDbType.Int)).Value = (object)1;
                    result = command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                }
                finally
                {
                    connection.Close();
                }
            }
            return result;
        }

        public int CrearMotivoInfraccion(MotivoInfraccionModel model)
        {
            int result = 0;
            string strQuery = @"INSERT INTO motivosInfraccion
                               VALUES (@nombre
                                      ,@fundamento
                                      ,@calificacionMinima
                                      ,@calificacionMaxima
                                      ,@fechaActualizacion
                                      ,@actualizadoPor
                                      ,@estatus
                                      ,@idCatMotivosInfraccion
                                      ,@idInfraccion);SELECT SCOPE_IDENTITY()";
            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(strQuery, connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("nombre", SqlDbType.NVarChar)).Value = (object)model.nombre;
                    command.Parameters.Add(new SqlParameter("fundamento", SqlDbType.NVarChar)).Value = (object)model.fundamento;
                    command.Parameters.Add(new SqlParameter("calificacionMinima", SqlDbType.Int)).Value = (object)model.calificacionMinima;
                    command.Parameters.Add(new SqlParameter("calificacionMaxima", SqlDbType.Int)).Value = (object)model.calificacionMaxima;
                    command.Parameters.Add(new SqlParameter("fechaActualizacion", SqlDbType.DateTime)).Value = (object)DateTime.Now;
                    command.Parameters.Add(new SqlParameter("actualizadoPor", SqlDbType.Int)).Value = (object)1;
                    command.Parameters.Add(new SqlParameter("estatus", SqlDbType.Int)).Value = (object)1;
                    command.Parameters.Add(new SqlParameter("idCatMotivosInfraccion", SqlDbType.Int)).Value = (object)model.idCatMotivosInfraccion;
                    command.Parameters.Add(new SqlParameter("idInfraccion", SqlDbType.Int)).Value = (object)model.idInfraccion;
                    result = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (SqlException ex)
                {
                }
                finally
                {
                    connection.Close();
                }
            }
            return result;
        }

        public int EliminarMotivoInfraccion(int idMotivoInfraccion)
        {
            int result = 0;
            string strQuery = @"UPDATE motivosInfraccion
                                SET fechaActualizacion = @fechaActualizacion
                                    actualizadoPor = @actualizadoPor
                                    estatus = @estatus)
                                WHERE idMotivoInfraccion = @idMotivoInfraccion";
            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(strQuery, connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("idMotivoInfraccion", SqlDbType.Int)).Value = (object)idMotivoInfraccion;
                    command.Parameters.Add(new SqlParameter("fechaActualizacion", SqlDbType.DateTime)).Value = (object)DateTime.Now;
                    command.Parameters.Add(new SqlParameter("actualizadoPor", SqlDbType.Int)).Value = (object)1;
                    command.Parameters.Add(new SqlParameter("estatus", SqlDbType.Int)).Value = (object)0;
                    result = command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                }
                finally
                {
                    connection.Close();
                }
            }
            return result;
        }

        public InfraccionesModel GetInfraccion2ById(int idInfraccion)
        {
            List<InfraccionesModel> modelList = new List<InfraccionesModel>();
            string strQuery = @"SELECT idInfraccion
                                      ,idOficial
                                      ,idDependencia
                                      ,idDelegacion
                                      ,idVehiculo
                                      ,idAplicacion
                                      ,idGarantia
                                      ,idEstatusInfraccion
                                      ,idMunicipio
                                      ,idTramo
                                      ,idCarretera
                                      ,idPersona
                                      ,idPersonaInfraccion
                                      ,placasVehiculo
                                      ,folioInfraccion
                                      ,fechaInfraccion
                                      ,kmCarretera
                                      ,observaciones
                                      ,lugarCalle
                                      ,lugarNumero
                                      ,lugarColonia
                                      ,lugarEntreCalle
                                      ,infraccionCortesia
                                      ,NumTarjetaCirculacion
                                      ,fechaActualizacion
                                      ,actualizadoPor
                                      ,estatus
                               FROM infracciones
                               WHERE estatus = 1
                               AND idInfraccion = @idInfraccion"
            ;

            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(strQuery, connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("@idInfraccion", SqlDbType.Int)).Value = (object)idInfraccion ?? DBNull.Value;
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            InfraccionesModel model = new InfraccionesModel();
                            model.idInfraccion = reader["idInfraccion"] == System.DBNull.Value ? default(int) : Convert.ToInt32(reader["idInfraccion"].ToString());
                            model.idOficial = reader["idOficial"] == System.DBNull.Value ? default(int?) : Convert.ToInt32(reader["idOficial"].ToString());
                            model.idDependencia = reader["idDependencia"] == System.DBNull.Value ? default(int?) : Convert.ToInt32(reader["idDependencia"].ToString());
                            model.idDelegacion = reader["idDelegacion"] == System.DBNull.Value ? default(int?) : Convert.ToInt32(reader["idDelegacion"].ToString());
                            model.idVehiculo = reader["idVehiculo"] == System.DBNull.Value ? default(int?) : Convert.ToInt32(reader["idVehiculo"].ToString());
                            model.idAplicacion = reader["idAplicacion"] == System.DBNull.Value ? default(int?) : Convert.ToInt32(reader["idAplicacion"].ToString());
                            model.idGarantia = reader["idGarantia"] == System.DBNull.Value ? default(int?) : Convert.ToInt32(reader["idGarantia"].ToString());
                            model.idEstatusInfraccion = reader["idEstatusInfraccion"] == System.DBNull.Value ? default(int?) : Convert.ToInt32(reader["idEstatusInfraccion"].ToString());
                            model.idMunicipio = reader["idMunicipio"] == System.DBNull.Value ? default(int?) : Convert.ToInt32(reader["idMunicipio"].ToString());
                            model.idTramo = reader["idTramo"] == System.DBNull.Value ? default(int?) : Convert.ToInt32(reader["idTramo"].ToString());
                            model.idCarretera = reader["idCarretera"] == System.DBNull.Value ? default(int?) : Convert.ToInt32(reader["idCarretera"].ToString());
                            model.idPersona = reader["idPersona"] == System.DBNull.Value ? default(int?) : Convert.ToInt32(reader["idPersona"].ToString());
                            model.idPersonaInfraccion = reader["idPersonaInfraccion"] == System.DBNull.Value ? default(int?) : Convert.ToInt32(reader["idPersonaInfraccion"].ToString());
                            model.placasVehiculo = reader["placasVehiculo"].ToString();
                            model.folioInfraccion = reader["folioInfraccion"].ToString();
                            model.fechaInfraccion = reader["fechaInfraccion"] == System.DBNull.Value ? default(DateTime) : Convert.ToDateTime(reader["fechaInfraccion"].ToString());
                            model.kmCarretera = reader["kmCarretera"] == System.DBNull.Value ? default(int) : Convert.ToInt32(reader["kmCarretera"].ToString());
                            model.observaciones = reader["observaciones"].ToString();
                            model.lugarCalle = reader["lugarCalle"].ToString();
                            model.lugarNumero = reader["lugarNumero"].ToString();
                            model.lugarColonia = reader["lugarColonia"].ToString();
                            model.lugarEntreCalle = reader["lugarEntreCalle"].ToString();
                            model.infraccionCortesia = reader["infraccionCortesia"] == System.DBNull.Value ? default(bool?) : Convert.ToBoolean(reader["infraccionCortesia"].ToString());
                            model.NumTarjetaCirculacion = reader["NumTarjetaCirculacion"].ToString();
                            model.PersonaInfraccion = GetPersonaInfraccionById((int)model.idPersonaInfraccion);
                            model.Vehiculo = _vehiculosService.GetVehiculoById((int)model.idVehiculo);
                            model.MotivosInfraccion = GetMotivosInfraccionByIdInfraccion(model.idInfraccion);
                            model.Garantia = model.idGarantia == null ? new GarantiaInfraccionModel() : GetGarantiaById((int)model.idGarantia);
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

        public int CrearInfraccion(InfraccionesModel model)
        {
            int result = 0;
            string strQuery = @"INSERT INTO infracciones
                                            (fechaInfraccion
                                            ,folioInfraccion
                                            ,idOficial
                                            ,idMunicipio
                                            ,idCarretera
                                            ,idTramo
                                            ,kmCarretera
                                            ,lugarCalle
                                            ,lugarNumero
                                            ,lugarColonia
                                            ,lugarEntreCalle
                                            ,idVehiculo
                                            ,idPersona
                                            ,idPersonaInfraccion
                                            ,placasVehiculo
                                            ,NumTarjetaCirculacion
                                            ,idEstatusInfraccion
                                            ,fechaActualizacion
                                            ,actualizadoPor
                                            ,estatus)
                                     VALUES (@fechaInfraccion
                                            ,@folioInfraccion
                                            ,@idOficial
                                            ,@idMunicipio
                                            ,@idCarretera
                                            ,@idTramo
                                            ,@kmCarretera
                                            ,@lugarCalle
                                            ,@lugarNumero
                                            ,@lugarColonia
                                            ,@lugarEntreCalle
                                            ,@idVehiculo
                                            ,@idPersona
                                            ,@idPersonaInfraccion
                                            ,@placasVehiculo
                                            ,@NumTarjetaCirculacion
                                            ,1
                                            ,fechaActualizacion
                                            ,actualizadoPor
                                            ,estatus);SELECT SCOPE_IDENTITY()";
            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(strQuery, connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("fechaInfraccion", SqlDbType.DateTime)).Value = (object)model.fechaInfraccion;
                    command.Parameters.Add(new SqlParameter("folioInfraccion", SqlDbType.NVarChar)).Value = (object)model.folioInfraccion;
                    command.Parameters.Add(new SqlParameter("idOficial", SqlDbType.Int)).Value = (object)model.idOficial;
                    command.Parameters.Add(new SqlParameter("idMunicipio", SqlDbType.Int)).Value = (object)model.idMunicipio;

                    command.Parameters.Add(new SqlParameter("idCarretera", SqlDbType.Int)).Value = (object)model.idCarretera;
                    command.Parameters.Add(new SqlParameter("idTramo", SqlDbType.Int)).Value = (object)model.idTramo;
                    command.Parameters.Add(new SqlParameter("kmCarretera", SqlDbType.Int)).Value = (object)model.kmCarretera;
                    command.Parameters.Add(new SqlParameter("lugarCalle", SqlDbType.NVarChar)).Value = (object)model.lugarCalle;
                    command.Parameters.Add(new SqlParameter("lugarNumero", SqlDbType.NVarChar)).Value = (object)model.lugarNumero;
                    command.Parameters.Add(new SqlParameter("lugarColonia", SqlDbType.NVarChar)).Value = (object)model.lugarColonia;
                    command.Parameters.Add(new SqlParameter("lugarEntreCalle", SqlDbType.NVarChar)).Value = (object)model.lugarEntreCalle;

                    command.Parameters.Add(new SqlParameter("idVehiculo", SqlDbType.Int)).Value = (object)model.idVehiculo;
                    command.Parameters.Add(new SqlParameter("idPersona", SqlDbType.Int)).Value = (object)model.idPersona;
                    command.Parameters.Add(new SqlParameter("idPersonaInfraccion", SqlDbType.Int)).Value = (object)model.idPersonaInfraccion;
                    command.Parameters.Add(new SqlParameter("placasVehiculo", SqlDbType.NVarChar)).Value = (object)model.placasVehiculo;
                    command.Parameters.Add(new SqlParameter("NumTarjetaCirculacion", SqlDbType.NVarChar)).Value = (object)model.NumTarjetaCirculacion;
                    //command.Parameters.Add(new SqlParameter("idEstatusInfraccion", SqlDbType.Int)).Value = (object)model.idEstatusInfraccion;

                    //command.Parameters.Add(new SqlParameter("idDependencia", SqlDbType.Int)).Value = (object)model.idDependencia;
                    //command.Parameters.Add(new SqlParameter("idDelegacion", SqlDbType.Int)).Value = (object)model.idDelegacion;
                    
                    //command.Parameters.Add(new SqlParameter("idAplicacion", SqlDbType.Int)).Value = (object)model.idAplicacion;
                    //command.Parameters.Add(new SqlParameter("idGarantia", SqlDbType.Int)).Value = (object)model.idGarantia;
                    //command.Parameters.Add(new SqlParameter("observaciones", SqlDbType.NVarChar)).Value = (object)model.observaciones;
                    //command.Parameters.Add(new SqlParameter("infraccionCortesia", SqlDbType.Bit)).Value = (object)model.infraccionCortesia;

                    command.Parameters.Add(new SqlParameter("fechaActualizacion", SqlDbType.DateTime)).Value = (object)DateTime.Now;
                    command.Parameters.Add(new SqlParameter("actualizadoPor", SqlDbType.Int)).Value = (object)1;
                    command.Parameters.Add(new SqlParameter("estatus", SqlDbType.Int)).Value = (object)1;


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

        public int ModificarInfraccion(InfraccionesModel model)
        {
            int result = 0;
            string strQuery = @"INSERT INTO infracciones2
                                       SET idOficial = @idOficial
                                          ,idDependencia = @idDependencia
                                          ,idDelegacion = @idDelegacion
                                          ,idVehiculo = @idVehiculo
                                          ,idAplicacion = @idAplicacion
                                          ,idGarantia = @idGarantia
                                          ,idEstatusInfraccion = @idEstatusInfraccion
                                          ,idMunicipio = @idMunicipio
                                          ,idTramo = @idTramo
                                          ,idCarretera = @idCarretera
                                          ,idPersona = @idPersona
                                          ,idPersonaInfraccion = @idPersonaInfraccion
                                          ,placasVehiculo = @placasVehiculo
                                          ,folioInfraccion = @folioInfraccion
                                          ,fechaInfraccion = @fechaInfraccion
                                          ,kmCarretera = @kmCarretera
                                          ,observaciones = @observaciones
                                          ,lugarCalle = @lugarCalle
                                          ,lugarNumero = @lugarNumero
                                          ,lugarColonia = @lugarColonia
                                          ,lugarEntreCalle = @lugarEntreCalle
                                          ,infraccionCortesia = @infraccionCortesia
                                          ,NumTarjetaCirculacion = @NumTarjetaCirculacion
                                          ,fechaActualizacion = @fechaActualizacion
                                          ,actualizadoPor = @actualizadoPor";
            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(strQuery, connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("idOficial", SqlDbType.Int)).Value = (object)model.idOficial;
                    command.Parameters.Add(new SqlParameter("idDependencia", SqlDbType.Int)).Value = (object)model.idDependencia;
                    command.Parameters.Add(new SqlParameter("idDelegacion", SqlDbType.Int)).Value = (object)model.idDelegacion;
                    command.Parameters.Add(new SqlParameter("idVehiculo", SqlDbType.Int)).Value = (object)model.idVehiculo;
                    command.Parameters.Add(new SqlParameter("idAplicacion", SqlDbType.Int)).Value = (object)model.idAplicacion;
                    command.Parameters.Add(new SqlParameter("idGarantia", SqlDbType.Int)).Value = (object)model.idGarantia;
                    command.Parameters.Add(new SqlParameter("idEstatusInfraccion", SqlDbType.Int)).Value = (object)model.idEstatusInfraccion;
                    command.Parameters.Add(new SqlParameter("idMunicipio", SqlDbType.Int)).Value = (object)model.idMunicipio;
                    command.Parameters.Add(new SqlParameter("idTramo", SqlDbType.Int)).Value = (object)model.idTramo;
                    command.Parameters.Add(new SqlParameter("idCarretera", SqlDbType.Int)).Value = (object)model.idCarretera;
                    command.Parameters.Add(new SqlParameter("idPersona", SqlDbType.Int)).Value = (object)model.idPersona;
                    command.Parameters.Add(new SqlParameter("idPersonaInfraccion", SqlDbType.Int)).Value = (object)model.idPersonaInfraccion;
                    command.Parameters.Add(new SqlParameter("placasVehiculo", SqlDbType.NVarChar)).Value = (object)model.placasVehiculo;
                    command.Parameters.Add(new SqlParameter("folioInfraccion", SqlDbType.NVarChar)).Value = (object)model.folioInfraccion;
                    command.Parameters.Add(new SqlParameter("fechaInfraccion", SqlDbType.DateTime)).Value = (object)model.fechaInfraccion;
                    command.Parameters.Add(new SqlParameter("kmCarretera", SqlDbType.Int)).Value = (object)model.kmCarretera;
                    command.Parameters.Add(new SqlParameter("observaciones", SqlDbType.NVarChar)).Value = (object)model.observaciones;
                    command.Parameters.Add(new SqlParameter("lugarCalle", SqlDbType.NVarChar)).Value = (object)model.lugarCalle;
                    command.Parameters.Add(new SqlParameter("lugarNumero", SqlDbType.NVarChar)).Value = (object)model.lugarNumero;
                    command.Parameters.Add(new SqlParameter("lugarColonia", SqlDbType.NVarChar)).Value = (object)model.lugarColonia;
                    command.Parameters.Add(new SqlParameter("lugarEntreCalle", SqlDbType.NVarChar)).Value = (object)model.lugarEntreCalle;
                    command.Parameters.Add(new SqlParameter("infraccionCortesia", SqlDbType.Bit)).Value = (object)model.infraccionCortesia;
                    command.Parameters.Add(new SqlParameter("NumTarjetaCirculacion", SqlDbType.NVarChar)).Value = (object)model.NumTarjetaCirculacion;
                    command.Parameters.Add(new SqlParameter("fechaActualizacion", SqlDbType.DateTime)).Value = (object)DateTime.Now;
                    command.Parameters.Add(new SqlParameter("actualizadoPor", SqlDbType.Int)).Value = (object)1;


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

        public Infracciones1Model GetInfraccionCompleteById(int IdInfraccion)
        {
            Infracciones1Model infraccion = new Infracciones1Model();
            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
                try
                {
                    connection.Open();
                    const string SqlTransact =
                        @"select inf.actualizadoPor,inf.estatus,inf.folioInfraccion,inf.fechaActualizacion,inf.IdInfraccion,inf.placas
                        ,inf.idOficial,inf.idDependencia,inf.idDelegacion,inf.oficial,inf.municipio,inf.fechaInfraccion
                        ,inf.carretera,inf.tramo,inf.kmCarretera,inf.idVehiculo,inf.idConductor,inf.conductor,inf.propietario
                        ,inf.idAplicacion,inf.infraccionCortesia,inf.observaciones,inf.idGarantia,inf.IdEstatusInfraccion
                        ,del.idDelegacion, del.delegacion,dep.idDependencia,dep.nombreDependencia,catGar.idGarantia,catGar.garantia
                        ,estIn.idEstatusInfraccion, estIn.estatusInfraccion
                        ,gar.idGarantia,gar.numPlaca,gar.numLicencia,gar.vehiculoDocumento
                        ,tipoP.idTipoPlaca, tipoP.tipoPlaca
                        ,tipoL.idTipoLicencia, tipoL.tipoLicencia
                        ,catOfi.idOficial,catOfi.nombre,catOfi.apellidoPaterno,catOfi.apellidoMaterno,catOfi.rango
                        ,catMun.idMunicipio,catMun.municipio
                        ,catTra.idTramo,catTra.tramo
                        ,catCarre.idCarretera,catCarre.carretera
                        ,veh.marca,veh.submarca, veh.serie,veh.tarjeta, veh.vigenciaTarjeta,veh.tipoVehiculo,veh.modelo
                        ,veh.color,veh.entidadRegistro,veh.tipoServicio, veh.propietario, veh.numeroEconomico
                        ,motInf.idMotivoInfraccion,motInf.nombre,motInf.fundamento,motInf.calificacionMinima,motInf.calificacionMaxima
                        ,catMotInf.idMotivoInfraccion,catMotInf.nombre,catMotInf.fundamento
                        ,catSubInf.idSubConcepto,catSubInf.subConcepto
                        ,catConInf.idConcepto,catConInf.concepto
                        from infracciones inf 
                        inner join dependencias dep on inf.idDependencia= dep.idDependencia
                        inner join delegaciones	del on inf.idDelegacion = del.idDelegacion
                        inner join catEstatusInfraccion  estIn on inf.IdEstatusInfraccion = estIn.idEstatusInfraccion
                        inner join catGarantias catGar on inf.idGarantia = catGar.idGarantia
                        inner join garantiasInfraccion gar on catGar.idGarantia= gar.idCatGarantia
                        inner join catTipoPlaca  tipoP on gar.idTipoPlaca=tipoP.idTipoPlaca
                        inner join catTipoLicencia tipoL on tipoL.idTipoLicencia= gar.idTipoLicencia
                        inner join catOficiales catOfi on inf.idOficial = catOfi.idOficial
                        inner join catMunicipios catMun on inf.idMunicipio =catMun.idMunicipio
                        inner join catTramos catTra on inf.idCarretera = catTra.idTramo
                        inner join catCarreteras catCarre on catTra.IdCarretera = catCarre.idCarretera
                        inner join vehiculos veh on inf.idVehiculo = veh.idVehiculo
                        inner join motivosInfraccion motInf on inf.IdInfraccion = motInf.idInfraccion
                        inner join catMotivosInfraccion catMotInf on motInf.idCatMotivosInfraccion = catMotInf.idMotivoInfraccion
                        inner join catSubConceptoInfraccion catSubInf on catMotInf.IdSubConcepto = catSubInf.idSubConcepto
                        inner join catConceptoInfraccion catConInf on  catSubInf.idConcepto = catConInf.idConcepto
                        where inf.estatus=1 and inf.IdInfraccion=@IdInfraccion";

                    SqlCommand command = new SqlCommand(SqlTransact, connection);
                    command.Parameters.Add(new SqlParameter("@IdInfraccion", SqlDbType.Int)).Value = (object)IdInfraccion ?? DBNull.Value;
                    command.CommandType = CommandType.Text;
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {

                            infraccion.IdInfraccion = Convert.ToInt32(reader["IdInfraccion"].ToString());
                            infraccion.folioInfraccion = reader["folioInfraccion"].ToString();
                            infraccion.placas = reader["placas"].ToString();
                            infraccion.idOficial = Convert.ToInt32(reader["idOficial"].ToString());
                            infraccion.idDependencia = Convert.ToInt32(reader["idDependencia"].ToString());
                            infraccion.idDelegacion = Convert.ToInt32(reader["idDelegacion"].ToString());
                            infraccion.oficial = reader["oficial"].ToString();
                            infraccion.municipio = reader["municipio"].ToString();
                            infraccion.fechaInfraccion = Convert.ToDateTime(reader["fechaInfraccion"].ToString());
                            infraccion.carretera = reader["carretera"].ToString();
                            infraccion.tramo = reader["tramo"].ToString();
                            infraccion.kmCarretera = reader["kmCarretera"].ToString();
                            infraccion.idVehiculo = Convert.ToInt32(reader["idVehiculo"].ToString());
                            infraccion.idConductor = Convert.ToInt32(reader["idConductor"].ToString());
                            infraccion.conductor = reader["conductor"].ToString();
                            infraccion.propietario = reader["propietario"].ToString();
                            infraccion.idAplicacion = Convert.ToInt32(reader["idAplicacion"].ToString());
                            infraccion.infraccionCortesia = Convert.ToBoolean(reader["infraccionCortesia"].ToString());
                            infraccion.observaciones = reader["observaciones"].ToString();
                            infraccion.idGarantia = Convert.ToInt32(reader["idGarantia"].ToString());
                            infraccion.garantia = reader["garantia"].ToString();
                            infraccion.delegacion = reader["delegacion"].ToString();
                            infraccion.idEstatusInfraccion = Convert.ToInt32(reader["idEstatusInfraccion"].ToString());
                            infraccion.fechaActualizacion = Convert.ToDateTime(reader["fechaActualizacion"].ToString());
                            infraccion.actualizadoPor = Convert.ToInt32(reader["actualizadoPor"].ToString());
                            infraccion.estatus = Convert.ToInt32(reader["estatus"].ToString());
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
            return infraccion;
        }


    }
}
