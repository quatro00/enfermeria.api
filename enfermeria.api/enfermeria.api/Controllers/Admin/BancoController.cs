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
using enfermeria.api.Models.Domain;
using enfermeria.api.Models.DTO.Banco;

namespace enfermeria.api.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class BancoController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IBancoRepository bancoRepository;

        public BancoController(IBancoRepository bancoRepository, IMapper mapper)
        {
            this.bancoRepository = bancoRepository;
            this.mapper = mapper;

        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetBancos()
        {
            //creamos la respuesta
            var response = new ResponseModel_2<List<BancoDto>>();

            try
            {
                //colocamos los filtros

                //convertimos de la clase al dto
                var bancos = await bancoRepository.ListAsync();
                var bancosDto = mapper.Map<List<BancoDto>>(bancos);

                //seteamos el resultado
                response.SetResponse(true, "");
                response.Result = bancosDto;

                return Ok(response.Result);
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
