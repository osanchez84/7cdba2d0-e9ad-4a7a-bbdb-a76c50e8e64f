using GuanajuatoAdminUsuarios.Interfaces;
using SITTEG.APIClientInfrastructure.Client;

namespace GuanajuatoAdminUsuarios.Services
{
    public class CotejarDocumentosClientService : BaseApiClientService
    {
        protected readonly IGenericClient _apiClient;
        private const string uri = "https://alfasiae.guanajuato.gob.mx";

        public CotejarDocumentosClientService(IGenericClient apiClient) : base(apiClient, uri)
        {
        }

        public void consumeService()
        {
            string url = "/RESTAdapter/CotejarDatos";
            dynamic requestModel = new { x = 1 };
            //var x = _apiClient.PostGenericResponse<, >(url, requestModel);
        }
    }
}
