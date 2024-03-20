using GuanajuatoAdminUsuarios.Entity;
using GuanajuatoAdminUsuarios.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GuanajuatoAdminUsuarios.Interfaces
{
    public interface IDepositosService
    {
        string GuardarSolicitud(SolicitudDepositoModel model,int idOficina,string oficina,string abreviaturaMunicipio,int anio,int dependencia);
        SolicitudDepositoModel ObtenerSolicitudPorID(int Isol);
        int ActualizarSolicitud(int? Isol,SolicitudDepositoModel model);
        int CompletarSolicitud(SolicitudDepositoModel model);
        SolicitudDepositoModel ImportarInfraccion(string folioBusquedaInfraccion, int idDependencia);

    
        SolicitudDepositoModel ImportarInfraccion(int folioBusquedaInfraccion,int idDependencia);

        List<SolicitudDepositoModel> ObtenerServicios();

    }
}
