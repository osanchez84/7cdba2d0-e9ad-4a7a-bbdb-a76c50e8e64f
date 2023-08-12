using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using SITTEG.APIClientInfrastructure.Client;
using System.Data;
using System;
using System.Data.SqlClient;

namespace GuanajuatoAdminUsuarios.Services
{
    public class CotejarDocumentosClientService : ICotejarDocumentosClientService
    {
        private readonly ISqlClientConnectionBD _sqlClientConnectionBD;
        protected readonly IGenericClient _apiClient;
        private const string uri = "https://alfasiae.guanajuato.gob.mx";

        public CotejarDocumentosClientService(IGenericClient apiClient, ISqlClientConnectionBD sqlClientConnectionBD) 
        {
            _sqlClientConnectionBD = sqlClientConnectionBD;
        }

        public void consumeService()
        {
            string url = "/RESTAdapter/CotejarDatos";
            dynamic requestModel = new { x = 1 };
            //var x = _apiClient.PostGenericResponse<, >(url, requestModel);
        }

        public string CrearPension()
        {

            var bodyRequest= "{\"Tp_folio\":\"4\",\"Folio\":\"E01038\",\"tp_consulta\":\"3\"}";
            var Url= "https://alfasiae.guanajuato.gob.mx/RESTAdapter/CotejarDatos";
            var user = "POEXTSSP_USR";
            var password = "fV115Kl*xGgV";

            string result = string.Empty;
            string strQuery = @"";
            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SP_WebClientConnect", connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("@URL", SqlDbType.NVarChar)).Value = Url;
                    command.Parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar)).Value = user;
                    command.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar)).Value = password;
                    command.Parameters.Add(new SqlParameter("@Body", SqlDbType.NVarChar)).Value = bodyRequest;                   
                    command.CommandType = CommandType.StoredProcedure;
                    result = Convert.ToString(command.ExecuteScalar());
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

    }
}
