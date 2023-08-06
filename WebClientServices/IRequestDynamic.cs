using System.Threading.Tasks;

namespace GuanajuatoAdminUsuarios.WebClientServices
{
    public interface IRequestDynamic
    {
        Task<string> EncryptionService(string UsuarioLog, string PasswordLog, string ReciboControlInterno, string FechaReversa, string urlName);
    }
}
