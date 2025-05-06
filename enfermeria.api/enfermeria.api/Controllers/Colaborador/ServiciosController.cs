using AutoMapper;
using enfermeria.api.Models.DTO.Pago;
using enfermeria.api.Models.DTO;
using enfermeria.api.Models.Specifications;
using enfermeria.api.Models;
using enfermeria.api.Repositories.Implementation;
using enfermeria.api.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using enfermeria.api.Models.DTO.Servicio;
using enfermeria.api.Models.Domain;
using enfermeria.api.Helpers.Cotizacion;

namespace enfermeria.api.Controllers.Colaborador
{
    [Route("api/colaborador/[controller]")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {
        private readonly IServicioFechaRepository servicioFechaRepository;
        private readonly IMapper mapper;
        public ServiciosController(
            IServicioFechaRepository servicioFechaRepository,
            IMapper mapper
            )
        {
            this.servicioFechaRepository = servicioFechaRepository;
            this.mapper = mapper;
        }

        [HttpGet("ver-servicios-disponibles")]
        [Authorize(Roles = "Colaborador")]
        public async Task<IActionResult> GetServiciosDisponibles([FromQuery] FilterGetServiciosDisponiblesDto model)
        {
            try
            {
                FiltroGlobal filtro = new FiltroGlobal()
                {
                    IncluirInactivos = false,
                    FechaInicio = model.fechaInicio,
                    FechaFin = model.FechaFin,
                    EstadoId = model.EstadoId,
                    MunicipioId = model.MunicipioId,
                    EstatusServicioFechaId = 1
                };

                var spec = new ServicioFechasSpecification(filtro);

                spec.IncludeStrings = new List<string>
                    {
                         "Servicio",
                        "Servicio.Municipio",
                        "Servicio.Municipio.Estado",
                        "Servicio.TipoEnfermera",
                        "Servicio.TipoLugar"
                    };

                var pacientes = await this.servicioFechaRepository.ListAsync(spec);
                var pacientesDto = mapper.Map<List<GetServiciosDisponiblesDto>>(pacientes);

                return Ok(pacientesDto.OrderBy(x=>x.FechaInicio));
            }
            catch (Exception ex)
            {
                BadRequest("Ocurrio un error inesperado.");
            }
            return Ok();

        }

        [HttpGet("ver-servicios-proximos")]
        [Authorize(Roles = "Colaborador")]
        public async Task<IActionResult> GetDepositos()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                BadRequest("Ocurrio un error inesperado.");
            }
            return Ok();

        }
    }
}
