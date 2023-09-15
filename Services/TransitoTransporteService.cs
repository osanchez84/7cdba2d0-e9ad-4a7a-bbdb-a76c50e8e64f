using GuanajuatoAdminUsuarios.Entity;
using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GuanajuatoAdminUsuarios.Services
{
    public class TransitoTransporteService : ITransitoTransporteService
    {
        private readonly ISqlClientConnectionBD _sqlClientConnectionBD;
        public TransitoTransporteService(ISqlClientConnectionBD sqlClientConnectionBD)
        {
            _sqlClientConnectionBD = sqlClientConnectionBD;
        }

        public List<TransitoTransporteModel> GetAllTransitoTransporte()
        {
            #region Consulta base
            //var query = @"select top(100) d.IdDeposito,d.IdSolicitud,d.IdDelegacion,d.IdMarca,d.IdSubmarca,d.IdPension,d.IdTramo,
            //            d.IdColor,d.Serie,d.Placa,d.FechaIngreso,d.Folio,d.Km,d.Liberado,d.Autoriza,d.FechaActualizacion,
            //            del.delegacion, d.ActualizadoPor, d.estatus, m.marcaVehiculo,subm.nombreSubmarca,sol.solicitanteNombre,
            //            sol.solicitanteAp,sol.solicitanteAm, col.color,pen.pension,cTra.tramo,
            //            --esto es nuevo
            //            sol.fechaSolicitud,sol.folio, dep.IdDependencia,dep.nombreDependencia,inf.IdInfraccion,inf.FolioInfraccion,
            //            veh.idVehiculo,veh.propietario,veh.numeroEconomico,
            //            g.IdGrua,g.Grua

            //            from depositos d inner join catDelegaciones del on d.idDelegacion= del.idDelegacion
            //            inner join marcasVehiculos m on d.idMarca=m.idMarcaVehiculo
            //            inner join colores col on d.idColor = col.idColor
            //            inner join pensiones pen on d.idPension	= pen.idPension
            //            inner join catTramos cTra  on d.Idtramo=cTra.idTramo
            //            inner join catSubmarcasVehiculos  subm on m.idMarcaVehiculo=subm.idMarcaVehiculo
            //            inner join solicitudes sol on d.idSolicitud = sol.idSolicitud
            //            inner join dependencias dep on sol.IdDependencia = dep.IdDependencia
            //            inner join Infracciones inf on sol.idInfraccion = inf.IdInfraccion
            //            inner join	vehiculos  veh on sol.idVehiculo =veh.idVehiculo 
            //            left join 	GruasDepositos gd on  d.IdDeposito=	 gd.IdDeposito
            //            left join Gruas g on g.IdGrua= gd.IdGrua
            //            where 
            //            --d.liberado=0 and d.estatus=1	and
            //            (d.IdDeposito=@IdDeposito  OR d.IdMarca=@IdMarca 
            //            OR d.Serie LIKE '%' + @Serie + '%' OR d.FechaIngreso =@FechaIngreso 
            //            OR d.Folio LIKE '%' + @Folio + '%')";
            #endregion

            List<TransitoTransporteModel> transitoList = new List<TransitoTransporteModel>();
            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
                try
                {
                    connection.Open();
                    const string SqlTransact =
                                        @"select d.iddeposito,d.idsolicitud,d.idDelegacion,d.idmarca,d.idsubmarca,d.idpension,d.idtramo,
                                        d.idcolor,d.serie,d.placa,d.fechaingreso,d.folio,d.km,d.liberado,d.autoriza,d.fechaactualizacion,
                                        del.delegacion, d.actualizadopor, d.estatus, m.marcavehiculo,subm.nombresubmarca,sol.solicitantenombre,
                                        sol.solicitanteap,sol.solicitanteam, col.color,pen.pension,ctra.tramo,                       
                                        sol.fechasolicitud, sol.folio as FolioSolicitud, inf.idinfraccion,inf.folioinfraccion,
                                        veh.idvehiculo,veh.propietario,veh.numeroeconomico,veh.modelo,
                                        con.IdConcesionario, con.concesionario,d.FechaLiberacion
                                        ,d.IdDependenciaGenera,d.IdDependenciaTransito,d.IdDependenciaNoTransito
                                        ,dep.idDependencia,dep.nombreDependencia
                                        from depositos d left join catDelegaciones del on d.idDelegacion= del.idDelegacion
                                        left join catMarcasVehiculos m on d.idMarca=m.idMarcaVehiculo
                                        left join catColores col on d.idcolor = col.idcolor
                                        left join pensiones pen on d.idpension	= pen.idpension
                                        left join catTramos ctra  on d.idtramo=ctra.idtramo
                                        left join catSubmarcasVehiculos  subm on d.idSubmarca=subm.idSubmarca
                                        left join solicitudes sol on d.idsolicitud = sol.idsolicitud
                                        left join infracciones inf on sol.idinfraccion = inf.idinfraccion
                                        left join vehiculos  veh on sol.idvehiculo =veh.idvehiculo 
                                        left join concesionarios con on con.IdConcesionario =d.IdConcesionario
                                        left join catDependencias dep on ((dep.idDependencia=d.IdDependenciaTransito)OR (dep.idDependencia=d.IdDependenciaNoTransito))
                                        where  sol.estatus !=0 and d.estatus!=0";

                    SqlCommand command = new SqlCommand(SqlTransact, connection);
                    command.CommandType = CommandType.Text;
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            TransitoTransporteModel transito = new TransitoTransporteModel();
                            transito.IdDeposito = Convert.IsDBNull(reader["IdDeposito"]) ? 0 : Convert.ToInt32(reader["IdDeposito"]);
                            transito.IdSolicitud = Convert.IsDBNull(reader["IdSolicitud"]) ? 0 : Convert.ToInt32(reader["IdSolicitud"]);
                            transito.IdDelegacion = Convert.IsDBNull(reader["IdDelegacion"]) ? 0 : Convert.ToInt32(reader["IdDelegacion"]);
                            transito.IdMarca = Convert.IsDBNull(reader["IdMarca"]) ? 0 : Convert.ToInt32(reader["IdMarca"]);
                            transito.IdSubmarca = Convert.IsDBNull(reader["IdSubmarca"]) ? 0 : Convert.ToInt32(reader["IdSubmarca"]);
                            transito.IdPension = Convert.IsDBNull(reader["IdPension"]) ? 0 : Convert.ToInt32(reader["IdPension"]);
                            transito.IdTramo = Convert.IsDBNull(reader["IdTramo"]) ? 0 : Convert.ToInt32(reader["IdTramo"]);
                            transito.IdColor = Convert.IsDBNull(reader["IdColor"]) ? 0 : Convert.ToInt32(reader["IdColor"]);
                            transito.Serie = reader["Serie"].ToString();
                            transito.Placa = reader["Placa"].ToString();
                            transito.FechaIngreso = Convert.IsDBNull(reader["FechaIngreso"]) ? DateTime.MinValue : Convert.ToDateTime(reader["FechaIngreso"]);
                            transito.FechaLiberacion = Convert.IsDBNull(reader["FechaLiberacion"]) ? DateTime.MinValue : Convert.ToDateTime(reader["FechaLiberacion"]);
                            transito.Folio = reader["Folio"].ToString();
                            transito.Km = reader["Km"].ToString();
                            transito.Liberado = Convert.IsDBNull(reader["Liberado"]) ? 0 : Convert.ToInt32(reader["Liberado"]);
                            transito.Autoriza = reader["Autoriza"].ToString();
                            transito.FechaActualizacion = Convert.IsDBNull(reader["FechaActualizacion"]) ? DateTime.MinValue : Convert.ToDateTime(reader["FechaActualizacion"]);
                            transito.ActualizadoPor = Convert.IsDBNull(reader["ActualizadoPor"]) ? 0 : Convert.ToInt32(reader["ActualizadoPor"]);
                            transito.DepositoEstatus = Convert.IsDBNull(reader["Estatus"]) ? 0 : Convert.ToInt32(reader["Estatus"]);
                            transito.marcaVehiculo = reader["marcaVehiculo"].ToString();
                            transito.nombreSubmarca = reader["nombreSubmarca"].ToString();
                            transito.delegacion = reader["delegacion"].ToString();
                            transito.modelo = reader["modelo"].ToString();
                            transito.solicitanteNombre = reader["solicitanteNombre"].ToString();
                            transito.solicitanteAp = reader["solicitanteAp"].ToString();
                            transito.solicitanteAm = reader["solicitanteAm"].ToString();
                            transito.Color = reader["Color"].ToString();
                            transito.pension = reader["pension"].ToString();
                            transito.tramo = reader["tramo"].ToString();

                            //nuevos
                            transito.FechaSolicitud = Convert.IsDBNull(reader["FechaSolicitud"]) ? DateTime.MinValue : Convert.ToDateTime(reader["FechaSolicitud"]);
                            transito.IdDependencia = Convert.IsDBNull(reader["IdDependencia"]) ? 0 : Convert.ToInt32(reader["IdDependencia"]);
                            transito.NombreDependencia = reader["NombreDependencia"].ToString();
                            transito.IdInfraccion = Convert.IsDBNull(reader["IdInfraccion"]) ? 0 : Convert.ToInt32(reader["IdInfraccion"]);
                            transito.FolioInfraccion = reader["folioInfraccion"].ToString();
                            transito.IdVehiculo = Convert.IsDBNull(reader["IdVehiculo"]) ? 0 : Convert.ToInt32(reader["IdVehiculo"]);
                            transito.propietario = reader["propietario"].ToString();
                            transito.numeroEconomico = reader["numeroEconomico"].ToString();
                            transito.FolioSolicitud = reader["FolioSolicitud"].ToString();
                            transito.IdConcesionario = Convert.IsDBNull(reader["IdConcesionario"]) ? 0 : Convert.ToInt32(reader["IdConcesionario"]);
                            transito.Concesionario = reader["concesionario"].ToString();
                            transito.IdDependenciaGenera = Convert.IsDBNull(reader["IdDependenciaGenera"]) ? 0 : (int)reader["IdDependenciaGenera"];
                            transito.IdDependenciaTransito = Convert.IsDBNull(reader["IdDependenciaTransito"]) ? 0 : (int)reader["IdDependenciaTransito"];
                            transito.IdDependenciaNoTransito = Convert.IsDBNull(reader["IdDependenciaNoTransito"]) ? 0 : (int)reader["IdDependenciaNoTransito"];
                            //sqlreader[indexAge] as int? ?? default(int)
                            transitoList.Add(transito);

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
            return transitoList;
        }

        public List<TransitoTransporteModel> GetTransitoTransportes(TransitoTransporteBusquedaModel model)
        {
            List<TransitoTransporteModel> transitoList = new List<TransitoTransporteModel>();
            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
                try
                {
                    connection.Open();
                    #region QueryBase
                    //const string SqlTransact =
                    //    @"select d.iddeposito,d.idsolicitud,d.iddelegacion,d.idmarca,d.idsubmarca,d.idpension,d.idtramo,
                    //        d.idcolor,d.serie,d.placa,d.fechaingreso,d.folio,d.km,d.liberado,d.autoriza,d.fechaactualizacion,
                    //        del.delegacion, d.actualizadopor, d.estatus, m.marcavehiculo,subm.nombresubmarca,sol.solicitantenombre,
                    //        sol.solicitanteap,sol.solicitanteam, col.color,pen.pension,ctra.tramo,                       
                    //        sol.fechasolicitud, sol.folio as FolioSolicitud, inf.idinfraccion,inf.folioinfraccion,
                    //        veh.idvehiculo,veh.propietario,veh.numeroeconomico,veh.modelo,
                    //        con.IdConcesionario, con.concesionario,d.FechaLiberacion
                    //        ,d.IdDependenciaGenera,d.IdDependenciaTransito,d.IdDependenciaNoTransito
                    //        ,dep.idDependencia,dep.nombreDependencia
                    //        from depositos d inner join catDelegaciones del on d.iddelegacion= del.iddelegacion
                    //        inner join catMarcasVehiculos m on d.idmarca=m.idmarcavehiculo
                    //        inner join colores col on d.idcolor = col.idcolor
                    //        inner join pensiones pen on d.idpension	= pen.idpension
                    //        inner join catTramos ctra  on d.idtramo=ctra.idtramo
                    //        inner join catSubmarcasVehiculos  subm on m.idmarcavehiculo=subm.idmarcavehiculo
                    //        inner join solicitudes sol on d.idsolicitud = sol.idsolicitud
                    //        inner join infracciones inf on sol.idinfraccion = inf.idinfraccion
                    //        inner join	vehiculos  veh on sol.idvehiculo =veh.idvehiculo 
                    //        inner join Concesionarios con on con.IdConcesionario =d.IdConcesionario
                    //        left join dependencias dep on ((dep.idDependencia=d.IdDependenciaTransito) OR (dep.idDependencia=d.IdDependenciaNoTransito))
                    //        where  sol.estatus !=0 and d.estatus!=0
                    //        and
                    //        (d.placa LIKE '%' + @Placa + '%'  OR sol.folio LIKE '%' + @FolioSolicitud + '%'  
                    //        OR inf.FolioInfraccion LIKE '%' + @FolioInfraccion + '%' OR veh.propietario LIKE '%' + @Propietario + '%'
                    //        OR veh.numeroEconomico LIKE '%' + @numeroEconomico + '%' OR del.idDelegacion=@IdDelegacion
                    //        OR pen.idpension=@IdPension	OR d.IdDependenciaGenera=@IdDependenciaGenera OR d.IdDependenciaTransito=@IdDependenciaTransito 
                    //        OR  d.IdDependenciaNoTransito=@IdDependenciaNoTransito OR d.fechaIngreso between @FechaIngreso and  @FechaIngresoFin)";
                    #endregion

                    const string SqlTransact =
                                @"SELECT d.iddeposito, d.idsolicitud, d.idDelegacion, d.idmarca, d.idsubmarca, d.idpension, d.idtramo,
                                         d.idcolor, d.serie, d.placa, d.fechaingreso, d.folio, d.km, d.liberado, d.autoriza, d.fechaactualizacion,
                                         del.delegacion, d.actualizadopor, d.estatus, m.marcavehiculo, subm.nombresubmarca, sol.solicitantenombre,
                                         sol.solicitanteap, sol.solicitanteam, col.color, pen.pension, ctra.tramo,                       
                                         sol.fechasolicitud, sol.folio AS FolioSolicitud, inf.idinfraccion, inf.folioinfraccion,
                                         veh.idvehiculo, veh.propietario, veh.numeroeconomico, veh.modelo,
                                         con.IdConcesionario, con.concesionario, d.FechaLiberacion,
                                         d.IdDependenciaGenera, d.IdDependenciaTransito, d.IdDependenciaNoTransito,
                                         dep.idDependencia, dep.nombreDependencia
                                FROM depositos d
                                LEFT JOIN catDelegaciones del ON d.idDelegacion = del.idDelegacion
                                LEFT JOIN catMarcasVehiculos m ON d.idMarca = m.idMarcaVehiculo
                                LEFT JOIN catColores col ON d.idcolor = col.idcolor
                                LEFT JOIN pensiones pen ON d.idpension = pen.idpension
                                LEFT JOIN catTramos ctra ON d.idtramo = ctra.idtramo
                                LEFT JOIN catSubmarcasVehiculos subm ON d.idSubmarca = subm.idSubmarca
                                LEFT JOIN solicitudes sol ON d.idsolicitud = sol.idsolicitud
                                LEFT JOIN infracciones inf ON sol.idinfraccion = inf.idinfraccion
                                LEFT JOIN vehiculos veh ON sol.idvehiculo = veh.idvehiculo 
                                LEFT JOIN concesionarios con ON con.IdConcesionario = d.IdConcesionario
                                LEFT JOIN catDependencias dep ON (dep.idDependencia = d.IdDependenciaTransito OR dep.idDependencia = d.IdDependenciaNoTransito)
                                WHERE d.estatus != 0
                                AND (d.placa LIKE '%' + @Placa + '%' OR sol.folio LIKE '%' + @FolioSolicitud + '%'  
                                OR inf.folioInfraccion LIKE '%' + @FolioInfraccion + '%' OR veh.propietario LIKE '%' + @Propietario + '%'
                                OR veh.numeroEconomico LIKE '%' + @numeroEconomico + '%' OR del.idDelegacion = @IdDelegacion
                                OR pen.idpension = @IdPension OR d.IdDependenciaGenera = @IdDependenciaGenera 
                                OR d.IdDependenciaTransito = @IdDependenciaTransito OR d.IdDependenciaNoTransito = @IdDependenciaNoTransito 
                                OR (@FechaIngreso IS NULL OR d.fechaIngreso BETWEEN @FechaIngreso AND @FechaIngresoFin)
                                OR 1 = CASE 
                                            WHEN @Estatus = 2 AND d.liberado = 0 THEN 1
                                            WHEN @Estatus = 3 AND d.liberado = 1 THEN 1
                                        END)";

                    SqlCommand command = new SqlCommand(SqlTransact, connection);

                    command.Parameters.Add(new SqlParameter("@Placa", SqlDbType.NVarChar)).Value = (object)model.Placas ?? DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@FolioSolicitud", SqlDbType.NVarChar)).Value = (object)model.FolioSolicitud ?? DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@folioInfraccion", SqlDbType.NVarChar)).Value = (object)model.FolioInfraccion ?? DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@Propietario", SqlDbType.NVarChar)).Value = (object)model.Propietario ?? DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@numeroEconomico", SqlDbType.NVarChar)).Value = (object)model.NumeroEconomico ?? DBNull.Value;

                    command.Parameters.Add(new SqlParameter("@IdDelegacion", SqlDbType.Int)).Value = (object)model.IdDelegacion ?? DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@IdPension", SqlDbType.Int)).Value = (object)model.IdPension ?? DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@IdDependenciaGenera", SqlDbType.Int)).Value = (object)model.IdDependenciaGenera ?? DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@IdDependenciaTransito", SqlDbType.Int)).Value = (object)model.IdDependenciaTransito ?? DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@IdDependenciaNoTransito", SqlDbType.Int)).Value = (object)model.IdDependenciaNoTransito ?? DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@Estatus", SqlDbType.Int)).Value = (object)model.IdEstatus ?? DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@FechaIngreso", SqlDbType.DateTime)).Value = (model.FechaIngreso == DateTime.MinValue) ? DBNull.Value : (object)model.FechaIngreso;
                    command.Parameters.Add(new SqlParameter("@FechaIngresoFin", SqlDbType.DateTime)).Value = (model.FechaIngresoFin == DateTime.MinValue) ? DBNull.Value : (object)model.FechaIngresoFin;

                    command.CommandType = CommandType.Text;
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            TransitoTransporteModel transito = new TransitoTransporteModel();
                            transito.IdDeposito = Convert.ToInt32(reader["IdDeposito"] is DBNull ? 0 : reader["IdDeposito"]);
                            transito.IdSolicitud = Convert.ToInt32(reader["IdSolicitud"] is DBNull ? 0 : reader["IdSolicitud"]);
                            transito.IdDelegacion = Convert.ToInt32(reader["IdDelegacion"] is DBNull ? 0 : reader["IdDelegacion"]);
                            transito.IdMarca = Convert.ToInt32(reader["IdMarca"] is DBNull ? 0 : reader["IdMarca"]);
                            transito.IdSubmarca = Convert.ToInt32(reader["IdSubmarca"] is DBNull ? 0 : reader["IdSubmarca"]);
                            transito.IdPension = Convert.ToInt32(reader["IdPension"] is DBNull ? 0 : reader["IdPension"]);
                            transito.IdTramo = Convert.ToInt32(reader["IdTramo"] is DBNull ? 0 : reader["IdTramo"]);
                            transito.IdColor = Convert.ToInt32(reader["IdColor"] is DBNull ? 0 : reader["IdColor"]);
                            transito.Serie = reader["Serie"].ToString();
                            transito.Placa = reader["Placa"].ToString();
                            transito.FechaIngreso = Convert.ToDateTime(reader["FechaIngreso"] is DBNull ? DateTime.MinValue : reader["FechaIngreso"]);
                            transito.FechaLiberacion = Convert.ToDateTime(reader["FechaLiberacion"] is DBNull ? DateTime.MinValue : reader["FechaLiberacion"]);
                            transito.Folio = reader["Folio"].ToString();
                            transito.Km = reader["Km"].ToString();
                            transito.Liberado = Convert.ToInt32(reader["Liberado"] is DBNull ? 0 : reader["Liberado"]);
                            transito.Autoriza = reader["Autoriza"].ToString();
                            transito.FechaActualizacion = Convert.ToDateTime(reader["FechaActualizacion"] is DBNull ? DateTime.MinValue : reader["FechaActualizacion"]);
                            transito.ActualizadoPor = Convert.ToInt32(reader["ActualizadoPor"] is DBNull ? 0 : reader["ActualizadoPor"]);
                            transito.DepositoEstatus = Convert.ToInt32(reader["Estatus"] is DBNull ? 0 : reader["Estatus"]);
                            transito.marcaVehiculo = reader["marcaVehiculo"].ToString();
                            transito.nombreSubmarca = reader["nombreSubmarca"].ToString();
                            transito.delegacion = reader["delegacion"].ToString();
                            transito.modelo = reader["modelo"].ToString();
                            transito.solicitanteNombre = reader["solicitanteNombre"].ToString();
                            transito.solicitanteAp = reader["solicitanteAp"].ToString();
                            transito.solicitanteAm = reader["solicitanteAm"].ToString();
                            transito.Color = reader["Color"].ToString();
                            transito.pension = reader["pension"].ToString();
                            transito.tramo = reader["tramo"].ToString();

                            // Nuevos
                            transito.FechaSolicitud = Convert.ToDateTime(reader["FechaSolicitud"] is DBNull ? DateTime.MinValue : reader["FechaSolicitud"]);
                            transito.IdDependencia = Convert.ToInt32(reader["IdDependencia"] is DBNull ? 0 : reader["IdDependencia"]);
                            transito.NombreDependencia = reader["NombreDependencia"].ToString();
                            transito.IdInfraccion = Convert.ToInt32(reader["IdInfraccion"] is DBNull ? 0 : reader["IdInfraccion"]);
                            transito.FolioInfraccion = reader["folioInfraccion"].ToString();
                            transito.IdVehiculo = Convert.ToInt32(reader["IdVehiculo"] is DBNull ? 0 : reader["IdVehiculo"]);
                            transito.propietario = reader["propietario"].ToString();
                            transito.numeroEconomico = reader["numeroEconomico"].ToString();
                            transito.FolioSolicitud = reader["FolioSolicitud"].ToString();
                            transito.IdConcesionario = Convert.ToInt32(reader["IdConcesionario"] is DBNull ? 0 : reader["IdConcesionario"]);
                            transito.Concesionario = reader["concesionario"].ToString();
                            transito.IdDependenciaGenera = reader["IdDependenciaGenera"] is DBNull ? 0 : (int)reader["IdDependenciaGenera"];
                            transito.IdDependenciaTransito = reader["IdDependenciaTransito"] is DBNull ? 0 : (int)reader["IdDependenciaTransito"];
                            transito.IdDependenciaNoTransito = reader["IdDependenciaNoTransito"] is DBNull ? 0 : (int)reader["IdDependenciaNoTransito"];

                            transitoList.Add(transito);

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
            return transitoList;
        }

        public TransitoTransporteModel GetTransitoTransporteById(int IdDeposito)
        {
         TransitoTransporteModel transito = new TransitoTransporteModel();
            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
                try
                {
                    connection.Open();
                 

                    const string SqlTransact =
                                       @"select d.iddeposito,d.idsolicitud,d.idDelegacion,d.idmarca,d.idsubmarca,d.idpension,d.idtramo,
                                        d.idcolor,d.serie,d.placa,d.fechaingreso,d.folio,d.km,d.liberado,d.autoriza,d.fechaactualizacion,
                                        del.delegacion, d.actualizadopor, d.estatus, m.marcavehiculo,subm.nombresubmarca,sol.solicitantenombre,
                                        sol.solicitanteap,sol.solicitanteam, col.color,pen.pension,ctra.tramo,                       
                                        sol.fechasolicitud, sol.folio as FolioSolicitud, inf.idinfraccion,inf.folioinfraccion,
                                        veh.idvehiculo,veh.propietario,veh.numeroeconomico,veh.modelo,
                                        con.IdConcesionario, con.concesionario,d.FechaLiberacion
                                        ,d.IdDependenciaGenera,d.IdDependenciaTransito,d.IdDependenciaNoTransito
                                        ,dep.idDependencia,dep.nombreDependencia
                                        from depositos d inner join catDelegaciones del on d.idDelegacion= del.idDelegacion
                                        inner join catMarcasVehiculos m on d.idMarca=m.idMarcaVehiculo
                                        inner join catColores col on d.idcolor = col.idcolor
                                        inner join pensiones pen on d.idpension	= pen.idpension
                                        inner join catTramos ctra  on d.idtramo=ctra.idtramo
                                        inner join catSubmarcasVehiculos  subm on d.idSubmarca=subm.idSubmarca
                                        inner join solicitudes sol on d.idsolicitud = sol.idsolicitud
                                        inner join infracciones inf on sol.idinfraccion = inf.idinfraccion
                                        inner join	vehiculos  veh on sol.idvehiculo =veh.idvehiculo 
                                        inner join concesionarios con on con.IdConcesionario =d.IdConcesionario
                                        left join dependencias dep on ((dep.idDependencia=d.IdDependenciaTransito)OR (dep.idDependencia=d.IdDependenciaNoTransito))
                                        where  sol.estatus !=0 and d.estatus!=0 and	d.idDeposito=@idDeposito ";

                    SqlCommand command = new SqlCommand(SqlTransact, connection);
                    command.Parameters.Add(new SqlParameter("@idDeposito", SqlDbType.Int)).Value = (object)IdDeposito ?? DBNull.Value;


                    command.CommandType = CommandType.Text;
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            transito.IdDeposito = Convert.ToInt32(reader["IdDeposito"].ToString());
                            transito.IdSolicitud = Convert.ToInt32(reader["IdSolicitud"].ToString());
                            transito.IdDelegacion = Convert.ToInt32(reader["IdDelegacion"].ToString());
                            transito.IdMarca = Convert.ToInt32(reader["IdMarca"].ToString());
                            transito.IdSubmarca = Convert.ToInt32(reader["IdSubmarca"].ToString());
                            transito.IdPension = Convert.ToInt32(reader["IdPension"].ToString());
                            transito.IdTramo = Convert.ToInt32(reader["IdTramo"].ToString());
                            transito.IdColor = Convert.ToInt32(reader["IdColor"].ToString());
                            transito.Serie = reader["Serie"].ToString();
                            transito.Placa = reader["Placa"].ToString();
                            transito.FechaIngreso = Convert.ToDateTime(reader["FechaIngreso"].ToString());
                            transito.FechaLiberacion = Convert.ToDateTime(reader["FechaLiberacion"].ToString());
                            transito.Folio = reader["Folio"].ToString();
                            transito.Km = reader["Km"].ToString();
                            transito.Liberado = Convert.ToInt32(reader["Liberado"].ToString());
                            transito.Autoriza = reader["Autoriza"].ToString();
                            transito.FechaActualizacion = Convert.ToDateTime(reader["FechaActualizacion"].ToString());
                            transito.ActualizadoPor = Convert.ToInt32(reader["ActualizadoPor"].ToString());
                            transito.DepositoEstatus = Convert.ToInt32(reader["Estatus"].ToString());
                            transito.marcaVehiculo = reader["marcaVehiculo"].ToString();
                            transito.nombreSubmarca = reader["nombreSubmarca"].ToString();
                            transito.delegacion = reader["delegacion"].ToString();
                            transito.modelo = reader["modelo"].ToString();
                            transito.solicitanteNombre = reader["solicitanteNombre"].ToString();
                            transito.solicitanteAp = reader["solicitanteAp"].ToString();
                            transito.solicitanteAm = reader["solicitanteAm"].ToString();
                            transito.Color = reader["Color"].ToString();
                            transito.pension = reader["pension"].ToString();
                            transito.tramo = reader["tramo"].ToString();

                            //nuevos
                            transito.FechaSolicitud = Convert.ToDateTime(reader["FechaSolicitud"].ToString());
                            transito.IdDependencia = Convert.ToInt32(reader["IdDependencia"].ToString());
                            transito.NombreDependencia = reader["NombreDependencia"].ToString();
                            transito.IdInfraccion = Convert.ToInt32(reader["IdInfraccion"].ToString());
                            transito.FolioInfraccion = reader["folioInfraccion"].ToString();
                            transito.IdVehiculo = Convert.ToInt32(reader["IdVehiculo"].ToString());
                            transito.propietario = reader["propietario"].ToString();
                            transito.numeroEconomico = reader["propietario"].ToString();
                            transito.FolioSolicitud = reader["FolioSolicitud"].ToString();
                            transito.IdConcesionario = Convert.ToInt32(reader["IdConcesionario"].ToString());
                            transito.Concesionario = reader["concesionario"].ToString();
                            transito.IdDependenciaGenera = reader["IdDependenciaGenera"] as int? ?? default(int);
                            transito.IdDependenciaTransito = reader["IdDependenciaTransito"] as int? ?? default(int);
                            transito.IdDependenciaNoTransito = reader["IdDependenciaNoTransito"] as int? ?? default(int);

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
            return transito;
        }


        /// <summary>
        /// Servicio Provicional ya que no tenemos acceso a repositorio
        /// </summary>
        /// <returns></returns>
        public List<Pensiones> GetPensiones()
        {
            List<Pensiones> pensiones = new List<Pensiones>();

            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
                try

                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("Select * from pensiones where estatus=1", connection);
                    command.CommandType = CommandType.Text;
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            Pensiones pension = new Pensiones();
                            pension.IdPension = Convert.ToInt32(reader["idPension"].ToString());
                            pension.Pension = reader["pension"].ToString();
                            pensiones.Add(pension);
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
            return pensiones;
        }

        public int DeleteTransitoTransporte(int IdDeposito, int IdSolicitud)
        {
            int result = 0;
            if ((IdDeposito != null || IdDeposito != 0) && (IdSolicitud != null || IdSolicitud != 0))
            {
                using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
                {
                    try
                    {
                        connection.Open();
                        const string SqlTransact =
                            @"UPDATE depositos
                        SET estatus= 0
                        WHERE idDeposito =@IdDeposito

                        UPDATE solicitudes
                        SET estatus= 0
                        WHERE idSolicitud =@IdSolicitud";

                        SqlCommand command = new SqlCommand(SqlTransact, connection);
                        command.Parameters.Add(new SqlParameter("@IdDeposito", SqlDbType.Int)).Value = (object)IdDeposito ?? DBNull.Value;
                        command.Parameters.Add(new SqlParameter("@IdSolicitud", SqlDbType.Int)).Value = (object)IdSolicitud ?? DBNull.Value;
                        command.CommandType = CommandType.Text;
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
            }
            return result;

        }
    }
}
