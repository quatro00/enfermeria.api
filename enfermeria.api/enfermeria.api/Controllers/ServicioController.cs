using AutoMapper;
using enfermeria.api.Helpers;
using enfermeria.api.Models.Domain;
using enfermeria.api.Models.DTO.Paciente;
using enfermeria.api.Models;
using enfermeria.api.Repositories.Implementation;
using enfermeria.api.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using enfermeria.api.Models.DTO.Servicio;
using enfermeria.api.Models.DTO.Banco;
using DocumentFormat.OpenXml.Presentation;
using enfermeria.api.Helpers.Cotizacion;
using enfermeria.api.Models.DTO.TipoEnfermera;
using enfermeria.api.Models.DTO.Estado;

namespace enfermeria.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IServicioRepository servicioRepository;
        private readonly IHorarioRepository horarioRepository;
        private readonly ITipoEnfermeraRepository tipoEnfermeraRepository;
        private readonly IEstadoRepository estadoRepository;
        private readonly ITipoLugarRepository tipoLugarRepository;
        private readonly IPacienteRepository pacienteRepository;

        public ServicioController(
            IServicioRepository servicioRepository, 
            IMapper mapper, 
            IHorarioRepository horarioRepository, 
            ITipoEnfermeraRepository tipoEnfermeraRepository, 
            IEstadoRepository estadoRepository, 
            ITipoLugarRepository tipoLugarRepository,
            IPacienteRepository pacienteRepository)
        {
            this.servicioRepository = servicioRepository;
            this.horarioRepository = horarioRepository;
            this.tipoEnfermeraRepository = tipoEnfermeraRepository;
            this.estadoRepository = estadoRepository;
            this.tipoLugarRepository = tipoLugarRepository;
            this.pacienteRepository = pacienteRepository;
            this.mapper = mapper;
        }

        [HttpGet("ObtenerCotizacion/{id}")]
        public async Task<IActionResult> ObtenerCotizacion(Guid id)
        {
            // 1. Obtener los datos de la cotización (reemplaza esto por tu lógica real)
            var servicio = await this.servicioRepository.GetByIdAsync(
                id,
               "Id",

                "Estado",
                "TipoLugar",
                "TipoEnfermera",
                "Paciente",
                "ServicioFechas",
                "ServicioFechas.ServicioCotizacions"
            );


            if (servicio == null)
                return NotFound();

            var fileName = $"Cotizacion_{servicio.No.ToString()}.pdf";
            var filePath = Path.Combine(Path.GetTempPath(), fileName);

            // Generar el PDF
            CotizacionPdfGenerator.GenerarPdf(servicio, filePath);

            // Devolverlo como archivo descargable
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            

            // 3. Retornar el PDF como archivo descargable
            return File(fileBytes, "application/pdf", $"cotizacion-{id}.pdf");
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CrearServicio([FromBody] CrearServicioDto dto)
        {
            var response = new ResponseModel_2<Servicio>();

            // Validar si el modelo es válido
            if (!ModelState.IsValid)
            {
                User.GetId();
                response.SetResponse(false, "Modelo de datos inválido.");
                return BadRequest(response);
            }

            try
            {
                var servicio = mapper.Map<Servicio>(dto);
                servicio.UsuarioCreacion = Guid.Parse(User.GetId());
                servicio.Vigencia = DateTime.Now.AddDays(10).Date;
                servicio.Lat = "";
                servicio.Lon = "";

                //buscamos al tipo de enfermera para ver el costo
                var horarios = await this.horarioRepository.ListAsync();
                var tipoEnfermera_response = await tipoEnfermeraRepository.Get(dto.tipoEnfermeraId);
                var estado_response = await estadoRepository.Get(dto.estadoId);
                var tipoLugar_List = await this.tipoLugarRepository.ListAsync();
                var tipoLugar = tipoLugar_List.Where(x=>x.Id == dto.tipoLugarId).FirstOrDefault();
                var paciente = await this.pacienteRepository.GetByIdAsync(dto.pacienteId);

                var serviciosCotizacion = new List<ServicioCotizacion>();

                GetTipoEnfermera_Response tipoEnfermera = tipoEnfermera_response.result;
                GetEstado_Response estado = estado_response.result;

                decimal costoPorHora = tipoEnfermera.costoHora;

                List<CrearServicioFechasFormatoDto> listaSalida = dto.servicioFechasDtos.Select(e => new CrearServicioFechasFormatoDto
                {
                    fechaInicio = DateTime.Parse($"{e.fecha}T{e.inicio}"),
                    fechaTermino = DateTime.Parse($"{e.fecha}T{e.termino}"),
                    cantidadHoras = e.horas
                }).ToList();

                //mapeamos las fechas
                var fechas = mapper.Map<List<ServicioFecha>>(listaSalida);
                

               
                decimal cantidadHoras = 0;
                foreach (var fecha in servicio.ServicioFechas)
                {
                    cantidadHoras = fecha.CantidadHoras + cantidadHoras;
                    fecha.UsuarioCreacion = Guid.Parse(User.GetId());
                }

                servicio.TotalHoras = cantidadHoras;

                //calculamos los horarios
                var fechasHorarios = CalculoCotizacion.CalcularHorasPorTurnoPorFecha(horarios, fechas);
                //colocamos los horarios para las fechas
                foreach(var item in fechasHorarios)
                {
                    serviciosCotizacion = new List<ServicioCotizacion>();

                    foreach(var itemDet in item.DetallePorTurno)
                    {
                        decimal factor = 1;
                        var horario = horarios.Where(x=>x.Descripcion == itemDet.Horario).FirstOrDefault();
                        if(horario != null) { factor = horario.PorcentajeTarifa; }
                        

                        var servicioCotizacion = new ServicioCotizacion()
                        {
                            Horas = itemDet.Horas,
                            Horario = itemDet.Horario,
                            PrecioHoraBase = tipoEnfermera.costoHora,
                            PrecioHoraFinal = tipoEnfermera.costoHora * factor,
                            PrecioFinal = (tipoEnfermera.costoHora * factor) * itemDet.Horas,
                            Activo = true,
                            FechaCreacion = DateTime.Now,
                            UsuarioCreacion = Guid.Parse(User.GetId()),
                        };


                        serviciosCotizacion.Add(servicioCotizacion);
                    }
                    
                    fechas.Where(x=>x.FechaInicio.Date == item.Fecha).FirstOrDefault().ServicioCotizacions = serviciosCotizacion;
                }
                // Devolver la respuesta con el nuevo paciente
                servicio.ServicioFechas = fechas;
                var res = await servicioRepository.AddAsync(servicio);

                // Establecer la respuesta de éxito
                response.SetResponse(true, "Paciente creado correctamente.");

                // Devolver la respuesta con el nuevo paciente

                // Generar un nombre temporal para el PDF
                servicio.Estado = new CatEstado() { Nombre = estado.nombre, NombreCorto = estado.nombreCorto, };
                servicio.TipoEnfermera = new CatTipoEnfermera() { Descripcion = tipoEnfermera.descripcion, CostoHora = tipoEnfermera.costoHora , };
                servicio.TipoLugar = tipoLugar;
                servicio.Paciente = paciente;

                var fileName = $"Cotizacion_{servicio.No.ToString()}.pdf";
                var filePath = Path.Combine(Path.GetTempPath(), fileName);

                // Generar el PDF
                CotizacionPdfGenerator.GenerarPdf(servicio, filePath);

                // Devolverlo como archivo descargable
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                //return File(fileBytes, "application/pdf", fileName);
                return Ok(servicio.Id);


                return Ok(res);
                return Ok();
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
