using GuanajuatoAdminUsuarios.Entity;
using GuanajuatoAdminUsuarios.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Example.WebUI.Controllers
{
    public class CatMunicipiosController : Controller
    {
        DBContextInssoft dbContext = new DBContextInssoft();
        public IActionResult Index()
        {
            //var products = dbContext.Products.ToList();
            var ListMunicipiosModel = GetMunicipios();

            return View(ListMunicipiosModel);

        }

        #region Modal Action
        public ActionResult IndexModal()
        {
            var ListMunicipiosModel = GetMunicipios();
            return View("Index", ListMunicipiosModel);
        }

        [HttpPost]
        public ActionResult AgregarMunicipioModal()
        {
            return PartialView("_Crear");
        }

        public ActionResult EditarMunicipioModal(int IdMunicipio)
        {
            var municipiosModel = GetMunicipioByID(IdMunicipio);
            return View("_Editar", municipiosModel);
        }

        public ActionResult EliminarMunicipioModal(int IdMunicipio)
        {
            var municipiosModel = GetMunicipioByID(IdMunicipio);
            return View("_Eliminar", municipiosModel);
        }


        [HttpPost]
        public ActionResult CrearMunicipioMod(CatMunicipiosModel model)
        {
            var errors = ModelState.Values.Select(s => s.Errors);
            ModelState.Remove("Municipio");
            if (ModelState.IsValid)
            {


                AgregarMunicipio(model);
                var ListMunicipiosModel = GetMunicipios();
                return PartialView("_ListaMunicipios", ListMunicipiosModel);
            }
            //SetDDLCategories();
            //return View("Create");
            return PartialView("_Crear");
        }

        public ActionResult EditarMunicipioMod(CatMunicipiosModel model)
        {
            var errors = ModelState.Values.Select(s => s.Errors);
            ModelState.Remove("Municipio");
            if (ModelState.IsValid)
            {


                EditarMunicipio(model);
                var ListMunicipiosModel = GetMunicipios();
                return PartialView("_ListaMunicipios", ListMunicipiosModel);
            }

            return PartialView("_Editar");
        }

        public ActionResult EliminarMunicipioMod(CatMunicipiosModel model)
        {
            var errors = ModelState.Values.Select(s => s.Errors);
            ModelState.Remove("Municipio");
            if (ModelState.IsValid)
            {


                EliminarMunicipio(model);
                var ListMunicipiosModel = GetMunicipios();
                return PartialView("_ListaMunicipios", ListMunicipiosModel);
            }

            return PartialView("_Eliminar");
        }
        public JsonResult GetMun([DataSourceRequest] DataSourceRequest request)
        {
            var ListMunicipiosModel = GetMunicipios();

            return Json(ListMunicipiosModel.ToDataSourceResult(request));
        }




        #endregion


        #region Acciones a base de datos

        public void AgregarMunicipio(CatMunicipiosModel model)
        {
            CatMunicipios municipio = new CatMunicipios();
            municipio.IdMunicipio = model.IdMunicipio;
            municipio.Municipio = model.Municipio;
            municipio.Estatus = 1;
            municipio.FechaActualizacion = DateTime.Now;
            dbContext.CatMunicipios.Add(municipio);
            dbContext.SaveChanges();
        }

        public void EditarMunicipio(CatMunicipiosModel model)
        {
            CatMunicipios municipio = new CatMunicipios();
            municipio.IdMunicipio = model.IdMunicipio;
            municipio.Municipio = model.Municipio;
            municipio.Estatus = 1;
            municipio.FechaActualizacion = DateTime.Now;
            dbContext.Entry(municipio).State = EntityState.Modified;
            dbContext.SaveChanges();

        }

        public void EliminarMunicipio(CatMunicipiosModel model)
        {
            CatMunicipios municipio = new CatMunicipios();
            municipio.IdMunicipio = model.IdMunicipio;
            municipio.Municipio = model.Municipio;
            municipio.Estatus = 1;
            municipio.FechaActualizacion = DateTime.Now;
            dbContext.Entry(municipio).State = EntityState.Modified;
            dbContext.SaveChanges();

        }

        private void SetDDLMunicipios()
        {
            ViewBag.Municipios = new SelectList(dbContext.CatMunicipios.ToList(), "IdMunicipio", "Municipio");
        }


        public CatMunicipiosModel GetMunicipioByID(int IdMunicipio)
        {

            var productEnitity = dbContext.CatMunicipios.Find(IdMunicipio);

            var municipiosModel = (from catMunicipios in dbContext.CatMunicipios.ToList()
                                   select new CatMunicipiosModel

                                   {
                                       IdMunicipio = catMunicipios.IdMunicipio,
                                       Municipio = catMunicipios.Municipio,


                                   }).Where(w => w.IdMunicipio == IdMunicipio).FirstOrDefault();

            return municipiosModel;
        }


        public List<CatMunicipiosModel> GetMunicipios()
        {
            var ListMunicipiosModel = (from catMunicipios in dbContext.CatMunicipios.ToList()
                                       join estatus in dbContext.Estatus.ToList()
                                       on catMunicipios.Estatus equals estatus.estatus
                                       select new CatMunicipiosModel
                                       {
                                           IdMunicipio = catMunicipios.IdMunicipio,
                                           Municipio = catMunicipios.Municipio,
                                           estatusDesc = estatus.estatusDesc

                                       }).ToList();
            return ListMunicipiosModel;
        }
        #endregion



    }
}
