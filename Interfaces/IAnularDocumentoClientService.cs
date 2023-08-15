using GuanajuatoAdminUsuarios.RESTModels;

namespace GuanajuatoAdminUsuarios.Interfaces
{
    public interface IAnularDocumentoClientService
    {
        public AnulacionDocumentoResponseModel AnularDocumento(AnulacionDocumentoRequestModel requestModel);

    }
}
