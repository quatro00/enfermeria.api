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

namespace enfermeria.api.Controllers.Colaborador
{
    [Route("api/colaborador/[controller]")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {
        public ServiciosController()
        {
        }



        [HttpGet("ver-servicios-proximos")]
        [Authorize(Roles = "Colaborador")]
        public async Task<IActionResult> GetDepositos()
        {
            try
            {
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
