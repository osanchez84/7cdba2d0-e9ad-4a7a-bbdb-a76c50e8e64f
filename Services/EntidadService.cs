using GuanajuatoAdminUsuarios.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.Data;

namespace GuanajuatoAdminUsuarios.Data
{
    public class EntidadService
    {
        public List<Entidad> GetEntidades()
        {
            var oLista = new List<Entidad>();
            var cn = new BDContext();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_getEntidades", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oLista.Add(new Entidad()
                        {
                            Id = Convert.ToInt32(dr["ID"]),
                            Descripcion = dr["DESCRIPCION"].ToString(),
                        });
                    }
                }

            }

            return oLista;
        }
    }
}
