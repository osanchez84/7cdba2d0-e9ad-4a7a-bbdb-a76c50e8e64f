using GuanajuatoAdminUsuarios.Data;
using GuanajuatoAdminUsuarios.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GuanajuatoAdminUsuarios.Controllers
{
    [Authorize]
    public class CatOficinasController : Controller
    {
        Oficinas catalogoOficina = new Oficinas();
        Entidades catalogoEntidad = new Entidades();

        // GET: CatOficinasController
        public IActionResult Index()
        {
            return View("../Catalogos/Oficinas/Index");
        }

        // GET: CatOficinasController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult GetOficinas([DataSourceRequest] DataSourceRequest request)
        {
            List<Oficina> listOficinas = new List<Oficina>();

            listOficinas = catalogoOficina.GetOficinas();


            DataSourceResult gridOficinas = listOficinas.ToDataSourceResult(request, cm => new
            {
                Id = cm.Id,
                Descripcion = cm.Descripcion,
                Estatus = cm.Estatus
            });
            return Json(gridOficinas);
        }

        // GET: CatOficinasController/Create
        public IActionResult Crear()
        {
            return View("../Catalogos/Oficinas/Create");
        }

        // POST: CatOficinasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear([Bind("Descripcion,IdEntidad")] Oficina oficina)
        {
            if (ModelState.IsValid)
            {
                var idUsuario = User.FindFirst("IdUsuario").Value;
                catalogoOficina.GuardaOficina(oficina.Descripcion, oficina.IdEntidad, Int32.Parse(idUsuario));
                return View("../Catalogos/Oficinas/Index");
            }
            return View("../Catalogos/Oficinas/Create", oficina);
        }


        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var catOficina = catalogoOficina.GetOficinaById(id);

            return View("../Catalogos/Oficinas/Edit", catOficina);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarOficina([Bind("Id,Descripcion,IdEntidad,Estatus")] Oficina oficina)
        {
             if (ModelState.IsValid)
            {
                var idUsuario = User.FindFirst("IdUsuario").Value;
                catalogoOficina.ActualizaOficina(oficina.Id, oficina.Descripcion, oficina.IdEntidad, oficina.Estatus, idUsuario);
             return RedirectToAction(nameof(Index));
            }

             return View("../Catalogos/Oficinas/Edit", oficina);
        }

        public JsonResult GetEntidadesAjax()
        {
            var entidades = catalogoEntidad.GetEntidades();

            return Json(entidades, new Newtonsoft.Json.JsonSerializerSettings());
        }
    }
}
