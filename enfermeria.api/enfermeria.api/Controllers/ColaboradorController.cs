using enfermeria.api.Helpers;
using enfermeria.api.Models.DTO;
using enfermeria.api.Models.DTO.Colaborador;
using enfermeria.api.Models.DTO.Estado;
using enfermeria.api.Repositories.Implementation;
using enfermeria.api.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace enfermeria.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColaboradorController : ControllerBase
    {
        private readonly IColaboradorRepository colaboradorRepository;
        public ColaboradorController(IColaboradorRepository colaboradorRepository)
        {
            this.colaboradorRepository = colaboradorRepository;
        }

        [HttpPost]
        [Route("Activar")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Activar([FromBody] ActivarDesactivar_Request model)
        {
            var response = await colaboradorRepository.Activar(model.id);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpPost]
        [Route("Desactivar")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Desactivar([FromBody] ActivarDesactivar_Request model)
        {
            var response = await colaboradorRepository.Desactivar(model.id);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpPost]
        [Route("GetColaboradores")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetColaboradores(GetColaboradores_Request model)
        {
            var response = await colaboradorRepository.GetColaboradores(model);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpGet]
        [Route("")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Get()
        {
            var response = await colaboradorRepository.Get();

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var response = await colaboradorRepository.Get(id);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([FromBody] CreateColaborador_Request model)
        {
            var response = await colaboradorRepository.Create(model, User.GetId());

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateColaborador_Request model)
        {
            var response = await colaboradorRepository.Update(model, id, User.GetId());

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }
    }
}
