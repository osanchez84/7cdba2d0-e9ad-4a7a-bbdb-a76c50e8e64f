using GuanajuatoAdminUsuarios.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GuanajuatoAdminUsuarios.Data
{
    public class Usuarios
    {


        public List<Usuario> ListarUsuarios()
        {
            var oLista = new List<Usuario>();
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_GetUsuarios", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oLista.Add(new Usuario()
                        {
                            IdUsuario = Convert.ToInt32(dr["ID"]),
                            Nombre = dr["NOMBRE"].ToString(),
                            Paterno = dr["PATERNO"].ToString(),
                            Materno = dr["MATERNO"].ToString(),
                            Estatus = Convert.ToInt32(dr["Estatus"])


                        });
                    }
                }

            }

            return oLista;
        }

        public string GuardaUsuario(string nombre, string paterno, string materno, string email, string login,
                                    string clave, string idUsuario)
        {
           // var oApss = new AppsModel();
            var cn = new Conexion();
            string salida = "";

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_guardaUsuario", conexion);
                cmd.Parameters.AddWithValue("Nombre", nombre);
                cmd.Parameters.AddWithValue("Paterno", paterno);
                cmd.Parameters.AddWithValue("Materno", materno);
                cmd.Parameters.AddWithValue("Email", email);
                cmd.Parameters.AddWithValue("Login", login);
                cmd.Parameters.AddWithValue("Clave", clave);
                cmd.Parameters.AddWithValue("IdUsuarioAlta", idUsuario);

                SqlParameter parm = new SqlParameter()
                {
                    ParameterName = "@returnVal",
                    SqlDbType = SqlDbType.VarChar,
                    Direction = System.Data.ParameterDirection.Output,
                    Size = 250
                };

                //  cmd.Parameters.AddWithValue("returnVal","");
                cmd.Parameters.Add(parm);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {
                    //while (dr.Read())
                    //{
                     //   salida = dr["ID"].ToString();

                    //}
                    salida = parm.Value.ToString();
                }

            }

            return salida;
        }


        public Usuario GetUsuario(int? idUsuario)
        {
            var oUsuario = new Usuario();
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_GetUsuario", conexion);
                cmd.Parameters.AddWithValue("idUsuario", idUsuario);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {

                        oUsuario.IdUsuario = Convert.ToInt32(dr["ID"]);
                        oUsuario.Nombre = dr["NOMBRE"].ToString();
                        oUsuario.Paterno = dr["PATERNO"].ToString();
                        oUsuario.Materno = dr["MATERNO"].ToString();
                        oUsuario.Email = dr["EMAIL"].ToString(); 
                        oUsuario.Login = dr["LOGIN"].ToString();
                        oUsuario.Clave = dr["PASSWORD"].ToString();
                        oUsuario.Estatus = Convert.ToInt32(dr["Estatus"]);


                       
                    }
                }

            }

            return oUsuario;
        }



        public string ActualizaUsuario(int id, string nombre, string paterno, string materno, string email, string login,
                                    int estatus, string idUsuario)
        {
            // var oApss = new AppsModel();
            var cn = new Conexion();
            string salida = "";

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_actualizaUsuario", conexion);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("Nombre", nombre);
                cmd.Parameters.AddWithValue("Paterno", paterno);
                cmd.Parameters.AddWithValue("Materno", materno);
                cmd.Parameters.AddWithValue("Email", email);
                cmd.Parameters.AddWithValue("Login", login);
                cmd.Parameters.AddWithValue("Estatus", estatus);
                cmd.Parameters.AddWithValue("IdUsuarioAlta", idUsuario);

                SqlParameter parm = new SqlParameter()
                {
                    ParameterName = "@returnVal",
                    SqlDbType = SqlDbType.VarChar,
                    Direction = System.Data.ParameterDirection.Output,
                    Size = 250
                };

                //  cmd.Parameters.AddWithValue("returnVal","");
                cmd.Parameters.Add(parm);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {
                    //while (dr.Read())
                    //{
                    //   salida = dr["ID"].ToString();

                    //}
                    salida = parm.Value.ToString();
                }

            }

            return salida;
        }



    }
}


