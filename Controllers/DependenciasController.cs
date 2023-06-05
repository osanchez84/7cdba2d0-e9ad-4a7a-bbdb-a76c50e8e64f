using GuanajuatoAdminUsuarios.Entity;
using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Example.WebUI.Controllers
{
    public class DependenciasController : Controller
    {
        DBContextInssoft dbContext = new DBContextInssoft();
        public IActionResult Index()
        {
            //var products = dbContext.Products.ToList();
            var ListDependenciasModel = GetDependencias();

            return View(ListDependenciasModel);

        }

        /// <summary>
        /// Accion que redirige a la vista
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Create()
        {
            SetDDLDependencias();
            return View();
        }

        /// <summary>
        /// Accion que recupera los datos de la vista para insertar en BDD
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create(DependenciasModel model)
        {
            var errors = ModelState.Values.Select(s => s.Errors);
            ModelState.Remove("NombreDependencia");
            if (ModelState.IsValid)
            {
                //Crear el producto

                CreateDependencia(model);
                return RedirectToAction("Index");
            }
            SetDDLDependencias();
            return View("Create");
        }


        [HttpGet]
        public IActionResult Update(int IdDependencia)
        {
            //aqui con productId debemos Consultar el producto para mostrar los datos actuales en la vista, para que sean modificados
            SetDDLDependencias();
            var dependenciasModel = GetDependenciaByID(IdDependencia);
            return View(dependenciasModel);
        }


        [HttpPost]
        public IActionResult Update(DependenciasModel dependenciasModel)
        {
            ModelState.Remove("CategoryName");
            if (ModelState.IsValid)
            {
                //Modificiacion del registro
                UpdateDependencia(dependenciasModel);
                return RedirectToAction("Index");
            }
            SetDDLDependencias();
            return View("Update");
        }

        [HttpGet]
        public IActionResult Delete(int IdDependencia)
        {
            //aqui con productId debemos Consultar el producto para mostrar los datos actuales en la vista, para que sean modificados
            SetDDLDependencias();
            var dependenciasModel = GetDependenciaByID(IdDependencia);
            return View(dependenciasModel);
        }


        [HttpPost]
        public IActionResult Delete(DependenciasModel dependenciasModel)
        {
            ModelState.Remove("CategoryName");
            if (ModelState.IsValid)
            {
                //Modificiacion del registro
                DeleteProduct(dependenciasModel);
                return RedirectToAction("Index");
            }
            SetDDLDependencias();
            return View("Delete");
        }



        ///Crear metodo de update (post)


        #region Modal Action
        public ActionResult IndexModal()
        {
            var ListDependenciasModel = GetDependencias();
            //return View("IndexModal");
            return View("IndexModal", ListDependenciasModel);
        }

        [HttpPost]
        public ActionResult AgregarPacial()
        {
            //SetDDLDependencias();
            return PartialView("_Create");
        }

        [HttpPost]
        public ActionResult EditarParcial(int IdDependencia)
        {
            var dependenciasModel = GetDependenciaByID(IdDependencia); 
            return PartialView("_Update", dependenciasModel);
        }

        

        public JsonResult Categories_Read()
        {
            var result = new SelectList(dbContext.Dependencias.ToList(), "IdDependencia", "NombreDependencia");
            return Json(result);
        }



        [HttpPost]
        public ActionResult CreatePartialModal(DependenciasModel model)
        {
            var errors = ModelState.Values.Select(s => s.Errors);
            ModelState.Remove("NombreDependencia");
            if (ModelState.IsValid)
            {
                //Crear el producto

                CreateDependencia(model);
                var ListDependenciasModel = GetDependencias();
                return PartialView("_ListaDependencias", ListDependenciasModel);
            }
            //SetDDLCategories();
            //return View("Create");
            return PartialView("_Create");
        }

        [HttpPost]
        public ActionResult UpdatePartialModal(DependenciasModel model)
        {
            var errors = ModelState.Values.Select(s => s.Errors);
            ModelState.Remove("NombreDependencia");
            if (ModelState.IsValid)
            {
                //Crear el producto

                UpdateDependencia(model);
                var ListDependenciasModel = GetDependencias();
                return PartialView("_ListaDependencias", ListDependenciasModel);
            }
            //SetDDLCategories();
            //return View("Create");
            return PartialView("_Create");
        }

        public JsonResult GetDeps([DataSourceRequest] DataSourceRequest request)
        {
            var ListProuctModel = GetDependencias();

            return Json(ListProuctModel.ToDataSourceResult(request));
        }




        #endregion


        #region Acciones a base de datos

        public void CreateDependencia(DependenciasModel model)
        {
            Dependencias dependencia = new Dependencias();
            dependencia.IdDependencia = model.IdDependencia;
            dependencia.NombreDependencia = model.NombreDependencia;
            dependencia.Estatus = 1;
            dbContext.Dependencias.Add(dependencia);
            dbContext.SaveChanges();
        }

        public void UpdateDependencia(DependenciasModel model)
        {
            //Sera mas rapido con automapeo de clases
            Dependencias dependencia = new Dependencias();
            dependencia.IdDependencia = model.IdDependencia;
            dependencia.NombreDependencia = model.NombreDependencia;       
            //Lineas de codigo para la modificacion del producto con EF
            dbContext.Entry(dependencia).State = EntityState.Modified;
            dbContext.SaveChanges();

        }

        public void DeleteProduct(DependenciasModel model)
        {
            //Sera mas rapido con automapeo de clases
            Dependencias dependencia = new Dependencias();
            dependencia.IdDependencia = model.IdDependencia;
            dependencia.NombreDependencia = model.NombreDependencia;

            dbContext.Dependencias.Remove(dependencia);
            dbContext.SaveChanges();

        }

        private void SetDDLDependencias()
        {
            ///Espacio en memoria de manera temporal que solo existe en la petición bool, list, string ,clases , selectlist
            ViewBag.Categories = new SelectList(dbContext.Dependencias.ToList(), "CategoryId", "CategoryName");
        }


        public DependenciasModel GetDependenciaByID(int IdDependencia)
        {

            var productEnitity = dbContext.Dependencias.Find(IdDependencia);

            var dependenciaModel = (from dependencias in dbContext.Dependencias.ToList()
                                    select new DependenciasModel

                                    {
                                    IdDependencia = dependencias.IdDependencia,
                                    NombreDependencia = dependencias.NombreDependencia,
                                   

                                }).Where(w => w.IdDependencia == IdDependencia).FirstOrDefault();

            return dependenciaModel;
        }

        /// <summary>
        /// Linq es una tecnologia de control de datos (excel, txt,EF,sqlclient etc)
        /// para la gestion un mejor control de la info
        /// </summary>
        /// <returns></returns>
        public List<DependenciasModel> GetDependencias()
        {
            var ListDependenciasModel = (from dependencias in dbContext.Dependencias.ToList()

                                         select new DependenciasModel
                                         {
                                             IdDependencia = dependencias.IdDependencia,
                                             NombreDependencia = dependencias.NombreDependencia,

                                         }).ToList();
            return ListDependenciasModel;
        }
        #endregion



    }
}
