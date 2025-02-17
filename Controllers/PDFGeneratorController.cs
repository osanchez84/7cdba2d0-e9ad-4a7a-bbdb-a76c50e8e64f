﻿using GuanajuatoAdminUsuarios.Helpers;
using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using GuanajuatoAdminUsuarios.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PdfSharp;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using static GuanajuatoAdminUsuarios.RESTModels.ConsultarDocumentoResponseModel;
using Newtonsoft.Json;
using GuanajuatoAdminUsuarios.Utils;
//using Telerik.SvgIcons;
using GuanajuatoAdminUsuarios.Models.PDFModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using System.Collections;

namespace GuanajuatoAdminUsuarios.Controllers
{
	[Authorize]
	public class PDFGeneratorController : Controller
	{
		private readonly ICapturaAccidentesService _capturaAccidentesService;
		private readonly ICatAutoridadesDisposicionService _catAutoridadesDisposicionservice;
		private readonly ICatAutoridadesEntregaService _catAutoridadesEntregaService;
		private readonly IAppSettingsService _appSettingsService;
		private readonly IBusquedaAccidentesService _busquedaAccidentesService;
		private readonly IInfraccionesService _infraccionesService;
		private readonly IPdfGenerator _pdfService;
		private readonly ICatEntidadesService _catEntidadesService;
		private readonly IOficiales _oficialesService;
		private readonly ICatAgenciasMinisterioService _catAgenciasMinisterioService;
		public PDFGeneratorController(ICapturaAccidentesService capturaAccidentesService, ICatAutoridadesDisposicionService catAutoridadesDisposicionservice, IAppSettingsService appSettingService, IBusquedaAccidentesService busquedaAccidentesService, IPdfGenerator pdfGenerator, IInfraccionesService infraccionesService, ICatAutoridadesEntregaService catAutoridadesEntregaService, ICatEntidadesService catEntidadesService, IOficiales oficialesService, ICatAgenciasMinisterioService catAgenciasMinisterioService)
		{
			_capturaAccidentesService = capturaAccidentesService;
			_catAutoridadesDisposicionservice = catAutoridadesDisposicionservice;
			_appSettingsService = appSettingService;
			_busquedaAccidentesService = busquedaAccidentesService;
			_pdfService = pdfGenerator;
			_infraccionesService = infraccionesService;
			_catAutoridadesEntregaService = catAutoridadesEntregaService;
			_catEntidadesService = catEntidadesService;
			_oficialesService = oficialesService;
			_catAgenciasMinisterioService = catAgenciasMinisterioService;
		}

		[HttpGet]
		public async Task<FileResult> AccidentesDetallado(int idAccidente)
		{
			int idOficina = HttpContext.Session.GetInt32("IdOficina") ?? 0;
           // int idDependencia = (int)HttpContext.Session.GetInt32("IdDependencia");

            var AccidenteSeleccionado = _capturaAccidentesService.ObtenerAccidentePorId(idAccidente, idOficina);
			DatosAccidenteModel datosAccidente = _capturaAccidentesService.ObtenerDatosFinales(idAccidente);
			var ListVehiculosInvolucrados = _capturaAccidentesService.VehiculosInvolucrados(idAccidente);

			var ListClasificaciones = _capturaAccidentesService.ObtenerDatosGrid(idAccidente);
			var ListFactores = _capturaAccidentesService.ObtenerDatosGridFactor(idAccidente);
			var ListCausas = _capturaAccidentesService.ObtenerDatosGridCausa(idAccidente);
			var ListInfracciones = _capturaAccidentesService.InfraccionesDeAccidente(idAccidente);

			var ListInvolucrados = _capturaAccidentesService.InvolucradosAccidente(idAccidente);

			foreach (var invo in ListVehiculosInvolucrados) 
			{
				foreach (var lst in ListInvolucrados.Where(r => r.IdVehiculo == invo.IdVehiculo))
				{
					lst.numeroConsecutivo = invo.numeroConsecutivo;
				} 
			}

            var ParteNombre = _appSettingsService.GetAppSetting("ParteNombre").SettingValue;
			var PartePuesto = _appSettingsService.GetAppSetting("PartePuesto").SettingValue;
			List<PDFMotivosInfracciones> motivosInfraccion = ListInfracciones.Select(s => new PDFMotivosInfracciones { idInfraccion = s.IdInfraccion, Motivos = _infraccionesService.GetMotivosInfraccionByIdInfraccion((int)s.IdInfraccion).Select(ss => ss.Concepto+"-"+ss.Fundamento).ToList() }).ToList();

			PDFAccidenteDetalladoModel model = new PDFAccidenteDetalladoModel();
			model.Involucrados = ListInvolucrados;
			model.MotivosInfraccion = motivosInfraccion;
			model.ParteAccidente = AccidenteSeleccionado;
			model.ParteAccidenteComplemento = datosAccidente;
			model.VehiculosInvolucrados = ListVehiculosInvolucrados;
			
			model.Clasificaciones = ListClasificaciones;
			model.Factores = ListFactores;
			model.CausasDeterminantes = ListCausas;

			foreach (var invo in ListVehiculosInvolucrados)
			{
				foreach (var lst in ListInfracciones.Where(r => r.IdVehiculo == invo.IdVehiculo))
				{
					lst.numeroConsecutivo = invo.numeroConsecutivo;
				}
			}
			model.Infracciones = ListInfracciones;
			

			model.ParteNombre = ParteNombre;
			model.PartePuesto = PartePuesto;
			model.ADisposicion = _catAutoridadesDisposicionservice.ObtenerAutoridadesActivas().Where(w => w.IdAutoridadDisposicion == datosAccidente.IdAutoridadDisposicion).Select(s => s.NombreAutoridadDisposicion).FirstOrDefault();
			model.entregadoA = _catAutoridadesEntregaService.ObtenerAutoridadesActivas().Where(w => w.IdAutoridadEntrega == datosAccidente.IdAutoridadEntrega).Select(s => s.AutoridadEntrega).FirstOrDefault();
			model.sede = _catEntidadesService.ObtenerEntidades().Where(w => w.idEntidad == datosAccidente.IdEntidadCompetencia).Select(s => s.nombreEntidad).FirstOrDefault();


			model.ElaboraConsignacion = _oficialesService.GetOficialesActivos().Where(w=> w.IdOficial == datosAccidente.IdElaboraConsignacion)
				.Select(o => CultureInfo.InvariantCulture.TextInfo.ToTitleCase($"{o.Nombre} {o.ApellidoPaterno} {o.ApellidoMaterno}")).FirstOrDefault();
			model.NoOficio = datosAccidente.numeroOficio;
			model.AgenciaRecibe = _catAgenciasMinisterioService.ObtenerAgenciasActivas().Where(w => w.IdAgenciaMinisterio == datosAccidente.IdAgenciaMinisterio).Select(s => s.NombreAgencia).FirstOrDefault();
			model.recibe = datosAccidente.RecibeMinisterio;

			model.Elabora = _oficialesService.GetOficialesActivos().Where(w => w.IdOficial == datosAccidente.IdElabora)
				.Select(o => CultureInfo.InvariantCulture.TextInfo.ToTitleCase($"{o.Nombre} {o.ApellidoPaterno} {o.ApellidoMaterno}")).FirstOrDefault();
			model.Supervisor = _oficialesService.GetOficialesActivos().Where(w => w.IdOficial == datosAccidente.IdSupervisa)
				.Select(o => CultureInfo.InvariantCulture.TextInfo.ToTitleCase($"{o.Nombre} {o.ApellidoPaterno} {o.ApellidoMaterno}")).FirstOrDefault();
			model.Autoriza = _oficialesService.GetOficialesActivos().Where(w => w.IdOficial == datosAccidente.IdAutoriza)
				.Select(o => CultureInfo.InvariantCulture.TextInfo.ToTitleCase($"{o.Nombre} {o.ApellidoPaterno} {o.ApellidoMaterno}")).FirstOrDefault();

			string jsonView = await this.RenderViewAsync("_AccidentesDetallado", model, false);
			var bootstrap = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "css", "bootstrap.min.css");
			bootstrap = System.IO.File.ReadAllText(bootstrap);
			string css = @".s1,.s2,.s4{text-decoration:none}.s1,.s2,.s4,p{color:#000;font-family:Arial,sans-serif;font-style:normal;font-weight:400}*,p{margin:0}h1,p{font-size:9pt}.lateral hr,.middle hr{margin-top:1rem;margin-bottom:1rem}table,table tr td p,td,th,thead,tr{padding:0!important;margin:0!important}table tr td p.s2,table tr td p.s3,table tr td p.s4,table tr td p.s5,table tr td p.s6{text-align:center}*{padding:0;text-indent:0}.s1{font-size:8pt}.s2,.s4{font-size:7pt}p{text-decoration:none}.s3,.s5,h1{font-weight:700;color:#000;font-family:Arial,sans-serif;font-style:normal;text-decoration:none}.s3{font-size:8pt}.s5{font-size:7pt}.s6{color:#000;font-family:Arial,sans-serif;font-style:normal;font-weight:400;text-decoration:none;font-size:2pt}table,tbody{vertical-align:top;overflow:visible;border-color:#000}.middle hr{border:0;width:50%;border-top:1pt solid #000}.lateral hr{border:0;width:60%;border-top:1pt solid #000}.table td{padding:1px!important;vertical-align:top;border-top:0}table,td,th,thead,tr{border-style:solid;border-width:.5pt;border-color:#000}table tr td p{font-size:5.5pt!important}table tr td p.s3{vertical-align:central}";
			bootstrap = bootstrap + css;
			////(MapPath("~/css/test.css"));

			byte[] pdfBytes = _pdfService.CreatePDFByHTML(jsonView, bootstrap, iTextSharp.text.PageSize.LETTER);
			//return Json(jsonView);
			return File(pdfBytes, "application/pdf");
			//return PartialView("_AccidentesDetallado", model);
		}

		[HttpPost]
		public ContentResult AccidentesGeneral(BusquedaAccidentesModel model)
		{
			int idOficina = HttpContext.Session.GetInt32("IdOficina") ?? 0;
            int idDependencia = (int)HttpContext.Session.GetInt32("IdDependencia");

            var modelList = _busquedaAccidentesService.GetAllAccidentes()
												.Where(w => w.idMunicipio == (model.idMunicipio > 0 ? model.idMunicipio : w.idMunicipio)
													&& w.idSupervisa == (model.IdOficialBusqueda > 0 ? model.IdOficialBusqueda : w.idSupervisa)
													&& w.idCarretera == (model.IdCarreteraBusqueda > 0 ? model.IdCarreteraBusqueda : w.idCarretera)
													&& w.idTramo == (model.IdTramoBusqueda > 0 ? model.IdTramoBusqueda : w.idTramo)
													&& w.idElabora == (model.IdOficialBusqueda > 0 ? model.IdOficialBusqueda : w.idElabora)
													&& w.idAutoriza == (model.IdOficialBusqueda > 0 ? model.IdOficialBusqueda : w.idAutoriza)
													&& w.idEstatusReporte == (model.IdEstatusAccidente > 0 ? model.IdEstatusAccidente : w.idEstatusReporte)
												   && (string.IsNullOrEmpty(model.folioBusqueda) || w.numeroReporte.Contains(model.folioBusqueda))
												   && (string.IsNullOrEmpty(model.placasBusqueda) || w.placa.Contains(model.placasBusqueda))
												   && (string.IsNullOrEmpty(model.propietarioBusqueda) || w.propietario.Contains(model.propietarioBusqueda))
												   && (string.IsNullOrEmpty(model.serieBusqueda) || w.serie.Contains(model.serieBusqueda))
												   && (string.IsNullOrEmpty(model.conductorBusqueda) || w.conductor.Contains(model.conductorBusqueda))
												   && ((model.FechaInicio == default(DateTime) && model.FechaFin == default(DateTime)) || (w.fecha >= model.FechaInicio && w.fecha <= model.FechaFin))
													).ToList();
			Dictionary<string, string> ColumnsNames = new Dictionary<string, string>()
			{
			{"IdAccidente","Número"},
			{"numeroReporte","Folio"},
			{"fecha","Fecha"},
			{"hora","Hora"},
			{"municipio","Municipio" },
			{"carretera","Carretera" },
			{"tramo","Tramo" },
			{"estatusReporte", "Estatus" }
			};

			var result = _pdfService.CreatePdf("ReporteAccidentesGeneral", "Reporte General de Accidentes", 8, ColumnsNames, modelList);
			//return File(result.Item1, "application/pdf", result.Item2);
			byte[] bytes = result.Item1.ToArray();
			string base64 = Convert.ToBase64String(bytes, 0, bytes.Length);
			return Content(base64);
		}

		[HttpPost]
		public ContentResult InfraccionesGeneral(InfraccionesBusquedaModel model)
		{


			int idDependencia = (int)HttpContext.Session.GetInt32("IdDependencia");

			int idOficina = HttpContext.Session.GetInt32("IdOficina") ?? 0;
			var modelList = _infraccionesService.GetAllInfracciones(model, idOficina, idDependencia);
			var pdfModel = modelList.Select(s => new InfraccionesGeneralPDFModel
			{
				folioInfraccion = s.folioInfraccion,
				conductor = s.PersonaInfraccion.nombreCompleto,
				propietario = s.Vehiculo.Persona.nombreCompleto,
				fechaAplicacion = s.fechaInfraccion,
				garantia = s.Garantia.garantia,
				vehiculo = s.Vehiculo.fullVehiculo,
				placas = s.Vehiculo.placas,
				delegacion = s.delegacion,
				estatusInfraccion = s.estatusInfraccion,
				aplicacion=s.aplicacion
			}).ToList();
			Dictionary<string, string> ColumnsNames = new Dictionary<string, string>()
			{
                //{"idInfraccion",""},
                {"folioInfraccion","Folio"},
				{"conductor","Conductor"},
				{"propietario","Propietario"},
				{"fechaAplicacion","Fecha Aplicación"},
				{"garantia","Garantía"},
				{"vehiculo","Vehículo"},
				{"placas","Placas"},
				{"delegacion","Delegación"},
				{ "estatusInfraccion", "Estatus"},
				{ "aplicacion","Aplicadoa"}
			};
			var result = _pdfService.CreatePdf("ReporteInfraccionesGeneral", "Reporte General de Infracciones", 10, ColumnsNames, pdfModel);
			byte[] bytes = result.Item1.ToArray();
			string base64 = Convert.ToBase64String(bytes, 0, bytes.Length);
			return Content(base64);
		}

	}
}
