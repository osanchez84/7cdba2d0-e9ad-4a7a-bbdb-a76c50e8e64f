using GuanajuatoAdminUsuarios.Interfaces;

namespace GuanajuatoAdminUsuarios.Services
{
    public class BusquedaAccidentesService : IBusquedaAccidentesService
    {
        private readonly ISqlClientConnectionBD _sqlClientConnectionBD;
        public BusquedaAccidentesService(ISqlClientConnectionBD sqlClientConnectionBD)
        {
            _sqlClientConnectionBD = sqlClientConnectionBD;
        }
    }
}
