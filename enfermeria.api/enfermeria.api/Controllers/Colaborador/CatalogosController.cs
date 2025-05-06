using AutoMapper;
using enfermeria.api.Models.DTO;
using enfermeria.api.Models.DTO.CatMunicipio;
using enfermeria.api.Models.Specifications;
using enfermeria.api.Repositories.Implementation;
using enfermeria.api.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace enfermeria.api.Controllers.Colaborador
{
    [Route("api/colaborador/[controller]")]
    [ApiController]
    public class CatalogosController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IEstadoRepository estadoRepository;
        private readonly IMunicipioRepository municipioRepository;
        public CatalogosController(
            IEstadoRepository estadoRepository,
            IMunicipioRepository municipioRepository,
            IMapper mapper
            )
        {
            this.mapper = mapper;
            this.estadoRepository = estadoRepository;
            this.municipioRepository = municipioRepository;
        }

        [HttpGet("estados")]
        [Authorize(Roles = "Colaborador")]
        public async Task<IActionResult> GetEstados()
        {
            try
            {
                var response = await this.estadoRepository.GetActivos();

                if (!response.response)
                {
                    ModelState.AddModelError("error", response.message);
                    return ValidationProblem(ModelState);
                }

                return Ok(response.result);
            }
            catch (Exception ex)
            {
                BadRequest("Ocurrio un error inesperado.");
            }
            return Ok();

        }

        [HttpGet("municipios")]
        [Authorize(Roles = "Colaborador")]
        public async Task<IActionResult> GetMunicipios()
        {
            try
            {
                FiltroGlobal filtro = new FiltroGlobal() 
                { 
                    IncluirInactivos = false,
                };

                var spec = new CatMunicipioSpecification(filtro);

                var municipios = await municipioRepository.ListAsync(spec);
                var municipioDtos = mapper.Map<List<GetMunicipioDto>>(municipios);

                return Ok(municipioDtos);
            }
            catch (Exception ex)
            {
                BadRequest("Ocurrio un error inesperado.");
            }
            return Ok();

        }
    }
}
