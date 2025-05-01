using AutoMapper;
using enfermeria.api.Models.DTO.ServicioFecha;
using enfermeria.api.Models.DTO;
using enfermeria.api.Models.Specifications;
using enfermeria.api.Models;
using enfermeria.api.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using enfermeria.api.Models.DTO.Dashboard;
using enfermeria.api.Enums;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace enfermeria.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IServicioFechaRepository servicioFechaRepository;
        private readonly ITipoLugarRepository tipoLugarRepository;
        private readonly IColaboradorRepository colaboradorRepository;
        private readonly IServicioFechasOfertaRepository servicioFechasOfertaRepository;
        private readonly IPagoRepository pagoRepository;

        public DashboardController(IServicioFechaRepository servicioFechaRepository, IServicioFechasOfertaRepository servicioFechasOfertaRepository, IColaboradorRepository colaboradorRepository, ITipoLugarRepository tipoLugarRepository, IPagoRepository pagoRepository, IMapper mapper)
        {
            this.servicioFechasOfertaRepository = servicioFechasOfertaRepository;
            this.colaboradorRepository = colaboradorRepository;
            this.servicioFechaRepository = servicioFechaRepository;
            this.tipoLugarRepository = tipoLugarRepository;
            this.pagoRepository = pagoRepository;

            this.mapper = mapper;
        }

        [HttpGet("obtener-indicadores")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetIndicadores(DateTime? Periodo)
        {
            if(Periodo == null) Periodo = DateTime.Now;
            DateTime? fechaInicio = null;
            DateTime? fechaFin = null;

            fechaInicio = new DateTime(Periodo.Value.Year, Periodo.Value.Month, 1);
            fechaFin = fechaInicio.Value.AddMonths(1).AddDays(-1);



            FiltroGlobal filtro = new FiltroGlobal()
            {
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
            };


            //creamos la respuesta
            var response = new ResponseModel_2<List<IndicadoresDto>>();
            filtro.IncluirInactivos = false;

            try
            {
                //colocamos los filtros
                var spec = new ServicioFechasSpecification(filtro);

                //colocamos los includes
                spec.IncludeStrings = new List<string> { "EstatusServicioFecha", "ServicioFechasOferta", "Servicio" };

                //convertimos de la clase al dto
                var result = await this.servicioFechaRepository.ListAsync(spec);
                
                List<int> estatusList = new List<int>() { 1,2,3,4,5,99};

                List<IndicadoresDto> resultDto = new List<IndicadoresDto>();
                foreach (var e in estatusList) 
                {
                    string descripcion = "";
                    string color = "";
                    string icono = "";
                    switch (e)
                    {
                        case 1:
                            icono = "user";
                            descripcion = "Por asignar";
                            color = "primary";
                            break;
                        case 2:
                            icono = "user-check";
                            descripcion = "Asignados";
                            color = "primary";
                            break;
                        case 3: //ok
                            descripcion = "Completados";
                            color = "success";
                            icono = "check-circle";
                            break;
                        case 4: //ok
                            descripcion = "En proceso de pago";
                            color = "warning";
                            icono = "money-bill";
                            break;
                        case 5: //ok
                            descripcion = "Pagados";
                            color = "success";
                            icono = "money-bill";
                            break;
                        case 99: //ok
                            descripcion = "Cancelados";
                            color = "warning";
                            icono = "cancel";
                            break;
                    }
                    resultDto.Add(new IndicadoresDto()
                    {
                        Color = color,
                        Descripcion = descripcion,
                        Icono = icono,
                        Orden = e,
                        Total = result.Where(x => x.EstatusServicioFechaId == e).Count(),

                    });
                    
                }

                //seteamos el resultado
                response.SetResponse(true, "");
                response.Result = resultDto.OrderBy(x=>x.Orden).ToList();

                return Ok(resultDto.OrderBy(x => x.Orden).ToList());
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

        [HttpGet("obtener-grafica-tipo-servicios")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetTipoServicios(DateTime? Periodo)
        {
            if (Periodo == null) Periodo = DateTime.Now;
            DateTime? fechaInicio = null;
            DateTime? fechaFin = null;

            fechaInicio = new DateTime(Periodo.Value.Year, Periodo.Value.Month, 1);
            fechaFin = fechaInicio.Value.AddMonths(1).AddDays(-1);



            FiltroGlobal filtro = new FiltroGlobal()
            {
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
            };


            //creamos la respuesta
            var response = new ResponseModel_2<GraficoTipoLugar>();
            filtro.IncluirInactivos = false;

            try
            {
                //buscamos los tipos de lugar
                var tiposLugar = await this.tipoLugarRepository.ListAsync();
                GraficoTipoLugar result = new GraficoTipoLugar() { labels = new List<string>(), series = new List<decimal>() };
                //colocamos los filtros
                var spec = new ServicioFechasSpecification(filtro);

                //colocamos los includes
                spec.IncludeStrings = new List<string> { "EstatusServicioFecha", "ServicioFechasOferta", "Servicio" };

                //convertimos de la clase al dto
                var servicios = await this.servicioFechaRepository.ListAsync(spec);
                tiposLugar = tiposLugar.OrderBy(x=>x.Id).ToList();
                foreach (var item in tiposLugar) 
                {
                    decimal total = 0;
                    total = servicios.Where(x => x.Servicio.TipoLugarId == item.Id).Count();
                    result.labels.Add(item.Descripcion);
                    result.series.Add(total);
                }
                //seteamos el resultado
                response.SetResponse(true, "");
                response.Result = result;

                return Ok(result);
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
        [HttpGet("obtener-grafica-pagos")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetGraficaPagos(DateTime? Periodo)
        {
            if (Periodo == null) Periodo = DateTime.Now;
            DateTime? fechaInicio = null;
            DateTime? fechaFin = null;

            fechaInicio = new DateTime(Periodo.Value.Year, Periodo.Value.Month, 1);
            fechaFin = fechaInicio.Value.AddMonths(1).AddDays(-1);



            FiltroGlobal filtro = new FiltroGlobal()
            {
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
            };


            //creamos la respuesta
            var response = new ResponseModel_2<GraficoDonut>();
            filtro.IncluirInactivos = false;

            try
            {
                //buscamos los tipos de lugar

                GraficoDonut result = new GraficoDonut() { labels = new List<string>(), series = new List<decimal>() };
                //colocamos los filtros
                var spec = new PagoSpecification(filtro);

                //colocamos los includes
                spec.IncludeStrings = new List<string> {  };

                //convertimos de la clase al dto
                var pagos = await this.pagoRepository.ListAsync(spec);
                var estatusPago = new List<int>();
                estatusPago.Add(1);
                estatusPago.Add(2);
                estatusPago.Add(99);



                foreach (var item in estatusPago)
                {
                    decimal total = 0;
                    string label = "";
                    total = pagos.Where(x => x.EstatusPagoId == item).Count();

                    switch (item)
                    {
                        case 1:
                            label = "Por pagar";
                            break;
                        case 2:
                            label = "Pagados";
                            break;
                        case 99:
                            label = "Cancelados";
                            break;
                    }

                    result.labels.Add(label);
                    result.series.Add(total);
                }
                //seteamos el resultado
                response.SetResponse(true, "");
                response.Result = result;

                return Ok(result);
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
        [HttpGet("obtener-guardias-proximas")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetGuardiasProximas()
        {
            
            DateTime? fechaInicio = null;
            DateTime? fechaFin = null;

            fechaInicio = DateTime.Now.Date;



            FiltroGlobal filtro = new FiltroGlobal()
            {
                FechaInicio = fechaInicio
            };


            //creamos la respuesta
            var response = new ResponseModel_2<List<EventoGuardiasDto>>();
            filtro.IncluirInactivos = false;

            try
            {
                //buscamos los tipos de lugar

                List<EventoGuardiasDto> result = new List<EventoGuardiasDto>();
                //colocamos los filtros
                var spec = new ServicioFechasSpecification(filtro);

                //colocamos los includes
                spec.IncludeStrings = new List<string> { "EstatusServicioFecha", "ServicioFechasOferta", "Servicio", "ColaboradorAsignado", "Servicio" };

                //convertimos de la clase al dto
                var guardias = await this.servicioFechaRepository.ListAsync(spec);
                foreach(var guardia in guardias)
                {
                    string colaboradorAsignado = guardia.ColaboradorAsignado != null
    ? guardia.ColaboradorAsignado.Nombre + " " + guardia.ColaboradorAsignado.Apellidos
    : "Por asignar";
                    result.Add(new EventoGuardiasDto() 
                    {
                        Title = "Servicio No." + guardia.Servicio.No.ToString() + " Colaborador: "+ colaboradorAsignado,
                        Start = guardia.FechaInicio.ToString("yyyy-MM-ddTHH:mm:ss"),
                        End = guardia.FechaTermino.ToString("yyyy-MM-ddTHH:mm:ss"),
                        Description = "Guardia",
                        Asignado = colaboradorAsignado,
                        Estatus = guardia.EstatusServicioFecha.Descripcion
                    });
                }
                //seteamos el resultado
                response.SetResponse(true, "");
                response.Result = result;

                return Ok(result);
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

        [HttpGet("obtener-grafica-ingresos")]
        //[Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetGraficaIngresos()
        {

            DateTime? fechaInicio = null;
            DateTime? fechaFin = null;

            fechaFin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            fechaFin = fechaFin.Value.AddMonths(1).AddDays(-1);
            fechaInicio = new DateTime(fechaFin.Value.AddYears(-1).AddMonths(1).Year, fechaFin.Value.AddYears(-1).AddMonths(1).Month, 1);



            FiltroGlobal filtro = new FiltroGlobal()
            {
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
            };

            //creamos la respuesta
            var response = new ResponseModel_2<List<EventoGuardiasDto>>();
            filtro.IncluirInactivos = false;

            try
            {
                //buscamos los tipos de lugar

                GraficaIngresosDto result = new GraficaIngresosDto() { Series = new List<GraficaIngresosDetalleDto>() };
                List<decimal> ingresos = new List<decimal>();
                List<decimal> costosDeServicio = new List<decimal>();
                List<decimal> utilidadNeta = new List<decimal>();
                List<string> meses = new List<string>();

                //colocamos los filtros
                var spec = new PagoSpecification(filtro);

                //colocamos los includes
                spec.IncludeStrings = new List<string> { };

                //convertimos de la clase al dto
                var guardias = await this.pagoRepository.ListAsync(spec);
                DateTime fechaBusqueda = (DateTime)fechaInicio;

                for (int i = 0; i < 12; i++){
                    DateTime fechaInicio_busqueda = (DateTime)fechaInicio;
                    DateTime fechaFinBusqueda = fechaInicio_busqueda.AddMonths(1).AddDays(-1);

                    ingresos.Add(guardias.Where(x => x.FechaCreacion >= fechaInicio_busqueda && x.FechaCreacion <= fechaFinBusqueda).Sum(x => x.ImporteBruto));
                    costosDeServicio.Add(guardias.Where(x => x.FechaCreacion >= fechaInicio_busqueda && x.FechaCreacion <= fechaFinBusqueda).Sum(x => x.Total));
                    utilidadNeta.Add(guardias.Where(x => x.FechaCreacion >= fechaInicio_busqueda && x.FechaCreacion <= fechaFinBusqueda).Sum(x => x.Comision));
                    meses.Add(fechaInicio.Value.ToString("MMMM", new System.Globalization.CultureInfo("es-ES")));
                    fechaInicio = fechaInicio.Value.AddMonths(1);
                }

                result.Series.Add(new GraficaIngresosDetalleDto()
                {
                    name = "Ingresos",
                    color = "#008FFB",//"#01B81A",
                    data = ingresos,
                });

                result.Series.Add(new GraficaIngresosDetalleDto()
                {
                    name = "Costos de servicios",
                    color = "#FA8B0C",
                    data = costosDeServicio,
                });

                result.Series.Add(new GraficaIngresosDetalleDto()
                {
                    name = "Utilidad neta",
                    color = "#01B81A",
                    data = utilidadNeta,
                });

                result.Meses = meses;
                //seteamos el resultado
                response.SetResponse(true, "");
                //response.Result = result;

                return Ok(result);
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
