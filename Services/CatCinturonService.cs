using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using System.Collections.Generic;
using System.Data;
using System;
using System.Data.SqlClient;
using GuanajuatoAdminUsuarios.Interfaces;

namespace GuanajuatoAdminUsuarios.Services
{
    public class CatCinturonService : ICatCinturon
    {
        private readonly ISqlClientConnectionBD _sqlClientConnectionBD;
        public CatCinturonService(ISqlClientConnectionBD sqlClientConnectionBD)
        {
            _sqlClientConnectionBD = sqlClientConnectionBD;
        }
        public List<CatCinturonModel> ObtenerCinturon()
        {
            //
            List<CatCinturonModel> ListaCinturon = new List<CatCinturonModel>();

            using (SqlConnection connection = new SqlConnection(_sqlClientConnectionBD.GetConnection()))
                try

                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT catCinturon.*, estatus.estatusdesc FROM catCinturon JOIN estatus ON catCinturon.estatus = estatus.estatus;", connection);
                    command.CommandType = CommandType.Text;
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            CatCinturonModel cinturon = new CatCinturonModel();
                            cinturon.IdCinturon = Convert.ToInt32(reader["IdCinturon"].ToString());
                            cinturon.Cinturon = reader["Cinturon"].ToString();
                            ListaCinturon.Add(cinturon);

                        }

                    }

                }
                catch (SqlException ex)
                {

                }
                finally
                {
                    connection.Close();
                }
            return ListaCinturon;


        }
    }
}

