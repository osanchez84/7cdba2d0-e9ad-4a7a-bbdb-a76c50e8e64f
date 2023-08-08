using System.Threading.Tasks;

namespace GuanajuatoAdminUsuarios.WebClientServices
{
    public interface IRequestDynamic<T> where T : class
    {
        Task<string> EncryptionService(T model, string urlName);
    }
}
