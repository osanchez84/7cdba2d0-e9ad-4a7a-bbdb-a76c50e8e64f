using GuanajuatoAdminUsuarios.Entity;
using GuanajuatoAdminUsuarios.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Example.WebUI.Controllers
{
    public class CatClasificacionAccidentesController : Controller
    {
        DBContextInssoft dbContext = new DBContextInssoft();
        public IActionResult Index()
        {
            var ListClasificacionAccidentesModel = GetClasificacionAccidentes();

            return View(ListClasificacionAccidentesModel);

        }




        #region Modal Action
        public ActionResult IndexModal()
        {
            var ListClasificacionAccidentesModel = GetClasificacionAccidentes();
            return View("Index", ListClasificacionAccidentesModel);
        }

        [HttpPost]
        public ActionResult AgregarClasificacionAccidenteModal()
        {
            //SetDDLDependencias();
            return PartialView("_Crear");
        }

        public ActionResult EditarClasificacionAccidenteModal(int IdClasificacionAccidente)
        {
            var clasificacionAccidentesModel = GetClasificacionAccidenteByID(IdClasificacionAccidente);
            return PartialView("_Editar", clasificacionAccidentesModel);
        }

        public ActionResult EliminarClasificacionAccidenteModal(int IdClasificacionAccidente)
        {
            var clasificacionAccidentesModel = GetClasificacionAccidenteByID(IdClasificacionAccidente);
            return PartialView("_Eliminar", clasificacionAccidentesModel);
        }



        [HttpPost]
        public ActionResult AgregarClasificacionAccidente(CatClasificacionAccidentesModel model)
        {
            var errors = ModelState.Values.Select(s => s.Errors);
            ModelState.Remove("NombreClasificacion");
            if (ModelState.IsValid)
            {


                CrearClasificacionAccidente(model);
                var ListClasificacionAccidentesModel = GetClasificacionAccidentes();
                return PartialView("_ListaClasificacionAccidentes", ListClasificacionAccidentesModel);
            }

            return PartialView("_Crear");
        }

        public ActionResult EditarClasificacionAccidenteMod(CatClasificacionAccidentesModel model)
        {
            var errors = ModelState.Values.Select(s => s.Errors);
            ModelState.Remove("NombreClasificacion");
            if (ModelState.IsValid)
            {


                EditarClasificacionAccidente(model);
                var ListClasificacionAccidentesModel = GetClasificacionAccidentes();
                return PartialView("_ListaClasificacionAccidentes", ListClasificacionAccidentesModel);
            }
            return PartialView("_Editar");
        }

        public ActionResult EliminarClasificacionAccidenteMod(CatClasificacionAccidentesModel model)
        {
            var errors = ModelState.Values.Select(s => s.Errors);
            ModelState.Remove("NombreClasificacion");
            if (ModelState.IsValid)
            {


                EliminaClasificacionAccidente(model);
                var ListClasificacionAccidentesModel = GetClasificacionAccidentes();
                return PartialView("_ListaClasificacionAccidentes", ListClasificacionAccidentesModel);
            }
            return PartialView("_Eliminar");
        }
        public JsonResult GetClasAccidentes([DataSourceRequest] DataSourceRequest request)
        {
            var ListClasificacionAccidentesModel = GetClasificacionAccidentes();

            return Json(ListClasificacionAccidentesModel.ToDataSourceResult(request));
        }




        #endregion


        #region Acciones a base de datos

        public void CrearClasificacionAccidente(CatClasificacionAccidentesModel model)
        {
            CatClasificacionAccidentes clasificacion = new CatClasificacionAccidentes();
            clasificacion.IdClasificacionAccidente = model.IdClasificacionAccidente;
            clasificacion.NombreClasificacion = model.NombreClasificacion;
            clasificacion.Estatus = 1;
            clasificacion.FechaActualizacion = DateTime.Now;
            dbContext.CatClasificacionAccidentes.Add(clasificacion);
            dbContext.SaveChanges();
        }

        public void EditarClasificacionAccidente(CatClasificacionAccidentesModel model)
        {
            CatClasificacionAccidentes clasificacion = new CatClasificacionAccidentes();
            clasificacion.IdClasificacionAccidente = model.IdClasificacionAccidente;
            clasificacion.NombreClasificacion = model.NombreClasificacion;
            clasificacion.Estatus = 1;
            clasificacion.FechaActualizacion = DateTime.Now;
            dbContext.Entry(clasificacion).State = EntityState.Modified;
            dbContext.SaveChanges();

        }

        public void EliminaClasificacionAccidente(CatClasificacionAccidentesModel model)
        {

            CatClasificacionAccidentes clasificacion = new CatClasificacionAccidentes();
            clasificacion.IdClasificacionAccidente = model.IdClasificacionAccidente;
            clasificacion.NombreClasificacion = model.NombreClasificacion;
            clasificacion.Estatus =0;
            clasificacion.FechaActualizacion = DateTime.Now;
            dbContext.Entry(clasificacion).State = EntityState.Modified;
            dbContext.SaveChanges();

        }




        public CatClasificacionAccidentesModel GetClasificacionAccidenteByID(int IdClasificacionAccidente)
        {

            var productEnitity = dbContext.CatAutoridadesEntrega.Find(IdClasificacionAccidente);

            var clasificacionAccidentesModel = (from catClasificacionAccidentes in dbContext.CatClasificacionAccidentes.ToList()
                                              select new CatClasificacionAccidentesModel

                                              {
                                                  IdClasificacionAccidente = catClasificacionAccidentes.IdClasificacionAccidente,
                                                  NombreClasificacion = catClasificacionAccidentes.NombreClasificacion,


                                              }).Where(w => w.IdClasificacionAccidente == IdClasificacionAccidente).FirstOrDefault();

            return clasificacionAccidentesModel;
        }

        /// <summary>
        /// Linq es una tecnologia de control de datos (excel, txt,EF,sqlclient etc)
        /// para la gestion un mejor control de la info
        /// </summary>
        /// <returns></returns>
        public List<CatClasificacionAccidentesModel> GetClasificacionAccidentes()
        {
            var ListClasificacionAccidentesModel = (from catClasificacionAccidentes in dbContext.CatClasificacionAccidentes.ToList()
                                                  join estatus in dbContext.Estatus.ToList()
                                                  on catClasificacionAccidentes.Estatus equals estatus.estatus
                                                  where catClasificacionAccidentes.Estatus == 1
                                                  select new CatClasificacionAccidentesModel
                                                  {
                                                      IdClasificacionAccidente = catClasificacionAccidentes.IdClasificacionAccidente,
                                                      NombreClasificacion = catClasificacionAccidentes.NombreClasificacion,
                                                      estatusDesc = estatus.estatusDesc,

                                                  }).ToList();
            return ListClasificacionAccidentesModel;
        }
        #endregion



    }
}
