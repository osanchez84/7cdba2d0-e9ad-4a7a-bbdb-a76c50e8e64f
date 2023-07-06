using GuanajuatoAdminUsuarios.Entity;
using GuanajuatoAdminUsuarios.Interfaces;
using System.Collections.Generic;
using System.Data;
using System;
using GuanajuatoAdminUsuarios.Models;
using System.Data.SqlClient;

namespace GuanajuatoAdminUsuarios.Services
{
    public class BusquedaAccidentesService : IBusquedaAccidentesService
    {
        private readonly ISqlClientConnectionBD _sqlClientConnectionBD;
        public BusquedaAccidentesService(ISqlClientConnectionBD sqlClientConnectionBD)
        {
            _sqlClientConnectionBD = sqlClientConnectionBD;
        }
        public List<BusquedaAccidentesModel> BusquedaAccidentes(BusquedaAccidentesModel model)
        {
            //
            List<BusquedaAccidentesModel> ListaAccidentes = new List<BusquedaAccidentesModel>();

            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
                try

                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT a.* ,mun.municipio,car.carretera,tra.tramo,er.estatusReporte,cla.nombreClasificacion,fa.factorAccidente, " +
                        "foa.factorOpcionAccidente,ofi1.nombre AS nombreElabora,ofi1.apellidoPaterno AS apellidoPaternoElabora,ofi1.apellidoMaterno AS apellidoMaternoElabora, " +
                        "ofi2.nombre AS nombreSuperviza,ofi2.apellidoPaterno AS apellidoPaternoSuperviza,ofi2.apellidoMaterno As apellidoMaternoSuperviza, " +
                        "ofi3.nombre AS nombreAutoriza,ofi3.apellidoPaterno AS apellidoPaternoAutoriza,ofi3.apellidoMaterno AS apellidoMaternoAutoriza, " +
                        "ofi4.nombre AS nombreElaboraCons,ofi4.apellidoPaterno AS apellidoPaternoElaboraCons,ofi4.apellidoMaterno AS apellidoMaternoElaboraCons, " +
                        "ent.nombreEntidad AS ciudad, hos.nombreHospital AS certificadoPor,agm.nombreAgencia,aute.autoridadEntrega,autd.nombreAutoridadDisposicion, " +
                        "cva.idPersona AS idConductor,vacc.placa,vacc.serie,v.idPersona AS Propietario,p1.nombre AS nombrePropietario, p1.apellidoPaterno AS apellidoPaternoPropietario, p1.apellidoMaterno as apellidoMaternoPropietario, " +
                        "p2.nombre AS nombreConductor, p2.apellidoPaterno AS apellidoPaternoConductor, p2.apellidoMaterno AS apellidoMaternoConductor " +
                        "FROM accidentes AS a " +
                        "JOIN vehiculosAccidente AS vacc ON a.idAccidente = vacc.idAccidente " +
                        "JOIN vehiculos AS v ON vacc.idVehiculo = v.idVehiculo " +
                        "JOIN personas AS p1 ON p1.idPersona = v.idPersona " +
                        "JOIN conductoresVehiculosAccidente AS cva ON a.idAccidente = cva.idAccidente " +
                        "JOIN personas AS p2 ON cva.idPersona = p2.idPersona " +
                        "LEFT JOIN catMunicipios AS mun ON a.idMunicipio = mun.idMunicipio " +
                        "LEFT JOIN catCarreteras AS car ON a.idCarretera = car.idCarretera " +
                        "LEFT JOIN catTramos AS tra ON a.idTramo = tra.idTramo " +
                        "LEFT JOIN catEstatusReporteAccidente AS er ON a.idEstatusReporte = er.idEstatusReporte " +
                        "LEFT JOIN catClasificacionAccidentes AS cla ON a.idClasificacionAccidente = cla.idClasificacionAccidente " +
                        "LEFT JOIN catFactoresAccidentes AS fa ON a.idFactorAccidente = fa.idFactorAccidente " +
                        "LEFT JOIN catFactoresOpcionesAccidentes AS foa ON a.idFactorOpcionAccidente = foa.idFactorOpcionAccidente " +
                        "LEFT JOIN catOficiales AS ofi1 ON a.idElabora = ofi1.idOficial " +
                        "LEFT JOIN catOficiales AS ofi2 ON a.idSupervisa = ofi2.idOficial "  +
                        "LEFT JOIN catOficiales AS ofi3 ON a.idAutoriza = ofi3.idOficial " +
                        "LEFT JOIN catOficiales AS ofi4 ON a.idElaboraConsignacion = ofi4.idOficial " +
                        "LEFT JOIN catEntidades AS ent ON a.idCiudad = ent.idEntidad " +
                        "LEFT JOIN catHospitales AS hos ON a.idCertificado = hos.idHospital " +
                        "LEFT JOIN catAgenciasMinisterio AS agm ON a.idAgenciaMinisterio = agm.idAgenciaMinisterio " +
                        "LEFT JOIN catAutoridadesEntrega AS aute ON a.idAutoridadEntrega = aute.idAutoridadEntrega " +
                        "LEFT JOIN catAutoridadesDisposicion AS autd ON a.idAutoridadDisposicion = autd.idAutoridadDisposicion " +
                        "WHERE a.fecha BETWEEN @fechaInicio AND @fechaFin " +
                        "OR UPPER(a.numeroReporte) = @oficioBusqueda OR a.idAgenciaMinisterio = @idDelegacionBusqueda OR a.idAutoriza = @idOficialBusqueda " +
                        "OR a.idSupervisa = @idOficialBusqueda OR a.idAutoriza = @idOficialBusqueda OR a.idCarretera = @idCarreteraBusqueda OR a.idTramo = @idTramoBusqueda " +
                        "OR p1.nombre LIKE '%' + @propietarioBusqueda + '%' OR UPPER(p1.apellidoPaterno) LIKE '%' + @propietarioBusqueda + '%' OR UPPER(p1.apellidoMaterno) LIKE '%' + @propietarioBusqueda + '%' " +
                        "OR p2.nombre LIKE '%' + @conductorBusqueda + '%' OR UPPER(p2.apellidoPaterno) LIKE '%' + @conductorBusqueda + '%' OR UPPER(p2.apellidoMaterno) LIKE '%' + @conductorBusqueda + '%' " +
                        "OR vacc.placa LIKE '%' + @placasBusqueda + '%' OR vacc.serie LIKE '%' + @serieBusqueda + '%';", connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@fechaInicio", model.FechaInicio);
                    command.Parameters.AddWithValue("@fechaFin", model.FechaFin);
                    command.Parameters.AddWithValue("@oficioBusqueda", model.folioBusqueda);
                    command.Parameters.AddWithValue("@idDelegacionBusqueda", model.IdDelegacionBusqueda);
                    command.Parameters.AddWithValue("@idOficialBusqueda", model.IdOficialBusqueda);
                    command.Parameters.AddWithValue("@idCarreteraBusqueda", model.IdCarreteraBusqueda);
                    command.Parameters.AddWithValue("@idTramoBusqueda", model.IdTramoBusqueda);
                    command.Parameters.AddWithValue("@propietarioBusqueda", model.propietarioBusqueda);
                    command.Parameters.AddWithValue("@propietarioBusqueda", model.propietarioBusqueda);
                    command.Parameters.AddWithValue("@propietarioBusqueda", model.propietarioBusqueda);
                    command.Parameters.AddWithValue("@conductorBusqueda", model.conductorBusqueda);
                    command.Parameters.AddWithValue("@placasBusqueda", model.placasBusqueda);
                    command.Parameters.AddWithValue("@serieBusqueda", model.serieBusqueda);



                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            BusquedaAccidentesModel accidente = new BusquedaAccidentesModel();
                            accidente.IdAccidente = Convert.IsDBNull(reader["idAccidente"]) ? 0 : Convert.ToInt32(reader["idAccidente"]);
                            accidente.idMunicipio = Convert.IsDBNull(reader["idMunicipio"]) ? 0 : Convert.ToInt32(reader["idMunicipio"]);
                            accidente.idCarretera = Convert.IsDBNull(reader["idCarretera"]) ? 0 : Convert.ToInt32(reader["idCarretera"]);
                            accidente.idTramo = Convert.IsDBNull(reader["idTramo"]) ? 0 : Convert.ToInt32(reader["idTramo"]);
                            accidente.estatusReporte = Convert.IsDBNull(reader["estatusReporte"]) ? 0 : Convert.ToInt32(reader["estatusReporte"]);
                            accidente.idVehiculo = Convert.IsDBNull(reader["idVehiculo"]) ? 0 : Convert.ToInt32(reader["idVehiculo"]);
                            accidente.idPersona = Convert.IsDBNull(reader["idPersona"]) ? 0 : Convert.ToInt32(reader["idPersona"]);
                            accidente.idClasificacionAccidente = Convert.IsDBNull(reader["idClasificacionAccidente"]) ? 0 : Convert.ToInt32(reader["idClasificacionAccidente"]);
                            accidente.idCausaAccidente = Convert.IsDBNull(reader["idCausaAccidente"]) ? 0 : Convert.ToInt32(reader["idCausaAccidente"]);
                            accidente.idFactorAccidente = Convert.IsDBNull(reader["idFactorAccidente"]) ? 0 : Convert.ToInt32(reader["idFactorAccidente"]);
                            accidente.idFactorOpcionAccidente = Convert.IsDBNull(reader["idFactorOpcionAccidente"]) ? 0 : Convert.ToInt32(reader["idFactorOpcionAccidente"]);
                            accidente.montoCamino = reader["montoCamino"] != DBNull.Value ? Convert.ToSingle(reader["montoCamino"]) : 0.0f;
                            accidente.montoPropietarios = reader["montoPropietarios"] != DBNull.Value ? Convert.ToSingle(reader["montoPropietarios"]) : 0.0f;
                            accidente.montoOtros = reader["montoOtros"] != DBNull.Value ? Convert.ToSingle(reader["montoOtros"]) : 0.0f;
                            accidente.montoVehiculo = reader["montoVehiculo"] != DBNull.Value ? Convert.ToSingle(reader["montoVehiculo"]) : 0.0f;
                            accidente.idElabora = Convert.IsDBNull(reader["idElabora"]) ? 0 : Convert.ToInt32(reader["idElabora"]);
                            accidente.idSupervisa = Convert.IsDBNull(reader["idSupervisa"]) ? 0 : Convert.ToInt32(reader["idSupervisa"]);
                            accidente.idAutoriza = Convert.IsDBNull(reader["idAutoriza"]) ? 0 : Convert.ToInt32(reader["idAutoriza"]);
                            accidente.idElaboraConsignacion = Convert.IsDBNull(reader["idElaboraConsignacion"]) ? 0 : Convert.ToInt32(reader["idElaboraConsignacion"]);
                            accidente.idCiudad = Convert.IsDBNull(reader["idCiudad"]) ? 0 : Convert.ToInt32(reader["idCiudad"]);
                            accidente.idAgenciaMinisterio = Convert.IsDBNull(reader["idAgenciaMinisterio"]) ? 0 : Convert.ToInt32(reader["idAgenciaMinisterio"]);
                            accidente.idAutoridadEntrega = Convert.IsDBNull(reader["idAutoridadEntrega"]) ? 0 : Convert.ToInt32(reader["idAutoridadEntrega"]);
                            accidente.idAutoridadDisposicion = Convert.IsDBNull(reader["idAutoridadDisposicion"]) ? 0 : Convert.ToInt32(reader["idAutoridadDisposicion"]);
                            accidente.armas = Convert.IsDBNull(reader["armas"]) ? 0 : Convert.ToInt32(reader["armas"]);
                            accidente.drogas = Convert.IsDBNull(reader["drogas"]) ? 0 : Convert.ToInt32(reader["drogas"]);
                            accidente.valores = Convert.IsDBNull(reader["valores"]) ? 0 : Convert.ToInt32(reader["valores"]);
                            accidente.prendas = Convert.IsDBNull(reader["prendas"]) ? 0 : Convert.ToInt32(reader["prendas"]);
                            accidente.otros = Convert.IsDBNull(reader["otros"]) ? 0 : Convert.ToInt32(reader["otros"]);
                            accidente.idEstatusReporte = Convert.IsDBNull(reader["idEstatusReporte"]) ? 0 : Convert.ToInt32(reader["idEstatusReporte"]);
                            accidente.idConductor = Convert.IsDBNull(reader["idConductor"]) ? 0 : Convert.ToInt32(reader["idConductor"]);
                            accidente.Propietario = Convert.IsDBNull(reader["Propietario"]) ? 0 : Convert.ToInt32(reader["Propietario"]);

                            accidente.latitud = reader["latitud"] != DBNull.Value ? Convert.ToSingle(reader["latitud"]) : 0.0f;
                            accidente.longitud = reader["longitud"] != DBNull.Value ? Convert.ToSingle(reader["longitud"]) : 0.0f;
                            accidente.idCertificado = Convert.IsDBNull(reader["idCertificado"]) ? 0 : Convert.ToInt32(reader["idCertificado"]);
                           
                            accidente.kilometro = reader["kilometro"].ToString();
                            accidente.descripcionAccidente = reader["descripcionAccidente"].ToString();
                            accidente.numeroReporte = reader["numeroReporte"].ToString();
                            accidente.fecha = reader["fecha"] != DBNull.Value ? Convert.ToDateTime(reader["fecha"]) : DateTime.MinValue;
                            accidente.hora = reader["hora"] != DBNull.Value ? TimeSpan.Parse(reader["hora"].ToString()) : TimeSpan.MinValue;
                            accidente.entregaOtros = reader["entregaOtros"].ToString();
                            accidente.entregaObjetos = reader["entregaObjetos"].ToString();
                            accidente.consignacionHechos = reader["consignacionHechos"].ToString();
                            accidente.numeroOficio = reader["numeroOficio"].ToString();
                            accidente.recibeMinisterio = reader["recibeMinisterio"].ToString();
                            accidente.nombreClasificacion = reader["nombreClasificacion"].ToString();
                            accidente.factorAccidente = reader["factorAccidente"].ToString();
                            accidente.factorOpcionAccidente = reader["factorOpcionAccidente"].ToString();
                            accidente.nombreElabora = reader["nombreElabora"].ToString();
                            accidente.apellidoPaternoElabora = reader["apellidoPaternoElabora"].ToString();
                            accidente.apellidoMaternoElabora = reader["apellidoMaternoElabora"].ToString();
                            accidente.nombreSupervisa = reader["nombreSuperviza"].ToString();
                            accidente.apellidoPaternoSupervisa = reader["apellidoPaternoSuperviza"].ToString();
                            accidente.apellidoMaternoSupervisa = reader["apellidoMaternoSuperviza"].ToString();
                            accidente.nombreAutoriza = reader["nombreAutoriza"].ToString();
                            accidente.apellidoPaternoAutoriza = reader["apellidoPaternoAutoriza"].ToString();
                            accidente.apellidoMaternoAutoriza = reader["apellidoMaternoAutoriza"].ToString();
                            accidente.nombreElaboraCons = reader["nombreElaboraCons"].ToString();
                            accidente.apellidoPaternoElaboraCons = reader["apellidoPaternoElaboraCons"].ToString();
                            accidente.apellidoMaternoElaboraCons = reader["apellidoMaternoElaboraCons"].ToString();
                            accidente.ciudad = reader["ciudad"].ToString();
                            accidente.certificadoPor = reader["certificadoPor"].ToString();
                            accidente.nombreAgencia = reader["nombreAgencia"].ToString();
                            accidente.autoridadEntrega = reader["autoridadEntrega"].ToString();
                            accidente.nombreAutoridadDisposicion = reader["nombreAutoridadDisposicion"].ToString();
                            accidente.placa = reader["placa"].ToString();
                            accidente.serie = reader["serie"].ToString();
                            accidente.nombrePropietario = reader["nombrePropietario"].ToString();
                            accidente.apellidoPaternoPropietario = reader["apellidoPaternoPropietario"].ToString();
                            accidente.apellidoMaternoPropietario = reader["apellidoMaternoPropietario"].ToString();
                            accidente.nombreConductor = reader["nombreConductor"].ToString();
                            accidente.apellidoPaternoConductor = reader["apellidoPaternoConductor"].ToString();
                            accidente.apellidoMaternoConductor = reader["apellidoMaternoConductor"].ToString();



                            ListaAccidentes.Add(accidente);

                        }

                    }

                }
                catch (SqlException ex)
                {

                }
                finally
                {
                    connection.Close();
                }
            return ListaAccidentes;


        }
    }
}
