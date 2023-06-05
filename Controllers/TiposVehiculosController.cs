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
    public class TiposVehiculosController : Controller
    {
        DBContextInssoft dbContext = new DBContextInssoft();
        public IActionResult Index()
        {
            //var products = dbContext.Products.ToList();
            var ListTiposVehiculosModel = GetTiposVehiculos();

            return View(ListTiposVehiculosModel);

        }
        [HttpGet]
        public IActionResult Create()
        {
            SetDDLTiposVehiculos();
            return View();
        }

        [HttpPost]
        public IActionResult Create(TiposVehiculosModel model)
        {
            var errors = ModelState.Values.Select(s => s.Errors);
            ModelState.Remove("TipoVehiculo");
            if (ModelState.IsValid)
            {
                //Crear el producto

                CreateTipoVehiculo(model);
                return RedirectToAction("Index");
            }
            SetDDLTiposVehiculos();
            return View("Create");
        }


        [HttpGet]
        public IActionResult Update(int IdTipoVehiculo)
        {
            //aqui con productId debemos Consultar el producto para mostrar los datos actuales en la vista, para que sean modificados
            SetDDLTiposVehiculos();
            var tiposVehiculosModel = GetTipoVehiculoByID(IdTipoVehiculo);
            return View(tiposVehiculosModel);
        }


        [HttpPost]
        public IActionResult Update(TiposVehiculosModel tiposVehiculosModel)
        {
            ModelState.Remove("color");
            if (ModelState.IsValid)
            {
                //Modificiacion del registro
                UpdateTipoVehiculo(tiposVehiculosModel);
                return RedirectToAction("Index");
            }
            SetDDLTiposVehiculos();
            return View("Update");
        }

        [HttpGet]
        public IActionResult Delete(int IdTipoVehiculo)
        {
            //aqui con productId debemos Consultar el producto para mostrar los datos actuales en la vista, para que sean modificados
            SetDDLTiposVehiculos();
            var tiposVehiculosModel = GetTipoVehiculoByID(IdTipoVehiculo);
            return View(tiposVehiculosModel);
        }


        [HttpPost]
        public IActionResult Delete(TiposVehiculosModel tiposVehiculosModel)
        {
            ModelState.Remove("TipoVehiculo");
            if (ModelState.IsValid)
            {
                //Modificiacion del registro
                DeleteTipoVehiculo(tiposVehiculosModel);
                return RedirectToAction("Index");
            }
            SetDDLTiposVehiculos();
            return View("Delete");
        }



        ///Crear metodo de update (post)


        #region Modal Action
        public ActionResult IndexModal()
        {
            var ListTiposVehiculosModel = GetTiposVehiculos();
            //return View("IndexModal");
            return View("IndexModal", ListTiposVehiculosModel);
        }

        [HttpPost]
        public ActionResult AgregarTipoVehiculo()
        {

            //SetDDLDependencias();
            return PartialView("_Create");
        }


        [HttpPost]
        public ActionResult EditarTipoVehiculo(int  IdTipoVehiculo)
        {

            var tiposVehiculosModel = GetTipoVehiculoByID(IdTipoVehiculo);
            return View("_Update",tiposVehiculosModel); 
        }

        [HttpPost]
        public ActionResult EliminarTipoVehiculoParcial(int IdTipoVehiculo)
        {

            var tiposVehiculosModel = GetTipoVehiculoByID(IdTipoVehiculo);
            return View("_Eliminar", tiposVehiculosModel);
        }

        public JsonResult Categories_Read()
        {
            var result = new SelectList(dbContext.TipoVehiculos.ToList(), "IdTipoVehiculo", "TipoVehiculo");
            return Json(result);
        }



        [HttpPost]
        public ActionResult CreatePartialTipoModal(TiposVehiculosModel model)
        {
            var errors = ModelState.Values.Select(s => s.Errors);
            ModelState.Remove("TipoVehiculo");
            if (ModelState.IsValid)
            {
                CreateTipoVehiculo(model);
                var ListTiposVehiculosModel = GetTiposVehiculos();
                return PartialView("_ListaTiposVehiculos", ListTiposVehiculosModel);
            }
            //SetDDLCategories();
            //return View("Create");
            return PartialView("_Create");
        }
       
        public ActionResult UpdatePartialTipoModal(TiposVehiculosModel model)
        {
            var errors = ModelState.Values.Select(s => s.Errors);
            ModelState.Remove("TipoVehiculo");
            if (ModelState.IsValid)
            {
                UpdateTipoVehiculo(model);
                var ListTiposVehiculosModel = GetTiposVehiculos();
                return PartialView("_ListaTiposVehiculos", ListTiposVehiculosModel);
            }
         
            return PartialView("_Update");
        }

        public ActionResult EliminarPartialTipoVehiculoModal(TiposVehiculosModel model)
        {
            var errors = ModelState.Values.Select(s => s.Errors);
            ModelState.Remove("TipoVehiculo");
            if (ModelState.IsValid)
            {
                DeleteTipoVehiculo(model);
                var ListTiposVehiculosModel = GetTiposVehiculos();
                return PartialView("_ListaTiposVehiculos", ListTiposVehiculosModel);
            }
           
            return PartialView("_Update");
        }

        public JsonResult GetTipos([DataSourceRequest] DataSourceRequest request)
        {
            var ListTiposVehiculosModel = GetTiposVehiculos();

            return Json(ListTiposVehiculosModel.ToDataSourceResult(request));
        }




        #endregion


        #region Acciones a base de datos

        public void CreateTipoVehiculo(TiposVehiculosModel model)
        {
            TipoVehiculos tipo = new TipoVehiculos();
            tipo.IdTipoVehiculo = model.IdTipoVehiculo;
            tipo.TipoVehiculo = model.TipoVehiculo;
            tipo.Estatus = 1;
            tipo.FechaActualizacion = DateTime.Now;
            dbContext.TipoVehiculos.Add(tipo);
            dbContext.SaveChanges();
        }

        public void UpdateTipoVehiculo(TiposVehiculosModel model)
        {
            TipoVehiculos tipo = new TipoVehiculos();
            tipo.IdTipoVehiculo = model.IdTipoVehiculo;
            tipo.TipoVehiculo = model.TipoVehiculo;
            tipo.Estatus = 1;
            tipo.FechaActualizacion = DateTime.Now;
            dbContext.Entry(tipo).State = EntityState.Modified;
            dbContext.SaveChanges();

        }

        public void DeleteTipoVehiculo(TiposVehiculosModel model)
        {
            TipoVehiculos tipo = new TipoVehiculos();
            tipo.IdTipoVehiculo = model.IdTipoVehiculo;
            tipo.TipoVehiculo = model.TipoVehiculo;
            tipo.Estatus = 0;
            tipo.FechaActualizacion = DateTime.Now;
            dbContext.Entry(tipo).State = EntityState.Modified;
            dbContext.SaveChanges();

        }

        private void SetDDLTiposVehiculos()
        {
            ViewBag.Categories = new SelectList(dbContext.TipoVehiculos.ToList(), "IdTipoVehiculo" ,"TipoVehiculo");
        }


        public TiposVehiculosModel GetTipoVehiculoByID(int IdTipoVehiculo)
        {

            var productEnitity = dbContext.TipoVehiculos.Find(IdTipoVehiculo);

            var tipoVehiculoModel = (from tiposVehiculo in dbContext.TipoVehiculos.ToList()
                              select new TiposVehiculosModel

                              {
                                  IdTipoVehiculo = tiposVehiculo.IdTipoVehiculo,
                                  TipoVehiculo = tiposVehiculo.TipoVehiculo,


                              }).Where(w => w.IdTipoVehiculo == IdTipoVehiculo).FirstOrDefault();

            return tipoVehiculoModel;
        }

        /// <summary>
        /// Linq es una tecnologia de control de datos (excel, txt,EF,sqlclient etc)
        /// para la gestion un mejor control de la info
        /// </summary>
        /// <returns></returns>
        public List<TiposVehiculosModel> GetTiposVehiculos()
        {
            var ListTiposVehiculosModel = (from tiposVehiculo in dbContext.TipoVehiculos.ToList()
                                           join Estatus in dbContext.Estatus.ToList()
                                           on tiposVehiculo.Estatus equals Estatus.estatus
                                           where tiposVehiculo.Estatus == 1


                                    select new TiposVehiculosModel
                                    {
                                        IdTipoVehiculo = tiposVehiculo.IdTipoVehiculo,
                                        TipoVehiculo = tiposVehiculo.TipoVehiculo,
                                        Estatus =tiposVehiculo.Estatus,
                                        estatusDesc= Estatus.estatusDesc

                                    }).ToList();
            return ListTiposVehiculosModel;
        }
        #endregion



    }
}
