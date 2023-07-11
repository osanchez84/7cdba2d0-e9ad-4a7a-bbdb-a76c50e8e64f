using GuanajuatoAdminUsuarios.Models;
using System.Collections.Generic;

namespace GuanajuatoAdminUsuarios.Interfaces
{
    public interface IBusquedaAccidentesService
    {
        List<BusquedaAccidentesModel> BusquedaAccidentes(BusquedaAccidentesModel model);
        public BusquedaAccidentesModel ObtenerAccidentePorId(int idAccidente);
        List<BusquedaAccidentesModel> ObtenerAccidentes(BusquedaAccidentesModel model);


    }
}
