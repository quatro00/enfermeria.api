using AutoMapper;
using enfermeria.api.Models.DTO.ServicioFecha;
using enfermeria.api.Models.DTO;
using enfermeria.api.Models.Specifications;
using enfermeria.api.Models;
using enfermeria.api.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using enfermeria.api.Models.DTO.ServicioFechaOferta;

namespace enfermeria.api.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class ServicioFechasOfertaController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IServicioFechasOfertaRepository servicioFechasOfertaRepository;

        public ServicioFechasOfertaController(IServicioFechasOfertaRepository servicioFechasOfertaRepository, IMapper mapper)
        {
            this.servicioFechasOfertaRepository = servicioFechasOfertaRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetServicioFecha([FromQuery] GetServicioFechaOfertaFilter model)
        {

            FiltroGlobal filtro = new FiltroGlobal()
            {
                ServicioFechaId = model.servicioFechaId,
                EstatusOfertaId = 1
            };


            //creamos la respuesta
            var response = new ResponseModel_2<List<GetServicioFechaOfertaDto>>();

            //colocamos los filtros
            filtro.IncluirInactivos = false;
            filtro.EstatusOfertaId = 1;


            try
            {
                //colocamos los filtros
                var spec = new ServicioFechasOfertaSpecification(filtro);

                //colocamos los includes
                spec.IncludeStrings = new List<string> { "Colaborador", "EstatusOferta", "ServicioFecha", "Colaborador.TipoEnfermera" };

                //convertimos de la clase al dto
                var result = await servicioFechasOfertaRepository.ListAsync(spec);
                var resultDto = mapper.Map<List<GetServicioFechaOfertaDto>>(result);

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
    }
}
