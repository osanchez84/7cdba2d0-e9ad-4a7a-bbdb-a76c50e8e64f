using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuanajuatoAdminUsuarios.Models;
using System.Data.SqlClient;
using System.Data;

namespace GuanajuatoAdminUsuarios.Data
{
    public class SeguridadService
    {
        public Usuario GetLogin(string usuario, string password)
        {
            var oUsuario = new Usuario();
            var cn = new BDContext();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_Login", conexion);
                cmd.Parameters.AddWithValue("@usuario", usuario);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {


                        oUsuario.IdUsuario = Convert.ToInt32(dr["ID"]);
                        oUsuario.Nombre = dr["NOMBRE"].ToString();
                        oUsuario.Paterno = dr["PATERNO"].ToString();
                        oUsuario.Materno = dr["MATERNO"].ToString();
                    }
                }

            }

            return oUsuario;
        }


       


    }
}
