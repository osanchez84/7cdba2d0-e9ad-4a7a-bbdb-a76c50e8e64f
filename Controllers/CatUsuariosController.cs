using GuanajuatoAdminUsuarios.Data;
using GuanajuatoAdminUsuarios.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GuanajuatoAdminUsuarios.Controllers
{
    [Authorize]
    public class CatUsuariosController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        Usuarios _Catalogos = new Usuarios();
        private IConfiguration configuration;


        public CatUsuariosController(ILogger<HomeController> logger, IConfiguration conf)
        {
            _logger = logger;
            configuration = conf;
        }


        public IActionResult UsuariosIndex()
        {

            return View("../Catalogos/Usuarios/Index");
        }


        public ActionResult GetUsuarios([DataSourceRequest] DataSourceRequest request)
        {
            List<Usuario> listUsuarios = new List<Usuario>();

             listUsuarios = _Catalogos.ListarUsuarios();


            DataSourceResult gridUsuarios = listUsuarios.ToDataSourceResult(request, cm => new
            {
                IdUsuario = cm.IdUsuario,
                Nombre = cm.Nombre,
                Paterno = cm.Paterno,
                Materno = cm.Materno,
                Estatus = cm.Estatus


            });
            return Json(gridUsuarios);
        }

        public IActionResult CreateUsuario()
        {
            //redirecciona si no hay sesion
            if (((string)HttpContext.Session.GetString("idUsuario") ?? "0") == "0")
            {
              //  HttpContext.SignOutAsync();
                return RedirectToAction("Index", "Home", new { @id = 0 });
            }
          
            //   ViewBag.menu = menu.Menu(HttpContext);
            return View("../Catalogos/Usuarios/Create");
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUsuario([Bind("Nombre,Paterno,Materno,Email,Login,Clave")] Usuario usuario)
        {
           
            if (ModelState.IsValid)
            {
                var usuarioSesion = (string)HttpContext.Session.GetString("idUsuario") ?? "0";
                //redirecciona si no hay sesion
                if (usuarioSesion == "0")
                {
                   // await HttpContext.SignOutAsync();
                    return RedirectToAction("Index", "Home", new { @id = 0 });
                }

                String resultado = _Catalogos.GuardaUsuario(usuario.Nombre, usuario.Paterno, usuario.Materno, usuario.Email,
                                                            usuario.Login, usuario.Clave, usuarioSesion);


                return View("../Catalogos/Usuarios/Index");
            }
            return View("../Catalogos/Usuarios/Create", usuario);
           
        }


        public async Task<IActionResult> EditUsuario(int? id)
        {
            //redirecciona si no hay sesion
            if (((string)HttpContext.Session.GetString("idUsuario") ?? "0") == "0")
            {
             //   await HttpContext.SignOutAsync();
                return RedirectToAction("Index", "Home", new { @id = 0 });
            }

            if (id == null)
            {
                return NotFound();
            }

            var CatUsuario = _Catalogos.GetUsuario(id);
      
            return View("../Catalogos/Usuarios/Edit", CatUsuario);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUsuario( [Bind("IdUsuario,Nombre,Paterno,Materno,Email,Estatus,Login")] Usuario usuario)
        {
            //redirecciona si no hay sesion
            if (((string)HttpContext.Session.GetString("idUsuario") ?? "0") == "0")
            {
                //   await HttpContext.SignOutAsync();
                return RedirectToAction("Index", "Home", new { @id = 0 });
            }
            var usuarioSesion = (string)HttpContext.Session.GetString("idUsuario") ?? "0";


            var CatUsuario = _Catalogos.ActualizaUsuario(usuario.IdUsuario, usuario.Nombre, usuario.Paterno, usuario.Materno, usuario.Email,
                                                            usuario.Login, usuario.Estatus, usuarioSesion);

            return View("../Catalogos/Usuarios/Index");
        }


        public async Task<IActionResult> DetailsUsuario(int? id)
        {
            //redirecciona si no hay sesion
            if (((string)HttpContext.Session.GetString("idUsuario") ?? "0") == "0")
            {
                //   await HttpContext.SignOutAsync();
                return RedirectToAction("Index", "Home", new { @id = 0 });
            }

            if (id == null)
            {
                return NotFound();
            }

            var CatUsuario = _Catalogos.GetUsuario(id);

            return View("../Catalogos/Usuarios/Details", CatUsuario);
        }




    }
}
