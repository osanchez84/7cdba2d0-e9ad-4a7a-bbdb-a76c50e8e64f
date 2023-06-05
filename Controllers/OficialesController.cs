using GuanajuatoAdminUsuarios.Entity;
using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using GuanajuatoAdminUsuarios.Services;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Example.WebUI.Controllers
{
    public class OficialesController : Controller
    {
        DBContextInssoft dbContext = new DBContextInssoft();
        public IActionResult Index()
        {
            //var products = dbContext.Products.ToList();
            var ListOficialesModel = GetOficiales();

            return View(ListOficialesModel);

        }
        private readonly IOficiales _oficialesService;

        public OficialesController(IOficiales oficialesService)
        {
            _oficialesService = oficialesService;
        }
        /// <summary>
        /// Accion que redirige a la vista
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Create()
        {
            SetDDLOficiales();
            return View();
        }

        /// <summary>
        /// Accion que recupera los datos de la vista para insertar en BDD
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create(OficialesModel model)
        {
            var errors = ModelState.Values.Select(s => s.Errors);
             
            ModelState.Remove("Nombre");
            if (ModelState.IsValid)
            {
                //Crear el producto

                CreateOficial(model);
                return RedirectToAction("Index");
            }
            SetDDLOficiales();
            return View("Create");
        }


        [HttpGet]
        public IActionResult Update(int IdOficial)
        {
            //aqui con productId debemos Consultar el producto para mostrar los datos actuales en la vista, para que sean modificados
            SetDDLOficiales();
            var oficialesModel = GetOficialByID(IdOficial);
            return View(oficialesModel);
        }


        [HttpPost]
        public IActionResult Update(OficialesModel oficialesModel)
        {
            ModelState.Remove("Nombre");
            if (ModelState.IsValid)
            {
                //Modificiacion del registro
                UpdateOficial(oficialesModel);
                return RedirectToAction("Index");
            }
            SetDDLOficiales();
            return View("Update");
        }

        [HttpGet]
        public IActionResult Delete(int IdOficial)
        {
            //aqui con productId debemos Consultar el producto para mostrar los datos actuales en la vista, para que sean modificados
            SetDDLOficiales();
            var oficialesModel = GetOficialByID(IdOficial);
            return View(oficialesModel);
        }


        [HttpPost]
        public IActionResult Delete(OficialesModel oficialesModel)
        {
            ModelState.Remove("Nombre");
            if (ModelState.IsValid)
            {
                //Modificiacion del registro
                DeleteOficial(oficialesModel);
                return RedirectToAction("Index");
            }
            SetDDLOficiales();
            return View("Delete");
        }



        ///Crear metodo de update (post)


        #region Modal Action
        public ActionResult IndexModal()
        {
            var ListOficialessModel = GetOficiales();
            //return View("IndexModal");
            return View("IndexModal", ListOficialessModel);
        }

        [HttpPost]
        public ActionResult AgregarParcial()
        {
            //SetDDLDependencias();
            return PartialView("_Create");
        }

        public JsonResult Categories_Read()
        {
            var result = new SelectList(dbContext.Oficiales.ToList(), "IdOficial", "Nombre");
            return Json(result);
        }



        [HttpPost]
        public ActionResult CreatePartialModal(OficialesModel model)
        {
            var errors = ModelState.Values.Select(s => s.Errors);
            ModelState.Remove("Nombre");
            if (ModelState.IsValid)
            {
                //Crear el producto

                CreateOficial(model);
                var ListOficialesModel = GetOficiales();
                return PartialView("_ListaOficiales", ListOficialesModel);
            }
            //SetDDLCategories();
            //return View("Create");
            return PartialView("_Create");
        }

        public JsonResult GetOficialess([DataSourceRequest] DataSourceRequest request)
        {
            var ListOficialesModel = GetOficiales();

            return Json(ListOficialesModel.ToDataSourceResult(request));
        }




        #endregion


        #region Acciones a base de datos

        public void CreateOficial(OficialesModel model)
        {
            Oficiales oficial = new Oficiales();
            oficial.IdOficial = model.IdOficial;
            oficial.Rango = model.Rango;
            oficial.Nombre = model.Nombre;
            oficial.ApellidoPaterno = model.ApellidoPaterno;
            oficial.ApellidoMaterno = model.ApellidoMaterno;
            oficial.IdDelegacion = model.IdDelegacion;

            dbContext.Oficiales.Add(oficial);
            dbContext.SaveChanges();
        }

        public void UpdateOficial(OficialesModel model)
        {
            Oficiales oficial = new Oficiales();
            oficial.IdOficial = model.IdOficial;
            oficial.Rango = model.Rango;
            oficial.Nombre = model.Nombre;
            oficial.ApellidoPaterno = model.ApellidoPaterno;
            oficial.ApellidoMaterno = model.ApellidoMaterno;
            dbContext.Entry(oficial).State = EntityState.Modified;
            dbContext.SaveChanges();

        }

        public void DeleteOficial(OficialesModel model)
        {
            Oficiales oficial = new Oficiales();
            oficial.IdOficial = model.IdOficial;
            oficial.Rango = model.Rango;
            oficial.Nombre = model.Nombre;
            oficial.ApellidoPaterno = model.ApellidoPaterno;
            oficial.ApellidoMaterno = model.ApellidoMaterno;
            dbContext.Oficiales.Remove(oficial);
            dbContext.SaveChanges();

        }

        private void SetDDLOficiales()
        {
            ///Espacio en memoria de manera temporal que solo existe en la petición bool, list, string ,clases , selectlist
            ViewBag.Delegaciones = new SelectList(dbContext.Delegaciones.ToList(), "IdDelegacion", "Delegacion");
        }

       
      

        public OficialesModel GetOficialByID(int IdOficial)
        {

            var productEnitity = dbContext.Oficiales.Find(IdOficial);

            var oficialModel = (from oficiales in dbContext.Oficiales.ToList()
                                 select new OficialesModel

                                 {
                                     IdOficial = oficiales.IdOficial,
                                     Rango = oficiales.Rango,
                                     Nombre = oficiales.Nombre,
                                     ApellidoPaterno = oficiales.ApellidoPaterno,
                                     ApellidoMaterno = oficiales.ApellidoMaterno,



                                 }).Where(w => w.IdOficial == IdOficial).FirstOrDefault();

            return oficialModel;
        }

        /// <summary>
        /// Linq es una tecnologia de control de datos (excel, txt,EF,sqlclient etc)
        /// para la gestion un mejor control de la info
        /// </summary>
        /// <returns></returns>
        public List<OficialesModel> GetOficiales()
        {
            var ListOficialessModel = (from oficiales in dbContext.Oficiales.ToList()
                                       join Delegaciones in dbContext.Delegaciones.ToList()
                                       on oficiales.IdDelegacion equals Delegaciones.IdDelegacion

                                       select new OficialesModel
                                      {
                                          IdOficial = oficiales.IdOficial,
                                          Rango = oficiales.Rango,
                                          Nombre = oficiales.Nombre,
                                          ApellidoPaterno = oficiales.ApellidoPaterno,
                                          ApellidoMaterno = oficiales.ApellidoMaterno,
                                          Delegacion= Delegaciones.Delegacion,

                                      }).ToList();
            return ListOficialessModel;
        }
        #endregion



    }
}
