using AutoMapper;
using enfermeria.api.Data;
using enfermeria.api.Helpers;
using enfermeria.api.Models.DTO.Servicio;
using enfermeria.api.Models.DTO;
using enfermeria.api.Models.Specifications;
using enfermeria.api.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using enfermeria.api.Models.DTO.Pago;
using DocumentFormat.OpenXml.Drawing.Charts;
using enfermeria.api.Models;
using System.Globalization;

namespace enfermeria.api.Controllers.Colaborador
{
    [Route("api/Colaborador/[controller]")]
    [ApiController]
    public class PagosController : ControllerBase
    {
        private readonly IServicioFechasOfertaRepository servicioFechasOfertaRepository;
        private readonly IServicioFechaRepository servicioFechaRepository;
        private readonly IColaboradorRepository colaboradorRepository;
        private readonly IPagoRepository pagoRepository;
        private readonly IPagoLoteRepository pagoLoteRepository;
        private readonly IMapper mapper;
        private readonly DbContext _context;
        public PagosController(
            IServicioFechasOfertaRepository servicioFechasOfertaRepository,
            IServicioFechaRepository servicioFechaRepository,
            IColaboradorRepository colaboradorRepository,
            DbAb1c8aEnfermeriaContext context,
            IPagoRepository pagoRepository,
            IPagoLoteRepository pagoLoteRepository,
            IMapper mapper
            )
        {
            this.servicioFechasOfertaRepository = servicioFechasOfertaRepository;
            this.servicioFechaRepository = servicioFechaRepository;
            this.colaboradorRepository = colaboradorRepository;
            this.pagoRepository = pagoRepository;
            this.pagoLoteRepository = pagoLoteRepository;
            this.mapper = mapper;
            _context = context;
        }

        [HttpGet("ver-pagos")]
        [Authorize(Roles = "Colaborador")]
        public async Task<IActionResult> GetPagos([FromQuery] GetPagosColaborador model)
        {
            try
            {
                var userid = User.GetId();
                var colaborador = await this.colaboradorRepository.GetByUserIdAsync(userid);

                DateTime inicio = DateTime.ParseExact(model.periodo + "-01", "yyyy-MM-dd", null);
                DateTime fin = inicio.AddMonths(1).AddDays(-1);

                FiltroGlobal filtro = new FiltroGlobal()
                {
                    IncluirInactivos = false,
                    ColaboradorAsignadoId = colaborador.Id,
                    FechaInicio = inicio,
                    FechaFin = fin,
                };

                var spec = new PagoLoteSpecification(filtro);

                spec.IncludeStrings = new List<string>
                    {
                         "EstatosPagoLote",
                        "Pagos",
                        "Pagos.ServicioFecha"
                    };

                var pacientes = await this.pagoLoteRepository.ListAsync(spec);
                var pacientesDto = mapper.Map<List<GetPagosColaboradorDto>>(pacientes);
                return Ok(pacientesDto.OrderBy(x=>x.Folio));
            }
            catch (Exception ex)
            {
                BadRequest("Ocurrio un error inesperado.");
            }
            return Ok();

        }

        [HttpGet("ver-grafica-pagos")]
        [Authorize(Roles = "Colaborador")]
        public async Task<IActionResult> GetGraficaPagos()
        {
            try
            {
                var userid = User.GetId();
                var colaborador = await this.colaboradorRepository.GetByUserIdAsync(userid);


                // Obtenemos la fecha de hoy
                DateTime hoy = DateTime.Today;

                // Calculamos el último día del mes actual (termino)
                DateTime fin = new DateTime(hoy.Year, hoy.Month, DateTime.DaysInMonth(hoy.Year, hoy.Month));

                // Calculamos el primer día de tres meses atrás (inicio)
                DateTime inicio = new DateTime(hoy.Year, hoy.Month, 1).AddMonths(-3);

                FiltroGlobal filtro = new FiltroGlobal()
                {
                    IncluirInactivos = false,
                    ColaboradorAsignadoId = colaborador.Id,
                    FechaInicio = inicio,
                    FechaFin = fin,
                };

                var spec = new PagoLoteSpecification(filtro);

                spec.IncludeStrings = new List<string>
                    {
                         "EstatosPagoLote",
                        "Pagos",
                        "Pagos.ServicioFecha"
                    };

                var pacientes = await this.pagoLoteRepository.ListAsync(spec);
                var pacientesDto = mapper.Map<List<GetPagosColaboradorDto>>(pacientes);
                var cultura = new CultureInfo("es-MX");


                var ultimosMeses = Enumerable.Range(0, 3)
                .Select(i => hoy.AddMonths(-i))
                .Select(fecha => new {
                    Año = fecha.Year,
                    MesNumero = fecha.Month,
                    MesNombre = cultura.DateTimeFormat.GetMonthName(fecha.Month)
                })
                .OrderBy(x => x.Año)
                .ThenBy(x => x.MesNumero)
                .ToList();


                var pagosAgrupados = pacientesDto
                .GroupBy(p => new { p.Fecha.Year, p.Fecha.Month })
                .Select(g => new {
                    Año = g.Key.Year,
                    MesNumero = g.Key.Month,
                    TotalMonto = g.Sum(p => p.Total)
                })
                .ToList();


                var resultadoFinal = ultimosMeses
                .GroupJoin(
                    pagosAgrupados,
                    mes => new { mes.Año, mes.MesNumero },
                    pago => new { pago.Año, pago.MesNumero },
                    (mes, pagos) => new {
                        Año = mes.Año,
                        Mes = mes.MesNombre,
                        TotalMonto = pagos.FirstOrDefault()?.TotalMonto ?? 0
                    })
                .ToList();







                return Ok(resultadoFinal);
            }
            catch (Exception ex)
            {
                BadRequest("Ocurrio un error inesperado.");
            }
            return Ok();

        }

        // [Authorize(Roles = "Colaborador")]
        [HttpGet("descargar-pago")]
        public async Task<IActionResult> DescargarDeposito([FromQuery]Guid pagoLoteId)
        {

            FiltroGlobal filtro = new FiltroGlobal()
            {
                PagoLoteId = pagoLoteId
            };


            //creamos la respuesta
            var response = new ResponseModel_2<List<GetPagosDto>>();
            filtro.IncluirInactivos = false;

            try
            {
                //colocamos los filtros
                var spec = new PagoSpecification(filtro);

                //colocamos los includes
                spec.IncludeStrings = new List<string> { "ServicioFecha", "ServicioFecha.ColaboradorAsignado", "EstatusPago" };

                //convertimos de la clase al dto
                var result = await pagoRepository.ListAsync(spec);
                var pago = result.FirstOrDefault();


                var filePath = pago.Comprobante;

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound();
                }

                var bytes = System.IO.File.ReadAllBytes(filePath);
                string extension = Path.GetExtension(filePath);

                return File(bytes, "application/octet-stream", result.FirstOrDefault().Referencia + extension);

            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, manejar el error
                response.SetResponse(false, "Ocurrió un error al crear el paciente.");

                // Puedes registrar el error o manejarlo como desees, por ejemplo:
                // Log.Error(ex, "Error al crear paciente");

                // Devolver una respuesta con el error
                response.Data = ex.Message; // Puedes agregar más detalles del error si lo deseas
                return StatusCode(500, response); // O devolver un BadRequest(400) si el error es de entrada
            }

        }
    }
}
