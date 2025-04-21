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

namespace enfermeria.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioFechaController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IServicioFechaRepository servicioFechaRepository;
        private readonly IServicioFechasOfertaRepository servicioFechasOfertaRepository;

        public ServicioFechaController(IServicioFechaRepository servicioFechaRepository, IServicioFechasOfertaRepository servicioFechasOfertaRepository, IMapper mapper)
        {
            this.servicioFechasOfertaRepository = servicioFechasOfertaRepository;
            this.servicioFechaRepository = servicioFechaRepository;
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
    }
}
