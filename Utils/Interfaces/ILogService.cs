using Org.BouncyCastle.Security;
using System.Threading.Tasks;
using System;

namespace GuanajuatoAdminUsuarios.Utils.Interfaces
{
    public interface ILogService 
    {
        public void Error(string Message, object extraParams = null);
        public void Error(Exception ex, object extraParams = null);
        public Task ErrorAsync(Exception ex, object extraParams = null);
        public Task ErrorAsync(string Message, object extraParams = null);
        public void Error(Exception ex, Guid requestId, object extraParams = null);
        public void Error(Exception ex, Guid? requestId = null, object extraParams = null);
        public Task ErrorAsync(Exception ex, Guid? requestId = null, object extraParams = null);
        public Task ErrorAsync(string Message, Guid? requestId = null, object extraParams = null);
        public void Trace(string message, object extraParameters = null);
        public void Trace(string message, Guid? requestId, object extraParameters = null);
        public void Trace(string applicationName, string message, Guid? requestId = null, object extraParameters = null);
        public void Log(string type, string message, object extraParameters = null);
        public void Log(string type, string message, Guid? requestId, object extraParameters = null);
        public void Log(string type, SignatureException exception, Guid? requestId = null, object extraParameters = null);
        public Task LogAsync(string type, SignatureException message, Guid? requestId, object extraParameters = null);
        public Task LogAsync(string type, string message, Guid? requestId = null, object extraParameters = null);
    }
}
