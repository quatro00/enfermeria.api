using enfermeria.api.Helpers;
using enfermeria.api.Models.DTO.Estado;
using enfermeria.api.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace enfermeria.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoController : ControllerBase
    {
        private readonly IEstadoRepository estadoRepository;
        public EstadoController(IEstadoRepository estadoRepository)
        {
            this.estadoRepository = estadoRepository;
        }

        //endpoints de administrador
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Get()
        {
            var response = await estadoRepository.Get();

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
            var response = await estadoRepository.Get(id);

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
        public async Task<IActionResult> Create([FromBody] CreateEstado_Request model)
        {
            var response = await estadoRepository.Create(model, User.GetId());

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
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateEstado_Request model)
        {
            var response = await estadoRepository.Update(model, id, User.GetId());

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpPost]
        [Route("Activar")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Activar([FromBody] ActivarDesactivar_Request model)
        {
            var response = await estadoRepository.Activar(model.id);

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
            var response = await estadoRepository.Desactivar(model.id);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }
    }
}
