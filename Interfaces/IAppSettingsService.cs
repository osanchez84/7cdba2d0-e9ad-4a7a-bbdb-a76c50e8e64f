using GuanajuatoAdminUsuarios.Models;

namespace GuanajuatoAdminUsuarios.Interfaces
{
    public interface IAppSettingsService
    {
        public AppSettingsModel GetAppSetting(string settingName);
    }
}
