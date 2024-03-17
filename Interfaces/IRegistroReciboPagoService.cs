using GuanajuatoAdminUsuarios.Entity;
using GuanajuatoAdminUsuarios.Models;
using System;
using System.Collections.Generic;

namespace GuanajuatoAdminUsuarios.Interfaces
{
    public interface IRegistroReciboPagoService
    {
        List<RegistroReciboPagoModel> ObtInfracciones(string FolioInfraccion, int idDependencia);
        RegistroReciboPagoModel ObtenerDetallePorId(int Id);

        int GuardarRecibo(string ReciboPago, float Monto, DateTime FechaPago, string LugarPago, int IdInfraccion);



    }
}
