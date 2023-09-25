using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using Microsoft.Data.SqlClient;
using SITTEG.APIClientInfrastructure.Client;
using System;
using System.Data;

namespace GuanajuatoAdminUsuarios.Services
{
    public class AppSettingService :IAppSettingsService
    {

        private readonly ISqlClientConnectionBD _sqlClientConnectionBD;

        public AppSettingService(ISqlClientConnectionBD sqlClientConnectionBD)
        {
            _sqlClientConnectionBD = sqlClientConnectionBD;
        }
        public AppSettingsModel GetAppSetting(string settingName)
        {
            AppSettingsModel model = null;

            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
            {
                try
                {
                    connection.Open();
                    string strQuery = @"SELECT 
                                        Id
                                        ,SettingName
                                        ,SettingValue
                                        ,IsActive
                                        FROM serviceAppSettings
                                        WHERE SettingName = @settingName";
                    SqlCommand sqlCommand = new SqlCommand(strQuery, connection);
                    sqlCommand.Parameters.Add(new SqlParameter("@settingName", SqlDbType.NVarChar)).Value = settingName;
                    sqlCommand.CommandType = CommandType.Text;

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            model = new AppSettingsModel();
                            model.id = Convert.ToInt32(reader["Id"]);
                            model.SettingName = Convert.ToString(reader["SettingName"]);
                            model.SettingValue = Convert.ToString(reader["SettingValue"]);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Manejo de errores y log
                    return model = null;
                }
                finally
                {
                    connection.Close();
                }
            }
            return model;
        }

    }
}
