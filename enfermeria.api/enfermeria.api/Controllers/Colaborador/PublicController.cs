using AutoMapper;
using enfermeria.api.Helpers;
using enfermeria.api.Models.Domain;
using enfermeria.api.Models;
using enfermeria.api.Models.DTO.Colaborador;
using enfermeria.api.Models.DTO.Mensaje;
using enfermeria.api.Repositories.Implementation;
using enfermeria.api.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using iText.Layout.Element;

namespace enfermeria.api.Controllers.Colaborador
{
    [Route("api/colaborador/[controller]")]
    [ApiController]
    public class PublicController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IMensajeRepository mensajeRepository;
        private readonly IEmailService emailService;

        public PublicController(
            IMensajeRepository mensajeRepository,
            IEmailService emailService,
            IMapper mapper)
        {
            this.mensajeRepository = mensajeRepository;
            this.mapper = mapper;
            this.emailService = emailService;
        }

        [HttpPost("CrearMensaje")]
        public async Task<IActionResult> CrearMensaje([FromBody] CrearMensajeDto dto)
        {
            var response = new ResponseModel_2<Paciente>();

            //validaciones

            // Validar si el modelo es válido
            if (!ModelState.IsValid)
            {
                response.SetResponse(false, "Modelo de datos inválido.");
                return BadRequest(response);
            }

            try
            {

                // Mapea el DTO a la entidad Paciente
                var mensaje = mapper.Map<Mensaje>(dto);


                // Agregamos
                mensaje = await mensajeRepository.AddAsync(mensaje);

                await emailService.SendEmailAsync_NuevoMensaje(mensaje);
                // Establecer la respuesta de éxito
                response.SetResponse(true, "Mensaje creado correctamente.");

                // Devolver la respuesta con el nuevo paciente
                return Ok(response);
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
