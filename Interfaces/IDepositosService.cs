using GuanajuatoAdminUsuarios.Entity;
using GuanajuatoAdminUsuarios.Models;
using System.Collections.Generic;

namespace GuanajuatoAdminUsuarios.Interfaces
{
    public interface IDepositosService
    {
        int GuardarSolicitud(SolicitudDepositoModel model);
        SolicitudDepositoModel ObtenerSolicitudPorID(int Isol);
        int ActualizarSolicitud(int? Isol,SolicitudDepositoModel model);
        int CompletarSolicitud(SolicitudDepositoModel model);
        
    }
}
