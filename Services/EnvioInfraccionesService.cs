using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace GuanajuatoAdminUsuarios.Services
{
    public class EnvioInfraccionesService : IEnvioInfraccionesService
    {
        private readonly ISqlClientConnectionBD _sqlClientConnectionBD;
        public EnvioInfraccionesService(ISqlClientConnectionBD sqlClientConnectionBD)
        {
            _sqlClientConnectionBD = sqlClientConnectionBD;
        }
    }
}
