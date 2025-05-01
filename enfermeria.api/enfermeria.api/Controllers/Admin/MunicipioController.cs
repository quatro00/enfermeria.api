using AutoMapper;
using enfermeria.api.Models.DTO.Paciente;
using enfermeria.api.Models.DTO;
using enfermeria.api.Models.Specifications;
using enfermeria.api.Models;
using enfermeria.api.Repositories.Implementation;
using enfermeria.api.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using enfermeria.api.Models.DTO.CatMunicipio;

namespace enfermeria.api.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class MunicipioController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IMunicipioRepository municipioRepository;

        public MunicipioController(IMunicipioRepository municipioRepository, IMapper mapper)
        {
            this.municipioRepository = municipioRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetMunicipios([FromQuery] FiltroGlobal filtro)
        {
            //creamos la respuesta
            var response = new ResponseModel_2<List<GetMunicipioDto>>();
            if (User.IsInRole("Administrador"))
            {
                filtro.EstadoId = filtro.EstadoId;
                filtro.IncluirInactivos = true;
            }
            try
            {
                //colocamos los filtros
                var spec = new CatMunicipioSpecification(filtro);

                //convertimos de la clase al dto
                var pacientes = await municipioRepository.ListAsync(spec);
                var pacientesDto = mapper.Map<List<GetMunicipioDto>>(pacientes);

                //seteamos el resultado
                response.SetResponse(true, "");
                response.Result = pacientesDto;

                return Ok(pacientesDto);
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
