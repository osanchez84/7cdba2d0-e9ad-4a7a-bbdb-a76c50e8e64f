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
    public class ColoresController : Controller
    {
        DBContextInssoft dbContext = new DBContextInssoft();
        public IActionResult Index()
        {
            //var products = dbContext.Products.ToList();
            var ListColoresModel = GetColores();

            return View(ListColoresModel);

        }

        /// <summary>
        /// Accion que redirige a la vista
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Create()
        {
            SetDDLColores();
            return View();
        }

        /// <summary>
        /// Accion que recupera los datos de la vista para insertar en BDD
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create(ColoresModel model)
        {
            var errors = ModelState.Values.Select(s => s.Errors);
            ModelState.Remove("color");
            if (ModelState.IsValid)
            {
                //Crear el producto

                CreateColor(model);
                return RedirectToAction("Index");
            }
            SetDDLColores();
            return View("Create");
        }


        [HttpGet]
        public IActionResult Update(int IdColor)
        {
            //aqui con productId debemos Consultar el producto para mostrar los datos actuales en la vista, para que sean modificados
            SetDDLColores();
            var coloresModel = GetColorByID(IdColor);
            return View(coloresModel);
        }


        [HttpPost]
        public IActionResult Update(ColoresModel coloresModel)
        {
            ModelState.Remove("color");
            if (ModelState.IsValid)
            {
                //Modificiacion del registro
                UpdateColor(coloresModel);
                return RedirectToAction("Index");
            }
            SetDDLColores();
            return View("Update");
        }

        [HttpGet]
        public IActionResult Delete(int IdColor)
        {
            //aqui con productId debemos Consultar el producto para mostrar los datos actuales en la vista, para que sean modificados
            SetDDLColores();
            var coloresModel = GetColorByID(IdColor);
            return View(coloresModel);
        }


        [HttpPost]
        public IActionResult Delete(ColoresModel coloresModel)
        {
            ModelState.Remove("CategoryName");
            if (ModelState.IsValid)
            {
                //Modificiacion del registro
                DeleteColor(coloresModel);
                return RedirectToAction("Index");
            }
            SetDDLColores();
            return View("Delete");
        }



        ///Crear metodo de update (post)


        #region Modal Action
        public ActionResult IndexModal()
        {
            var ListColoresModel = GetColores();
            //return View("IndexModal");
            return View("IndexModal", ListColoresModel);
        }

        [HttpPost]
        public ActionResult AgregarPacial()
        {
            //SetDDLDependencias();
            return PartialView("_Create");
        }

        public ActionResult EditarParcial(int Id)
        {
            var coloresModel = GetColorByID(Id);
            return View("_Update",coloresModel); 
        }

        public ActionResult EliminarColorParcial(int IdColor)
        {
            var coloresModel = GetColorByID(IdColor);
            return View("_Eliminar", coloresModel);
        }

        public JsonResult Categories_Read()
        {
            var result = new SelectList(dbContext.Colores.ToList(), "IdColor", "color");
            return Json(result);
        }

      



        [HttpPost]
        public ActionResult CreatePartialModal(ColoresModel model)
        {
            var errors = ModelState.Values.Select(s => s.Errors);
            ModelState.Remove("color");
            if (ModelState.IsValid)
            {
              

                CreateColor(model);
                var ListColoresModel = GetColores();
                return PartialView("_ListaColores", ListColoresModel);
            }
            //SetDDLCategories();
            //return View("Create");
            return PartialView("_Create");
        }

        public ActionResult UpdatePartialModal(ColoresModel model)
        {
            var errors = ModelState.Values.Select(s => s.Errors);
            ModelState.Remove("color");
            if (ModelState.IsValid)
            {


                UpdateColor(model);
                var ListColoresModel = GetColores();
                return PartialView("_ListaColores", ListColoresModel);
            }
            //SetDDLCategories();
            //return View("Create");
            return PartialView("_Update");
        }

        public ActionResult EliminarPartialModal(ColoresModel model)
        {
            var errors = ModelState.Values.Select(s => s.Errors);
            ModelState.Remove("color");
            if (ModelState.IsValid)
            {


                DeleteColor(model);
                var ListColoresModel = GetColores();
                return PartialView("_ListaColores", ListColoresModel);
            }
            //SetDDLCategories();
            //return View("Create");
            return PartialView("_Update");
        }
        public JsonResult GetCols([DataSourceRequest] DataSourceRequest request)
        {
            var ListColoresModel = GetColores();

            return Json(ListColoresModel.ToDataSourceResult(request));
        }




        #endregion


        #region Acciones a base de datos

        public void CreateColor(ColoresModel model)
        {
            Colores color = new Colores();
            color.IdColor = model.IdColor;
            color.color = model.color;
            color.Estatus = 1;
            color.FechaActualizacion = DateTime.Now;
            dbContext.Colores.Add(color);
            dbContext.SaveChanges();
        }

        public void UpdateColor(ColoresModel model)
        {
            Colores color = new Colores();
            color.IdColor = model.IdColor;
            color.color = model.color;
            color.Estatus = 1;
            color.FechaActualizacion = DateTime.Now;
            dbContext.Entry(color).State = EntityState.Modified;
            dbContext.SaveChanges();

        }

        public void DeleteColor(ColoresModel model)
        {
            Colores color = new Colores();
            color.IdColor = model.IdColor;
            color.color = model.color;
            color.Estatus = 0;
            color.FechaActualizacion = DateTime.Now;
            dbContext.Entry(color).State = EntityState.Modified;
            dbContext.SaveChanges();

        }

        private void SetDDLColores()
        {
            ///Espacio en memoria de manera temporal que solo existe en la petición bool, list, string ,clases , selectlist
            ViewBag.Categories = new SelectList(dbContext.Colores.ToList(), "IdColor", "color");
        }


        public ColoresModel GetColorByID(int Id)
        {

            var productEnitity = dbContext.Colores.Find(Id);

            var colorModel = (from colores in dbContext.Colores.ToList()
                                    select new ColoresModel

                                    {
                                        IdColor = colores.IdColor,
                                        color = colores.color,


                                    }).Where(w => w.IdColor == Id).FirstOrDefault();

            return colorModel;
        }

        /// <summary>
        /// Linq es una tecnologia de control de datos (excel, txt,EF,sqlclient etc)
        /// para la gestion un mejor control de la info
        /// </summary>
        /// <returns></returns>
        public List<ColoresModel> GetColores()
        {
            var ListColoresModel = (from colores in dbContext.Colores.ToList()
                                    join estatus in dbContext.Estatus.ToList()
                                    on colores.Estatus equals estatus.estatus
                                    where colores.Estatus == 1
                                    select new ColoresModel
                                         {
                                             IdColor = colores.IdColor,
                                             color = colores.color,
                                             estatusDesc = estatus.estatusDesc

                                         }).ToList();
            return ListColoresModel;
        }
        #endregion



    }
}
