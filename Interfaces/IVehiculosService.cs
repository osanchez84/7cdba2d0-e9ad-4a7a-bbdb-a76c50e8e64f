using GuanajuatoAdminUsuarios.Models;
using System.Collections.Generic;

namespace GuanajuatoAdminUsuarios.Interfaces
{
    public interface IVehiculosService
    {
        public IEnumerable<VehiculoModel> GetAllVehiculos();
        public VehiculoModel GetVehiculoById(int idVehiculo);
        public VehiculoModel GetVehiculoToAnexo(VehiculoBusquedaModel model);
    }
}
