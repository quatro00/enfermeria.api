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
using enfermeria.api.Helpers;
using Microsoft.EntityFrameworkCore;
using enfermeria.api.Data;
using Stripe;

namespace enfermeria.api.Controllers.Colaborador
{
    [Route("api/colaborador/[controller]")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {
        private readonly IServicioFechasOfertaRepository servicioFechasOfertaRepository;
        private readonly IServicioFechaRepository servicioFechaRepository;
        private readonly IColaboradorRepository colaboradorRepository;
        private readonly IMapper mapper;
        private readonly DbContext _context;
        public ServiciosController(
            IServicioFechasOfertaRepository servicioFechasOfertaRepository,
            IServicioFechaRepository servicioFechaRepository,
            IColaboradorRepository colaboradorRepository,
            DbAb1c8aEnfermeriaContext context,
            IMapper mapper
            )
        {
            this.servicioFechasOfertaRepository = servicioFechasOfertaRepository;
            this.servicioFechaRepository = servicioFechaRepository;
            this.colaboradorRepository = colaboradorRepository;
            this.mapper = mapper;
            _context = context;
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
                    FechaInicio = DateTime.Now,
                    //FechaFin = model.FechaFin,
                    EstadoId = model.EstadoId,
                    MunicipioId = model.MunicipioId,
                    EstatusServicioFechaId = 1,
                    EstatusServicioId = 2 //SOLO LOS PAGADOS
                };

                var spec = new ServicioFechasSpecification(filtro);

                spec.IncludeStrings = new List<string>
                    {
                         "Servicio",
                        "Servicio.Municipio",
                        "Servicio.Municipio.Estado",
                        "Servicio.TipoEnfermera",
                        "Servicio.TipoLugar",
                        "ServicioFechasOferta"
                    };

                var userid = User.GetId();
                var colaborador = await this.colaboradorRepository.GetByUserIdAsync(userid);

                var pacientes = await this.servicioFechaRepository.ListAsync(spec);
                foreach(var item in pacientes)
                {
                    item.ServicioFechasOferta = item.ServicioFechasOferta.Where(x=>x.ColaboradorId == colaborador.Id && x.Activo == true && x.EstatusOfertaId == 1).ToList();
                }

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
        public async Task<IActionResult> GetServiciosProximos()
        {
            try
            {
                var userid = User.GetId();
                var colaborador = await this.colaboradorRepository.GetByUserIdAsync(userid);

                FiltroGlobal filtro = new FiltroGlobal()
                {
                    IncluirInactivos = false,
                    FechaInicio = DateTime.Now,
                    ColaboradorAsignadoId = colaborador.Id,
                    EstatusServicioFechaId = 2,
                    EstatusServicioId = 2 //SOLO LOS PAGADOS
                };

                var spec = new ServicioFechasSpecification(filtro);

                spec.IncludeStrings = new List<string>
                    {
                         "Servicio",
                        "Servicio.Municipio",
                        "Servicio.Municipio.Estado",
                        "Servicio.TipoEnfermera",
                        "Servicio.TipoLugar",
                        "ServicioFechasOferta"
                    };

                

                var pacientes = await this.servicioFechaRepository.ListAsync(spec);
                foreach (var item in pacientes)
                {
                    item.ServicioFechasOferta = item.ServicioFechasOferta.Where(x => x.ColaboradorId == colaborador.Id && x.Activo == true && x.EstatusOfertaId == 1).ToList();
                }

                var pacientesDto = mapper.Map<List<GetServiciosDisponiblesDto>>(pacientes);

                return Ok(pacientesDto.OrderBy(x => x.FechaInicio));
            }
            catch (Exception ex)
            {
                BadRequest("Ocurrio un error inesperado.");
            }
            return Ok();

        }

        [HttpPost("enviar-cotizacion")]
        [Authorize(Roles = "Colaborador")]
        public async Task<IActionResult> EnviarCotizacion([FromBody] EnviarCotizacionDto model)
        {
            try
            {
                FiltroGlobal filtro = new FiltroGlobal()
                {
                    ServicioFechaId = model.ServicioFechaId,
                };

                var spec = new ServicioFechasSpecification(filtro);

                spec.IncludeStrings = new List<string> { "Servicio", "ServicioFechasOferta" };

                var servicioFecha = await this.servicioFechaRepository.GetByIdAsync(
                model.ServicioFechaId,
                "Id",
                "Servicio",
                "ServicioFechasOferta"
            );

                var userid = User.GetId();
                var colaborador = await this.colaboradorRepository.GetByUserIdAsync(userid);

                //validamos el estatus del servicio fecha
                if (servicioFecha.EstatusServicioFechaId != 1) { 
                    return BadRequest("La guardia ya no se encuentra disponible"); }


                //validamos el estatus del servicio
                if (servicioFecha.Servicio.EstatusServicioId != 2) { 
                    return BadRequest("El servicio ya no se encuentra disponible"); }

                //validamos que no existe una oferta para la misma serviciofecha y colaborador
                var existeCotizacion = await this.servicioFechasOfertaRepository
                .AnyAsync(p => p.ServicioFechaId == model.ServicioFechaId && p.ColaboradorId == colaborador.Id && p.Activo == true);

                if (existeCotizacion)
                {
                    return BadRequest("La guardia ya se encuentra cotizada");
                }

                //si pasan las validaciones iniciamos una transaccion
                using var transaction = await _context.Database.BeginTransactionAsync();

                //generamos la oferta
                ServicioFechasOfertum servicioFechasOfertum = new ServicioFechasOfertum()
                {
                    ServicioFechaId = model.ServicioFechaId,
                    ColaboradorId = colaborador.Id,
                    MontoSolicitado = model.Monto,
                    Observaciones = model.Comentario,
                    Comentario = model.Comentario,
                    EstatusOfertaId = 1,
                    UsuarioCreacion = Guid.Parse(User.GetId()),
                    FechaCreacion = DateTime.Now,
                    Activo = true
                };

                await this.servicioFechasOfertaRepository.AddAsync(servicioFechasOfertum);
                await transaction.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                BadRequest("Ocurrio un error inesperado.");
            }
            return Ok();

        }
       

        [HttpPost("eliminar-cotizacion")]
        [Authorize(Roles = "Colaborador")]
        public async Task<IActionResult> EliminarCotizacion([FromBody] EliminarCotizacionDto model)
        {
            try
            {
                var userid = User.GetId();
                var colaborador = await this.colaboradorRepository.GetByUserIdAsync(userid);

                await this.servicioFechasOfertaRepository.UpdateWhereAsync(
                    c => c.ServicioFechaId == model.ServicioFechaId && c.ColaboradorId == colaborador.Id,
                    c => {
                        c.Activo = false;
                    }
                );
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
