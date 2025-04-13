using AutoMapper;
using enfermeria.api.Data;
using enfermeria.api.Helpers;
using enfermeria.api.Models;
using enfermeria.api.Models.Domain;
using enfermeria.api.Models.DTO.Paciente;
using enfermeria.api.Repositories.Implementation;
using enfermeria.api.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace enfermeria.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacienteController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IPacienteRepository pacienteRepository;

        public PacienteController(IPacienteRepository pacienteRepository, IMapper mapper)
        {
            this.pacienteRepository = pacienteRepository;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CrearPaciente([FromBody] CrearPacienteDto dto)
        {
            var response = new ResponseModel_2<Paciente>();

            // Validar si el modelo es válido
            if (!ModelState.IsValid)
            {
                User.GetId();
                response.SetResponse(false, "Modelo de datos inválido.");
                return BadRequest(response);
            }

            try
            {
                // Mapea el DTO a la entidad Paciente
                var paciente = mapper.Map<Paciente>(dto);
                paciente.UsuarioCreacion = Guid.NewGuid();// Guid.Parse(User.GetId());

                var contactos = dto.Contactos.Select(contactoDto =>
                {
                    var contacto = mapper.Map<Contacto>(contactoDto);
                    // Asignar los campos de control a cada contacto
                    contacto.FechaCreacion = DateTime.UtcNow;
                    contacto.UsuarioCreacionId = Guid.NewGuid();// Guid.Parse(User.GetId());
                    contacto.Activo = true;
                   // contacto.PacienteId = paciente.Id;  // Relacionar el contacto con el paciente

                    return contacto;
                }).ToList();
                // Agregar el paciente al repositorio
                await pacienteRepository.AddAsync(paciente);

                // Establecer la respuesta de éxito
                response.SetResponse(true, "Paciente creado correctamente.");

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPacienteById(Guid id)
        {
            var response = new ResponseModel_2<Paciente>();

            try
            {
                var paciente = await pacienteRepository.GetByIdAsync(id);
                if (paciente == null)
                {
                    response.SetResponse(false, "Paciente no encontrado.");
                    return NotFound(response);
                }

                response.SetResponse(true, "Paciente encontrado.");
                response.Result = paciente;

                return Ok(response);
            }
            catch (Exception ex) {
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
