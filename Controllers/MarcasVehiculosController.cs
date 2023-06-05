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
    public class MarcasVehiculosController : Controller
    {
        DBContextInssoft dbContext = new DBContextInssoft();
        public IActionResult Index()
        {
            //var products = dbContext.Products.ToList();
            var ListMarcasModel = GetMarcas();

            return View(ListMarcasModel);

        }


        #region Modal Action
        public ActionResult IndexModal()
        {
            var ListMarcasModel = GetMarcas();
            //return View("IndexModal");
            return View("Index", ListMarcasModel);
        }

        [HttpPost]
        public ActionResult AgregarPacial()
        {
            //SetDDLDependencias();
            return PartialView("_Crear");
        }

        [HttpPost]
        public ActionResult EditarParcial(int IdMarcaVehiculo)
        {
            var marcasVehiculosModel = GetMarcaByID(IdMarcaVehiculo);
            return View("_Editar", marcasVehiculosModel);
        }
        [HttpPost]
        public ActionResult EliminarMarcaParcial(int IdMarcaVehiculo)
        {
            var marcasVehiculosModel = GetMarcaByID(IdMarcaVehiculo);
            return View("_Eliminar", marcasVehiculosModel);
        }

        [HttpPost]
        public IActionResult GetUpdate(int IdMarcaVehiculo)
        {
            var marcaVehiculoModel = GetMarcaByID(IdMarcaVehiculo);
            return View(marcaVehiculoModel);
        }


        public JsonResult Categories_Read()
        {
            var result = new SelectList(dbContext.MarcasVehiculos.ToList(), "IdMarcaVehiculo", "MarcaVehiculo");
            return Json(result);
        }



        [HttpPost]
        public ActionResult CreatePartialModal(MarcaVehiculoModel model)
        {
            var errors = ModelState.Values.Select(s => s.Errors);
            ModelState.Remove("MarcaVehiculo");
            if (ModelState.IsValid)
            {
                //Crear el producto

                CreateMarca(model);
                var ListMarcasModel = GetMarcas();
                return PartialView("_ListaMarcasVehiculos", ListMarcasModel);
            }
            //SetDDLCategories();
            //return View("Create");
            return PartialView("_Crear");
        }

        public ActionResult UpdatePartialModal(MarcaVehiculoModel model)
        {
            bool switchMarcas = Request.Form["MarcasSwitch"].Contains("true");
            model.Estatus = switchMarcas ? 1 : 0;
            var errors = ModelState.Values.Select(s => s.Errors);
            ModelState.Remove("MarcaVehiculo");
            if (ModelState.IsValid)
            {
                //Crear el producto

                UpdateMarca(model);
                var ListMarcasModel = GetMarcas();
                return PartialView("_ListaMarcasVehiculos", ListMarcasModel);
            }
            //SetDDLCategories();
            //return View("Create");
            return PartialView("_Editar");
        }

        public ActionResult EliminarMarcaModal(MarcaVehiculoModel model)
        {
            var errors = ModelState.Values.Select(s => s.Errors);
            ModelState.Remove("MarcaVehiculo");
            if (ModelState.IsValid)
            {

                DeleteMarca(model);
                var ListMarcasModel = GetMarcas();
                return PartialView("_ListaMarcasVehiculos", ListMarcasModel);
            }
            //SetDDLCategories();
            //return View("Create");
            return PartialView("_Eliminar");
        }



        public JsonResult GetMarca2([DataSourceRequest] DataSourceRequest request)
        {
            var ListMarcasModel = GetMarcas();

            return Json(ListMarcasModel.ToDataSourceResult(request));
        }




        #endregion


        #region Acciones a base de datos

        public void CreateMarca(MarcaVehiculoModel model)
        {
            MarcasVehiculo marca = new MarcasVehiculo();
            marca.IdMarcaVehiculo = model.IdMarcaVehiculo;
            marca.MarcaVehiculo = model.MarcaVehiculo;
            marca.Estatus = 1;
            marca.FechaActualizacion = DateTime.Now;
            dbContext.MarcasVehiculos.Add(marca);
            dbContext.SaveChanges();
        }

        public void UpdateMarca(MarcaVehiculoModel model)
        {
            MarcasVehiculo marca = new MarcasVehiculo();
            marca.IdMarcaVehiculo = model.IdMarcaVehiculo;
            marca.MarcaVehiculo = model.MarcaVehiculo;
            marca.Estatus = model.Estatus;
            marca.FechaActualizacion = DateTime.Now;
            dbContext.Entry(marca).State = EntityState.Modified;
            dbContext.SaveChanges();

        }

        public void DeleteMarca(MarcaVehiculoModel model)
        {
            //Sera mas rapido con automapeo de clases
            MarcasVehiculo marca = new MarcasVehiculo();
            marca.IdMarcaVehiculo = model.IdMarcaVehiculo;
            marca.MarcaVehiculo = model.MarcaVehiculo;
            marca.Estatus = 0;
            marca.FechaActualizacion = DateTime.Now;
            dbContext.Entry(marca).State = EntityState.Modified;
            dbContext.SaveChanges();

        }

        private void SetDDLMarcas()
        {
            ///Espacio en memoria de manera temporal que solo existe en la petición bool, list, string ,clases , selectlist
            ViewBag.Categories = new SelectList(dbContext.MarcasVehiculos.ToList(), "IdMarcaVehiculo", "MarcaVehiculo");
        }


        public MarcaVehiculoModel GetMarcaByID(int IdMarcaVehiculo)
        {

            var productEnitity = dbContext.Dependencias.Find(IdMarcaVehiculo);

            var marcaModel = (from marcasVehiculos in dbContext.MarcasVehiculos.ToList()
                              select new MarcaVehiculoModel

                              {
                                  IdMarcaVehiculo = marcasVehiculos.IdMarcaVehiculo,
                                  MarcaVehiculo = marcasVehiculos.MarcaVehiculo,


                              }).Where(w => w.IdMarcaVehiculo == IdMarcaVehiculo).FirstOrDefault();

            return marcaModel;
        }

        /// <summary>
        /// Linq es una tecnologia de control de datos (excel, txt,EF,sqlclient etc)
        /// para la gestion un mejor control de la info
        /// </summary>
        /// <returns></returns>
        public List<MarcaVehiculoModel> GetMarcas()
        {
            var ListMarcasModel = (from marcasVehiculos in dbContext.MarcasVehiculos.ToList()
                                   join estatus in dbContext.Estatus.ToList()
                                    on marcasVehiculos.Estatus equals estatus.estatus
                                   where marcasVehiculos.Estatus == 1

                                   select new MarcaVehiculoModel
                                   {
                                       IdMarcaVehiculo = marcasVehiculos.IdMarcaVehiculo,
                                       MarcaVehiculo = marcasVehiculos.MarcaVehiculo,
                                       Estatus = marcasVehiculos.Estatus,
                                       estatusDesc = estatus.estatusDesc

                                   }).ToList();
            return ListMarcasModel;
        }
        #endregion



    }
}
