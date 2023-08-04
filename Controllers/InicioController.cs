using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuanajuatoAdminUsuarios.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Http;
using AdminUsuarios.Models.Commons;
using AdminUsuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using AdminUsuarios.Helpers;
using ReciboPago;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Runtime.Serialization.DataContracts;
using Org.BouncyCastle.Crypto.IO;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Text;
using System.Xml;
using Telerik.SvgIcons;
using System.Net.Http;

namespace GuanajuatoAdminUsuarios.Controllers
{
    [Route("")]
    public class InicioController : Controller
    {

        private readonly ILogger<InicioController> _logger;


        public InicioController(ILogger<InicioController> logger)
        {
            _logger = logger;

        }

        //public RecibosPagoWSClient client { get; set; }

        [HttpGet("Inicio")]
        [Route("")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            //WebClient();
            //HttpClientCustome();
            //reversaDePagoRequest test = new reversaDePagoRequest();
            //test.FechaReversa = Convert.ToString(DateTime.Now);
            //test.UsuarioLog = "Ruben";
            //test.PasswordLog = "Password";
            //test.ReciboControlInterno = "1234";

            //RecibosPagoWSClient client = new RecibosPagoWSClient();

            //var response = getResponse(client,test);
            //response.Wait();
            //client.CloseAsync();
            //await client.CloseAsync();



            if (User.Identity.IsAuthenticated)
                return View("Inicio");
            return View("Marca");
        }
      
        //private void HttpClientCustome()
        //{
        //    reversaDePagoRequest test = new reversaDePagoRequest();
        //    test.FechaReversa = Convert.ToString(DateTime.Now);
        //    test.UsuarioLog = "Ruben";
        //    test.PasswordLog = "Password";
        //    test.ReciboControlInterno = "1234";

        //    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(test.GetType());
        //    var algoo= x.Serialize(test);


        //    HttpClient httpClient = new HttpClient();
        //    httpClient.DefaultRequestHeaders.Add("SOAPAction", "http://spenlinea.guanajuato.gob.mx:8080/SittegWS/RecibosPagoWS/reversaDePago");
        //    //string soapEnvelope = "<s:Envelope xmlns:s= \"http://schemas.xmlsoap.org/soap/envelope/\"><s:Body><Mymethod_Async xmlns=\"http://tempuri.org/\"/></s:Body></s:Envelope>";
        //    var content = new StringContent(test, Encoding.UTF8, "text/xml");
        //    HttpResponseMessage hrm = httpClient.PostAsync("http://spenlinea.guanajuato.gob.mx:8080/SittegWS/RecibosPagoWS?wsdl", content).Result;
        //    var result = hrm.Content.ReadAsStream();
        //    StreamReader reader = new StreamReader(result);
        //    string text = reader.ReadToEnd();
        //}


        public string ToXML()
        {
            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(stringwriter, this);
                return stringwriter.ToString();
            }
        }

        public static reversaDePagoRequest LoadFromXMLString(string xmlText)
        {
            using (var stringReader = new System.IO.StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(reversaDePagoRequest));
                return serializer.Deserialize(stringReader) as reversaDePagoRequest;
            }
        }

        private void WebClient()
        {
            reversaDePagoRequest test = new reversaDePagoRequest();
            test.FechaReversa = Convert.ToString(DateTime.Now);
            test.UsuarioLog = "Ruben";
            test.PasswordLog = "Password";
            test.ReciboControlInterno = "1234";

            //var uri = "http://spenlinea.guanajuato.gob.mx:8080/SittegWS/RecibosPagoWS?wsdl";
            var uri = "http://spenlinea.guanajuato.gob.mx:8080/SittegWS/RecibosPagoWS?wsdl/";
            WebClient proxy = new WebClient();
            string serviceURL = string.Format(uri, test);
            byte[] data = proxy.DownloadData(serviceURL);
            Stream stream = new MemoryStream(data);


            //XmlDataContractSerializerOutputFormatter  obj = new XmlDataContractSerializerOutputFormatter(typeof(respuestaObj));
            //XmlDataContractSerializerInputFormatter xmlData= new XmlDataContractSerializerInputFormatter();


            string myNamespace = "http://modificacion.recibos.sittegws/";
         

           
            // convert stream to string
            StreamReader reader = new StreamReader(stream);
            string text = reader.ReadToEnd();


            stream.Position = 0;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(respuestaObj));
            respuestaObj testObj = ((respuestaObj)xmlSerializer.Deserialize(stream));
     

            //DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(respuestaObj));
            //respuestaObj employee = obj.ReadObject(stream) as respuestaObj;
        }

        private async Task<reversaDePagoResponse> getResponse(RecibosPagoWSClient client,reversaDePagoRequest request)
        {
            // get wallet coin list 
            await client.OpenAsync();

           return await client.reversaDePagoAsync(request).ConfigureAwait(false);
       
        }

        [Route("/Principal")]
        public IActionResult Principal()
        {
            return View("Inicio");
        }


        private async Task SignInUser(int idUsuario, string nombre, string perfil)
        {
            var claims = new List<Claim>
            {
                new Claim(CustomClaims.IdUsuario, idUsuario.ToString()),
                 new Claim(CustomClaims.Nombre, nombre),
                new Claim(CustomClaims.Perfil, perfil)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
        }
        [Route("cerrar-sesion")]
        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete(".GtoAdminApp");
            HttpContext.Session.Clear();
            return Redirect("/login");
        }




        [Route("/MarcasVehiculos")]
        [Authorize]
        public IActionResult Inicio()
        {
            return View();
        }
        [Route("error")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
