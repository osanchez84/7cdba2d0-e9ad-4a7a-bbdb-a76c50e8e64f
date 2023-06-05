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
    public class TiposCargaController : Controller
    {
        DBContextInssoft dbContext = new DBContextInssoft();
        public IActionResult Index()
        {
            //var products = dbContext.Products.ToList();
            var ListTiposCargaModel = GetTiposCarga();

            return View(ListTiposCargaModel);

        }

        /// <summary>
        /// Accion que redirige a la vista
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Create()
        {
           // SetDDLColores();
            return View();
        }

        /// <summary>
        /// Accion que recupera los datos de la vista para insertar en BDD
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create(TiposCargaModel model)
        {
            var errors = ModelState.Values.Select(s => s.Errors);
            ModelState.Remove("TipoCarga");
            if (ModelState.IsValid)
            {
                //Crear el producto

                CreateTipoCarga(model);
                return RedirectToAction("Index");
            }
            //SetDDLColores();
            return View("Create");
        }


        [HttpGet]
        public IActionResult Update(int IdTipoCarga)
        {
            //aqui con productId debemos Consultar el producto para mostrar los datos actuales en la vista, para que sean modificados
            SetDDLTiposCarga();
            var tiposCargaModel = GetTipoCargaByID(IdTipoCarga);
            return View(tiposCargaModel);
        }


        [HttpPost]
        public IActionResult Update(TiposCargaModel tiposCargaModel)
        {
            ModelState.Remove("TipoCarga");
            if (ModelState.IsValid)
            {
                //Modificiacion del registro
                UpdateTipoCarga(tiposCargaModel);
                return RedirectToAction("Index");
            }
            SetDDLTiposCarga();
            return View("Update");
        }

        [HttpGet]
        public IActionResult Delete(int IdTipoCarga)
        {
            //aqui con productId debemos Consultar el producto para mostrar los datos actuales en la vista, para que sean modificados
            SetDDLTiposCarga();
            var tiposCargaModel = GetTipoCargaByID(IdTipoCarga);
            return View(tiposCargaModel);
        }


        [HttpPost]
        public IActionResult Delete(TiposCargaModel tiposCargaModel)
        {
            ModelState.Remove("TipoCarga");
            if (ModelState.IsValid)
            {
                //Modificiacion del registro
                DeleteTipoCarga(tiposCargaModel);
                return RedirectToAction("Index");
            }
            SetDDLTiposCarga();
            return View("Delete");
        }



        ///Crear metodo de update (post)


        #region Modal Action
        public ActionResult IndexModal()
        {
            var ListTiposCargaModel = GetTiposCarga();
            //return View("IndexModal");
            return View("IndexModal", ListTiposCargaModel);
        }

        [HttpPost]
        public ActionResult AgregarTipoCargaParcial()
        {
            //SetDDLDependencias();
            return PartialView("_Create");
        }

        [HttpPost]
        public ActionResult EditarParcial(int IdTipoCarga)
        {
            var tiposCargaModel = GetTipoCargaByID(IdTipoCarga);
            return View("_Editar",tiposCargaModel);
        }

        [HttpPost]
        public ActionResult EliminarTipoCargaParcial(int IdTipoCarga)
        {
            var tiposCargaModel = GetTipoCargaByID(IdTipoCarga);
            return View("_Eliminar", tiposCargaModel);
        }
        public JsonResult Categories_Read()
        {
            var result = new SelectList(dbContext.TiposCarga.ToList(), "IdTipoCarga", "TipoCarga");
            return Json(result);
        }



        [HttpPost]
        public ActionResult CreatePartialModal(TiposCargaModel model)
        {
            var errors = ModelState.Values.Select(s => s.Errors);
            ModelState.Remove("TipoCarga");
            if (ModelState.IsValid)
            {


                CreateTipoCarga(model);
                var ListTiposCargaModel = GetTiposCarga();
                return PartialView("_ListaTiposCarga", ListTiposCargaModel);
            }
            //SetDDLCategories();
            //return View("Create");
            return PartialView("_Create");
        }

        [HttpPost]
        public ActionResult EditarParcialModal(TiposCargaModel model)
        {
            var errors = ModelState.Values.Select(s => s.Errors);
            ModelState.Remove("TipoCarga");
            if (ModelState.IsValid)
            {


                UpdateTipoCarga(model);
                var ListTiposCargaModel = GetTiposCarga();
                return PartialView("_ListaTiposCarga", ListTiposCargaModel);
            }
         
            return PartialView("_Create");
        }

        [HttpPost]
        public ActionResult EditarTipoCargaParcialModal(TiposCargaModel model)
        {
            var errors = ModelState.Values.Select(s => s.Errors);
            ModelState.Remove("TipoCarga");
            if (ModelState.IsValid)
            {


                DeleteTipoCarga(model);
                var ListTiposCargaModel = GetTiposCarga();
                return PartialView("_ListaTiposCarga", ListTiposCargaModel);
            }
           
            return PartialView("_Eliminar");
        }

        public JsonResult GetTipos([DataSourceRequest] DataSourceRequest request)
        {
            var ListTiposCargaModel = GetTiposCarga();

            return Json(ListTiposCargaModel.ToDataSourceResult(request));
        }




        #endregion


        #region Acciones a base de datos

        public void CreateTipoCarga(TiposCargaModel model)
        {
            TiposCarga tipo = new TiposCarga();
            tipo.IdTipoCarga = model.IdTipoCarga;
            tipo.TipoCarga = model.TipoCarga;
            tipo.Estatus = 1;
            tipo.FechaActualizacion = DateTime.Now;
            dbContext.TiposCarga.Add(tipo);
            dbContext.SaveChanges();
        }
         
        public void UpdateTipoCarga(TiposCargaModel model)
        {
            TiposCarga tipo = new TiposCarga();
            tipo.IdTipoCarga = model.IdTipoCarga;
            tipo.TipoCarga = model.TipoCarga;
            tipo.Estatus = 1;
            tipo.FechaActualizacion = DateTime.Now;
            dbContext.Entry(tipo).State = EntityState.Modified;
            dbContext.SaveChanges();

        }

        public void DeleteTipoCarga(TiposCargaModel model)
        {
            TiposCarga tipo = new TiposCarga();
            tipo.IdTipoCarga = model.IdTipoCarga;
            tipo.TipoCarga = model.TipoCarga;
            tipo.Estatus = 0;
            tipo.FechaActualizacion = DateTime.Now;
            dbContext.Entry(tipo).State = EntityState.Modified;
            dbContext.SaveChanges();

        }

        private void SetDDLTiposCarga()
        {
            ///Espacio en memoria de manera temporal que solo existe en la petición bool, list, string ,clases , selectlist
            ViewBag.Tiposcarga = new SelectList(dbContext.TiposCarga.ToList(), "IdTipoCarga", "TipoCarga");
        }


        public TiposCargaModel GetTipoCargaByID(int IdTipoCarga)
        {

            var productEnitity = dbContext.TiposCarga.Find(IdTipoCarga);

            var tipoCargaModel = (from tiposCarga in dbContext.TiposCarga.ToList()
                              select new TiposCargaModel

                              {
                                  IdTipoCarga = tiposCarga.IdTipoCarga,
                                  TipoCarga = tiposCarga.TipoCarga,


                              }).Where(w => w.IdTipoCarga == IdTipoCarga).FirstOrDefault();

            return tipoCargaModel;
        }

        /// <summary>
        /// Linq es una tecnologia de control de datos (excel, txt,EF,sqlclient etc)
        /// para la gestion un mejor control de la info
        /// </summary>
        /// <returns></returns>
        public List<TiposCargaModel> GetTiposCarga()
        {
            var ListTiposcargaModel = (from tiposCarga in dbContext.TiposCarga.ToList()
                                       join estatus in dbContext.Estatus.ToList()
                                       on tiposCarga.Estatus equals estatus.estatus
                                       where tiposCarga.Estatus == 1

                                    select new TiposCargaModel
                                    {
                                        IdTipoCarga = tiposCarga.IdTipoCarga,
                                        TipoCarga = tiposCarga.TipoCarga,
                                        Estatus = tiposCarga.Estatus,
                                        EstatusDesc = estatus.estatusDesc

                                    }).ToList();
            return ListTiposcargaModel;
        }
        #endregion



    }
}
