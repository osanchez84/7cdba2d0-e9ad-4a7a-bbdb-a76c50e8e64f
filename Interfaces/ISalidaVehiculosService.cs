using GuanajuatoAdminUsuarios.Models;
using System.Collections.Generic;

namespace GuanajuatoAdminUsuarios.Interfaces
{
    public interface ISalidaVehiculosService
    {
        public List<SalidaVehiculosModel> ObtenerIngresos(SalidaVehiculosModel model);
        public SalidaVehiculosModel DetallesDeposito(int iDp);


    }
}
