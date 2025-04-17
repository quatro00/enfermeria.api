using enfermeria.api.Repositories.Implementation;
using enfermeria.api.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace enfermeria.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoLugarController : ControllerBase
    {
        private readonly ITipoLugarRepository tipoLugarRepository;
        public TipoLugarController(ITipoLugarRepository tipoLugarRepository)
        {
            this.tipoLugarRepository = tipoLugarRepository;
        }

        [HttpGet]
        [Route("")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Get()
        {
            var response = await tipoLugarRepository.ListAsync();

            return Ok(response);
        }
    }
}
