using AutoMapper;
using enfermeria.api.Models.DTO.Colaborador;
using enfermeria.api.Models.DTO;
using enfermeria.api.Models.Specifications;
using enfermeria.api.Models;
using enfermeria.api.Repositories.Implementation;
using enfermeria.api.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using enfermeria.api.Models.DTO.ServicioFecha;
using enfermeria.api.Enums;
using Microsoft.EntityFrameworkCore;
using enfermeria.api.Data;
using Stripe.Apps;

namespace enfermeria.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioFechaController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IServicioFechaRepository servicioFechaRepository;
        private readonly IServicioRepository servicioRepository;
        private readonly IColaboradorRepository colaboradorRepository;
        private readonly IServicioFechasOfertaRepository servicioFechasOfertaRepository;
        private readonly IConfiguracionRepository configuracionRepository;
        private readonly DbContext _context;

        public ServicioFechaController(IServicioFechaRepository servicioFechaRepository, IServicioFechasOfertaRepository servicioFechasOfertaRepository, IColaboradorRepository colaboradorRepository, IServicioRepository servicioRepository, IConfiguracionRepository configuracionRepository, DbAb1c8aEnfermeriaContext context, IMapper mapper)
        {
            this.servicioFechasOfertaRepository = servicioFechasOfertaRepository;
            this.configuracionRepository = configuracionRepository;
            this.colaboradorRepository = colaboradorRepository;
            this.servicioFechaRepository = servicioFechaRepository;
            this.servicioRepository = servicioRepository;
            this._context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetServicioFecha([FromQuery] ServicioFechaFilter model)
        {
            
            FiltroGlobal filtro = new FiltroGlobal()
            {
                ServicioId = model.ServicioId,
                EstatusServicioFechaId = 1
            };


            //creamos la respuesta
            var response = new ResponseModel_2<List<ServicioFechaDto>>();
            filtro.IncluirInactivos = false;

            try
            {
                //colocamos los filtros
                var spec = new ServicioFechasSpecification(filtro);

                //colocamos los includes
                spec.IncludeStrings = new List<string> { "EstatusServicioFecha", "ServicioFechasOferta", "Servicio" };

                //convertimos de la clase al dto
                var result = await this.servicioFechaRepository.ListAsync(spec);
                var resultDto = mapper.Map<List<ServicioFechaDto>>(result);

                //seteamos el resultado
                response.SetResponse(true, "");
                response.Result = resultDto;

                return Ok(resultDto);
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

        [HttpGet("obtener-servicios-fechas")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetServiciosFechaByServicio([FromQuery] ServicioFechaFilter dto)
        {

            FiltroGlobal filtro = new FiltroGlobal()
            {
                ServicioId = dto.ServicioId
            };


            //creamos la respuesta
            var response = new ResponseModel_2<List<ObtenerServiciosFechasDto>>();
            filtro.IncluirInactivos = false;

            try
            {
                //colocamos los filtros
                var spec = new ServicioFechasSpecification(filtro);

                //colocamos los includes
                spec.IncludeStrings = new List<string> { "EstatusServicioFecha", "ServicioFechasOferta", "Servicio" };

                //convertimos de la clase al dto
                var result = await this.servicioFechaRepository.ListAsync(spec);
                var resultDto = mapper.Map<List<ObtenerServiciosFechasDto>>(result);

                //seteamos el resultado
                response.SetResponse(true, "");
                response.Result = resultDto;

                return Ok(resultDto.OrderBy(x=>x.FechaInicio));
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

        [HttpGet("obtener-guardias-fechas")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetServicioFechaFiltros([FromQuery] GetServicioFechaFiltrosDto model)
        {

            //creamos la respuesta
            var response = new ResponseModel_2<List<GetServicioFechaFiltrosResponse>>();
            try
            {
                Guid? id = null;
                var configuraciones = await this.configuracionRepository.ListAsync();

                decimal costosOperativos = (decimal)configuraciones.Where(x => x.Id == 10).FirstOrDefault().ValorDecimal;
                decimal retenciones = (decimal)configuraciones.Where(x => x.Id == 11).FirstOrDefault().ValorDecimal;
                if (model.ColaboradorAsignadoId != null)
                {
                    var colaboradores = await this.colaboradorRepository.ListAsync();
                    var colaborador = colaboradores.Where(x => x.No == model.ColaboradorAsignadoId).FirstOrDefault();
                    id = colaborador.Id;
                }

                FiltroGlobal filtro = new FiltroGlobal()
                {
                    ColaboradorAsignadoId = id,
                    EstatusServicioFechaId =(int)EstatusServicioFechaEnum.Completada,// model.EstatusServicioFechaId,
                    FechaInicio = model.Inicio,
                    FechaFin = model.Fin
                };
                filtro.IncluirInactivos = false;

                var spec = new ServicioFechasSpecification(filtro);

                //colocamos los includes
                spec.IncludeStrings = new List<string> { "EstatusServicioFecha", "ServicioFechasOferta", "Servicio", "ColaboradorAsignado" };

                //convertimos de la clase al dto
                var result = await this.servicioFechaRepository.ListAsync(spec);
                var resultDto = mapper.Map<List<GetServicioFechaFiltrosResponse>>(result);

                foreach(var item in resultDto) 
                {
                    item.CostosOperativos = (item.Total - item.Descuento) * (costosOperativos / decimal.Parse("100"));
                    item.Retenciones = (item.Total - item.Descuento) * (retenciones / decimal.Parse("100"));
                    item.ImporteBruto = (item.Total - item.Descuento) - item.Comision - item.Retenciones - item.CostosOperativos;
                }
                //seteamos el resultado
                response.SetResponse(true, "");
                response.Result = resultDto;

                return Ok(resultDto);
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

        [HttpGet("obtener-guardias")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetGuardias([FromQuery] GetGuardiasFilter model)
        {

            FiltroGlobal filtro = new FiltroGlobal()
            {
                noServicio = model.NoServicio,
                EstatusServicioFechaId = model.EstatusServicioFechaId,
                FechaInicio = model.FechaInicio,
                FechaFin = model.FechaFin,
                IncluirInactivos = true
            };


            //creamos la respuesta
            var response = new ResponseModel_2<List<GetGuardiasDto>>();
            
            try
            {
                //colocamos los filtros
                var spec = new ServicioFechasSpecification(filtro);

                //colocamos los includes
                spec.IncludeStrings = new List<string> { "EstatusServicioFecha", "ColaboradorAsignado", "Servicio" };

                //convertimos de la clase al dto
                var result = await this.servicioFechaRepository.ListAsync(spec);
                var resultDto = mapper.Map<List<GetGuardiasDto>>(result);

                //seteamos el resultado
                response.SetResponse(true, "");
                response.Result = resultDto;

                return Ok(resultDto.OrderBy(x=>x.FechaInicio));
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

        [HttpPut("{id}/liberar-oferta")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> LiberarOferta(Guid id)
        {
            var servicioFecha = await this.servicioFechaRepository.GetByIdAsync(id);

            if (servicioFecha == null)
                return NotFound();

            servicioFecha.ColaboradorAsignadoId = null;
            servicioFecha.EstatusServicioFechaId = 1;

            await this.servicioFechaRepository.UpdateAsync(servicioFecha);

            return Ok();
        }
        [HttpPut("{id}/terminar-oferta")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> TerminarOferta(Guid id)
        {
            var servicioFecha = await this.servicioFechaRepository.GetByIdAsync(id);

            if (servicioFecha == null)
                return NotFound();

            servicioFecha.EstatusServicioFechaId = 3;

            await this.servicioFechaRepository.UpdateAsync(servicioFecha);

            return Ok();
        }
        [HttpPut("{id}/cancelar-oferta")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CancelarOferta(Guid id)
        {
            var servicioFecha = await this.servicioFechaRepository.GetByIdAsync(id);

            if (servicioFecha == null)
                return NotFound();

            servicioFecha.EstatusServicioFechaId = 99;

            await this.servicioFechaRepository.UpdateAsync(servicioFecha);

            return Ok();
        }

        [HttpPut("{id}/asignar-oferta")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AsignarOferta(Guid id, [FromBody] AsignarFechaDto dto)
        {
            var servicioFechasOferta = await this.servicioFechasOfertaRepository.GetByIdAsync(dto.ServicioFechasOfertaId);
            var servicioFecha = await this.servicioFechaRepository.GetByIdAsync(id);

            if (servicioFechasOferta == null)
                return NotFound();

            servicioFecha.ColaboradorAsignadoId = servicioFechasOferta.ColaboradorId;
            servicioFecha.EstatusServicioFechaId = 2;

            await this.servicioFechaRepository.UpdateAsync(servicioFecha);

            return Ok();
        }
        [HttpPost("asignar-descuentos")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AsignarDescuentos([FromBody] List<AplicarDescuentoDto> dto)
        {
            using var transaction = await this._context.Database.BeginTransactionAsync();
            try
            {
                Guid servicioId = new Guid();
                decimal descuento = 0;
                foreach (var item in dto)
                {
                    var model = await this.servicioFechaRepository.GetByIdAsync(item.Id);
                    descuento += model.Descuento;
                    servicioId = model.ServicioId;
                    model.Descuento = item.Descuento;
                    await this.servicioFechaRepository.UpdateAsync(model);
                }

                var servicio = await this.servicioRepository.GetByIdAsync(servicioId);
                servicio.Descuento = descuento;
                await this.servicioRepository.UpdateAsync(servicio);
                //actualizamos el servicio con el total del descuento
                await transaction.CommitAsync();
            }
            catch (Exception ex) { 
                await transaction.RollbackAsync();
                return BadRequest("Ocurrio un error al aplicar los descuentos.");
            }

            return Ok();
        }
    }
}
