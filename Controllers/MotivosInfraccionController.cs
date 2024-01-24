using GuanajuatoAdminUsuarios.Entity;
using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using static GuanajuatoAdminUsuarios.RESTModels.ConsultarDocumentoResponseModel;
using Microsoft.AspNetCore.Authorization;



namespace GuanajuatoAdminUsuarios.Controllers
{

    [Authorize]
    public class MotivosInfraccionController : BaseController
    {
        //DBContextInssoft dbContext = new DBContextInssoft();
        private readonly ICatDictionary _catDictionary;
        private readonly IMotivoInfraccionService _motivoInfraccionService;

        public MotivosInfraccionController(ICatDictionary catDictionary, IMotivoInfraccionService motivoInfraccionService)
        { 
            _catDictionary = catDictionary;
            _motivoInfraccionService = motivoInfraccionService;
        }

        public IActionResult Index()
        {
			int idDependencia = (int)HttpContext.Session.GetInt32("IdDependencia");
            int IdModulo = 1111;
            string listaPermisosJson = HttpContext.Session.GetString("Autorizaciones");
            List<int> listaPermisos = JsonConvert.DeserializeObject<List<int>>(listaPermisosJson);
            if (listaPermisos != null && listaPermisos.Contains(IdModulo))
            {
                
                var ListMotivosInfraccionModel = _motivoInfraccionService.GetMotivos(idDependencia);

            return View(ListMotivosInfraccionModel);
            }
            else
            {
                TempData["ErrorMessage"] = "El usuario no tiene permisos suficientes para esta acción.";
                return RedirectToAction("Principal", "Inicio", new { area = "" });
            }
        }



        #region Modal Action
        public ActionResult IndexModal()
        {
            int IdModulo = 1111;
            string listaPermisosJson = HttpContext.Session.GetString("Autorizaciones");
            List<int> listaPermisos = JsonConvert.DeserializeObject<List<int>>(listaPermisosJson);
            if (listaPermisos != null && listaPermisos.Contains(IdModulo))
            {
                int idDependencia = (int)HttpContext.Session.GetInt32("IdDependencia");
			var ListMotivosInfraccionModel = _motivoInfraccionService.GetMotivos(idDependencia);
            //return View("IndexModal");
            return View("Index", ListMotivosInfraccionModel);
        }
            else
            {
                TempData["ErrorMessage"] = "El usuario no tiene permisos suficientes para esta acción.";
                return PartialView("ErrorPartial");
            }
        }

        [HttpPost]
        public ActionResult AgregarMotivoParcial()
        {
                int IdModulo = 1113;
                string listaPermisosJson = HttpContext.Session.GetString("Autorizaciones");
                List<int> listaPermisos = JsonConvert.DeserializeObject<List<int>>(listaPermisosJson);
                if (listaPermisos != null && listaPermisos.Contains(IdModulo))
                {
                    var catConcepto = _catDictionary.GetCatalog("CatConceptoInfraccion", "0");
                ViewData["CatConcepto"] = new SelectList(catConcepto.CatalogList, "Id", "Text");
                return View("_Crear");
            }
            else
            {
                TempData["ErrorMessage"] = "El usuario no tiene permisos suficientes para esta acción.";
                return PartialView("ErrorPartial");
            }
        }

        public ActionResult EditarParcial(int IdCatMotivoInfraccion)
        {
			int idDependencia = (int)HttpContext.Session.GetInt32("IdDependencia");
                int IdModulo = 1115;
                string listaPermisosJson = HttpContext.Session.GetString("Autorizaciones");
                List<int> listaPermisos = JsonConvert.DeserializeObject<List<int>>(listaPermisosJson);
                if (listaPermisos != null && listaPermisos.Contains(IdModulo))
                {
                    var motivosInfraccionsModel = _motivoInfraccionService.GetMotivoByID(IdCatMotivoInfraccion, idDependencia);
                
                var catConcepto = _catDictionary.GetCatalog("CatConceptoInfraccion", "0");
                var catSubConcepto = _catDictionary.GetCatalog("CatSubConceptoInfraccion", motivosInfraccionsModel.idConcepto+"");
                ViewData["CatConcepto"] = new SelectList(catConcepto.CatalogList, "Id", "Text");
                ViewData["CatSubConceptoInfraccion"] = new SelectList(catSubConcepto.CatalogList, "Id", "Text");
                return View("_Editar", motivosInfraccionsModel);
                 }
            else
            {
                TempData["ErrorMessage"] = "El usuario no tiene permisos suficientes para esta acción.";
                return PartialView("ErrorPartial");
    }
}

        public ActionResult EliminarMotivoParcial(int IdCatMotivoInfraccion)
        {

			int idDependencia = (int)HttpContext.Session.GetInt32("IdDependencia");
			var motivosInfraccionsModel = _motivoInfraccionService.GetMotivoByID(IdCatMotivoInfraccion, idDependencia);
            return View("_Eliminar", motivosInfraccionsModel);
        }
        public JsonResult Categories_Read()
        {

			int idDependencia = (int)HttpContext.Session.GetInt32("IdDependencia");
			var result = new SelectList(_motivoInfraccionService.GetCatMotivos(idDependencia), "IdCatMotivoInfraccion", "Nombre");
            return Json(result);
        }



        [HttpPost]
        public ActionResult CreatePartialMotivoModal(CatMotivosInfraccionModel model)
        {

			int idDependencia = (int)HttpContext.Session.GetInt32("IdDependencia");
			var errors = ModelState.Values.Select(s => s.Errors);
            ModelState.Remove("Nombre");
            if (ModelState.IsValid)
            {


                CreateMotivo(model);
                var ListMotivosInfraccionModel = _motivoInfraccionService.GetMotivos(idDependencia);
                return Json(ListMotivosInfraccionModel);
            }
            //SetDDLCategories();
            //return View("Create");
            return PartialView("_Crear");
        }

        [HttpPost]
        public ActionResult EditarParcialModal(CatMotivosInfraccionModel model)
        {

			int idDependencia = (int)HttpContext.Session.GetInt32("IdDependencia");
			bool switchMotivosInfraccion = Request.Form["motivosInfraccionSwitch"].Contains("true");
            model.estatus = switchMotivosInfraccion ? 1 : 0;
            var errors = ModelState.Values.Select(s => s.Errors);
            ModelState.Remove("Nombre");
            if (ModelState.IsValid)
            { 

                UpdateMotivo(model);
                var ListMotivosInfraccionModel = _motivoInfraccionService.GetMotivos(idDependencia);
                return Json(ListMotivosInfraccionModel);
            }
            return PartialView("_Editar");
        }

        [HttpPost]
        public ActionResult EliminarMotivoParcialModal(CatMotivosInfraccionModel model)
        {

			int idDependencia = (int)HttpContext.Session.GetInt32("IdDependencia");
			var errors = ModelState.Values.Select(s => s.Errors);
            ModelState.Remove("Nombre");
            if (ModelState.IsValid)
            {


                DeleteMotivo(model);
                var ListMotivosInfraccionModel = _motivoInfraccionService.GetMotivos(idDependencia);
                return Json(ListMotivosInfraccionModel);
            }
            return PartialView("_Eliminar");
        }

        [HttpGet]
        public ActionResult BuscarMotivoByID(int idCatMotivoInfraccion)
        {

			int idDependencia = (int)HttpContext.Session.GetInt32("IdDependencia");
			CatMotivosInfraccionModel motivo = _motivoInfraccionService.GetMotivoByID(idCatMotivoInfraccion, idDependencia);
            return Json(motivo);  
        }

        public JsonResult GetMotInf([DataSourceRequest] DataSourceRequest request)
        {

			int idDependencia = (int)HttpContext.Session.GetInt32("IdDependencia");
			var ListMotivosInfraccionModel = _motivoInfraccionService.GetMotivos(idDependencia);

            return Json(ListMotivosInfraccionModel.ToDataSourceResult(request));
        }




        #endregion


        #region Acciones a base de datos

        public void CreateMotivo(CatMotivosInfraccionModel model)
        {

			int idDependencia = (int)HttpContext.Session.GetInt32("IdDependencia");
			_motivoInfraccionService.CrearMotivo(model, idDependencia);
        }

        public void UpdateMotivo(CatMotivosInfraccionModel model)
        {

			int idDependencia = (int)HttpContext.Session.GetInt32("IdDependencia");
			_motivoInfraccionService.UpdateMotivo(model, idDependencia);

        }

        public void DeleteMotivo(CatMotivosInfraccionModel model)
        {

			_motivoInfraccionService.DeleteMotivo(model);
        }

        /* private void SetDDLColores()
         {
             ///Espacio en memoria de manera temporal que solo existe en la petición bool, list, string ,clases , selectlist
             ViewBag.Categories = new SelectList(dbContext.Color.ToList(), "IdColor", "color");
         }*/

        
        #endregion



    }
}
