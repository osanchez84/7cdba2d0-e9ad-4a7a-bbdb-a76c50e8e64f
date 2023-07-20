using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using GuanajuatoAdminUsuarios.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace GuanajuatoAdminUsuarios.Controllers
{
    public class EnvioInfraccionesController : Controller
    {
        private readonly IEnvioInfraccionesService _envioInfraccionesService;
        private readonly ICatOficinasRentaService _catOficinasRentaService;

        public EnvioInfraccionesController(IEnvioInfraccionesService envioInfraccionesService, ICatOficinasRentaService catOficinasRentaService)
        {
            _envioInfraccionesService = envioInfraccionesService;
            _catOficinasRentaService = catOficinasRentaService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult BusquedaInfracciones(EnvioInfraccionesModel model)
        {
            var resultadoBusqueda = _envioInfraccionesService.ObtenerInfracciones(model);
            return Json(resultadoBusqueda);


        }
        public JsonResult OficinasRenta_Drop()
        {
            var result = new SelectList(_catOficinasRentaService.ObtenerOficinasActivas(), "IdOficinaRenta", "NombreOficina");
            return Json(result);
        }
        public ActionResult MostrarModal(List<int> idInfracciones)
        {
      
            var modalModel = new ModalEnvioModel
            {
                SelectedIds = idInfracciones,
                Oficio = "",
                FechaEnvio = DateTime.Now,
                IdLugarEnvio = 0,
            };
            return PartialView("_ModalEnvioInfracciones", modalModel);
        }

        public ActionResult ajax_GuardarInfraccionesEnviadas(ModalEnvioModel model)
        {
            var guardarDatos = _envioInfraccionesService.GuardarEnvioInfracciones(model);
            return PartialView("Index");
        }



    }
}
