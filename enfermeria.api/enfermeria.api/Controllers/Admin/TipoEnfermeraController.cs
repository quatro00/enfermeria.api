using enfermeria.api.Helpers;
using enfermeria.api.Models.DTO;
using enfermeria.api.Models.DTO.Estado;
using enfermeria.api.Models.DTO.TipoEnfermera;
using enfermeria.api.Repositories.Implementation;
using enfermeria.api.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace enfermeria.api.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class TipoEnfermeraController : ControllerBase
    {
        private readonly ITipoEnfermeraRepository tipoEnfermeraRepository;
        public TipoEnfermeraController(ITipoEnfermeraRepository tipoEnfermeraRepository)
        {
            this.tipoEnfermeraRepository = tipoEnfermeraRepository;
        }

        [HttpGet]
        [Route("")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Get()
        {
            var response = await tipoEnfermeraRepository.Get();

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpGet]
        [Route("GetActivos")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetActivos()
        {
            var response = await tipoEnfermeraRepository.GetActivos();

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
            var response = await tipoEnfermeraRepository.Get(id);

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
        public async Task<IActionResult> Create([FromBody] CreateTipoEnfermera_Request model)
        {
            var response = await tipoEnfermeraRepository.Create(model, User.GetId());

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
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTipoEnfermera_Request model)
        {
            var response = await tipoEnfermeraRepository.Update(model, id, User.GetId());

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
            var response = await tipoEnfermeraRepository.Activar(model.id);

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
            var response = await tipoEnfermeraRepository.Desactivar(model.id);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }
    }
}
