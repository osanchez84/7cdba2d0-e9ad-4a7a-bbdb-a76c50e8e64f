using GuanajuatoAdminUsuarios.Utils.Interfaces;
using Newtonsoft.Json;
using Org.BouncyCastle.Security;
using System.Diagnostics;
using System.Threading.Tasks;
using System;
using GuanajuatoAdminUsuarios.Models;
using static GuanajuatoAdminUsuarios.RESTModels.ConsultarDocumentoResponseModel;
using System.Data;
using System.Data.SqlClient;

namespace GuanajuatoAdminUsuarios.Utils.Services
{
    public class LogService : ILogService
    {
        public Guid RequestId { get; set; }

        public bool OnlyErrors { get; set; }
        public bool IsDBLog { get; set; }

        public LogService()
        {
            IsDBLog = true;
        }

        public void Error(string Message, object extraParams = null)
        {
            Log("Error", Message, RequestId, extraParams);
        }

        public void Error(Exception ex, object extraParams = null)
        {
            Log("Error", ex.Message, RequestId, new { ex, extraParams });
        }

        public async Task ErrorAsync(Exception ex, object extraParams = null)
        {
            await LogAsync("Error", ex.Message, RequestId, new { ex, extraParams });
        }

        public async Task ErrorAsync(string Message, object extraParams = null)
        {
            await LogAsync("Error", Message, RequestId, extraParams);
        }

        public void Error(Exception ex, Guid requestId, object extraParams = null)
        {
            Log("Error", ex.Message, requestId, new { ex, extraParams });
        }

        public void Error(Exception ex, Guid? requestId = null, object extraParams = null)
        {
            var error = "Error";

            if (ex is SignatureException exception)
                Log(error, exception, requestId, new { ex, extraParams });
            else
                Log(error, ex.Message, requestId, new { ex, extraParams });
        }

        public async Task ErrorAsync(Exception ex, Guid? requestId = null, object extraParams = null)
        {
            var error = "Error";

            if (ex is SignatureException exception)
                await LogAsync(error, exception, requestId, extraParams);
            else
                await LogAsync(error, ex.Message, requestId, new { ex, extraParams });
        }

        public async Task ErrorAsync(string Message, Guid? requestId = null, object extraParams = null)
        {
            await LogAsync("Error", Message, requestId, extraParams);
        }

        public void Trace(string message, object extraParameters = null)
        {
            Log("Trace", message, RequestId, extraParameters);
        }

        public void Trace(string message, Guid? requestId, object extraParameters = null)
        {
            Log("Trace", message, requestId, extraParameters);
        }

        public void Trace(string applicationName, string message, Guid? requestId = null, object extraParameters = null)
        {
            Log("Trace", message, requestId, extraParameters);
        }

        public void Log(string type, string message, object extraParameters = null)
        {
        }

        public void Log(string type, string message, Guid? requestId, object extraParameters = null)
        {
        }
        public void Log(string type, SignatureException exception, Guid? requestId = null, object extraParameters = null)
        {
        }

        public async Task LogAsync(string type, SignatureException message, Guid? requestId, object extraParameters = null)
        {
        }

        public async Task LogAsync(string type, string message, Guid? requestId = null, object extraParameters = null)
        {

        }

     /*   private void Save()
        {
            string strQuery = @"INSERT INTO accidentes( 
                                         [Hora]
                                        ,[idOficinaDelegacion]
                                        ,[idMunicipio]
                                        ,[idTramo]
                                        ,[Fecha]
                                        ,[idCarretera]
                                        ,[kilometro]
                                        ,[idEstatusReporte]
                                        ,[fechaActualizacion]
                                        ,[actualizadoPor]
                                        ,[estatus])
                                VALUES (
                                         @Hora
                                        ,@idOficina
                                        ,@idMunicipio
                                        ,@idTramo
                                        ,@Fecha
                                        ,@idCarretera
                                        ,@kilometro
                                        ,@idEstatusReporte
                                        ,@fechaActualizacion
                                        ,@actualizadoPor
                                        ,@estatus);
                                    SELECT SCOPE_IDENTITY();"; // Obtener el último ID insertado
            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(strQuery, connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("@Hora", SqlDbType.Time)).Value = (object)model.Hora ?? DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@idOficina", SqlDbType.Int)).Value = (object)idOficina ?? DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@kilometro", SqlDbType.NVarChar)).Value = (object)model.Kilometro ?? DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@idMunicipio", SqlDbType.Int)).Value = (object)model.IdMunicipio ?? DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@idEstatusReporte", SqlDbType.Int)).Value = 1;
                    command.Parameters.Add(new SqlParameter("@idTramo", SqlDbType.Int)).Value = (object)model.IdTramo ?? DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@idCarretera", SqlDbType.Int)).Value = (object)model.IdCarretera ?? DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@Fecha", SqlDbType.DateTime)).Value = (object)model.Fecha ?? DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@fechaActualizacion", SqlDbType.DateTime)).Value = DateTime.Now.ToString("yyyy-MM-dd");
                    command.Parameters.Add(new SqlParameter("@actualizadoPor", SqlDbType.Int)).Value = 1;
                    command.Parameters.Add(new SqlParameter("@estatus", SqlDbType.Int)).Value = 1;
                    result = Convert.ToInt32(command.ExecuteScalar()); // Valor de IdAccidente de este mismo registro
                    lastInsertedId = result; // Almacena el valor en la variable lastInsertedId
                }
                catch (SqlException ex)
                {
                    return lastInsertedId;
                }
                finally
                {
                    connection.Close();
                }
            }
            return lastInsertedId;
        }*/

    }
}
