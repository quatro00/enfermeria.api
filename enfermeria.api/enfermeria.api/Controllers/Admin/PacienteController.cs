using AutoMapper;
using enfermeria.api.Data;
using enfermeria.api.Helpers;
using enfermeria.api.Models;
using enfermeria.api.Models.Domain;
using enfermeria.api.Models.DTO;
using enfermeria.api.Models.DTO.Paciente;
using enfermeria.api.Models.Specifications;
using enfermeria.api.Repositories.Implementation;
using enfermeria.api.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace enfermeria.api.Controllers.Admin
{
    [Route("api/admin/[controller]")]
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
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CrearPaciente([FromBody] CrearPacienteDto dto)
        {
            var response = new ResponseModel_2<Paciente>();

            //validaciones

            // Validar si el modelo es válido
            if (!ModelState.IsValid)
            {
                User.GetId();
                response.SetResponse(false, "Modelo de datos inválido.");
                return BadRequest(response);
            }

            try
            {
                //validamos que el telefono no se repita
                var existeTelefono = await pacienteRepository
                .AnyAsync(p => p.Telefono == dto.Telefono);

                if (existeTelefono) { return BadRequest("El teléfono ya se encuentra registrado con otro paciente."); }

                //validamos que el correo no se repita
                var existeCorreo = await pacienteRepository
                .AnyAsync(p => p.CorreoElectronico == dto.CorreoElectronico);

                if (existeCorreo) { return BadRequest("El correo ya se encuentra registrado con otro paciente."); }
                // Mapea el DTO a la entidad Paciente
                var paciente = mapper.Map<Paciente>(dto);
                paciente.UsuarioCreacion = Guid.Parse(User.GetId());

                var contactos = dto.Contactos.Select(contactoDto =>
                {
                    var contacto = mapper.Map<Contacto>(contactoDto);
                    // Asignar los campos de control a cada contacto
                    contacto.FechaCreacion = DateTime.UtcNow;
                    contacto.UsuarioCreacionId = Guid.Parse(User.GetId());
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

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetPacientes([FromQuery] FiltroGlobal filtro)
        {
            //creamos la respuesta
            var response = new ResponseModel_2<List<GetPacienteDto>>();
            if (User.IsInRole("Administrador"))
            {
                filtro.IncluirInactivos = true;
            }
            try
            {
                //colocamos los filtros
                var spec = new PacienteSpecification(filtro);

                //convertimos de la clase al dto
                var pacientes = await pacienteRepository.ListAsync(spec);
                var pacientesDto = mapper.Map<List<GetPacienteDto>>(pacientes);

                //seteamos el resultado
                response.SetResponse(true, "");
                response.Result = pacientesDto;

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
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetPacienteById(Guid id)
        {
            var response = new ResponseModel_2<GetPacienteDto>();

            try
            {
                var paciente = await pacienteRepository.GetByIdAsync(id);
                if (paciente == null)
                {
                    response.SetResponse(false, "Paciente no encontrado.");
                    return NotFound(response);
                }

                response.SetResponse(true, "");

                var pacientesDto = mapper.Map<GetPacienteDto>(paciente);
                response.Result = pacientesDto;

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

        [Authorize(Roles = "Administrador")]
        [HttpPut("{id}/desactivar")]
        public async Task<IActionResult> Desactivar(Guid id)
        {
            var response = new ResponseModel_2<GetPacienteDto>();

            try
            {
                // Obtener el paciente actual desde la base de datos
                UpdatePacienteDto dto;

                var paciente = await pacienteRepository.GetByIdAsync(id);
                if (paciente == null)
                {
                    return NotFound("Paciente no encontrado.");
                }

                // Solo actualizamos el campo 'Activo' a false
                paciente.Activo = false;
                paciente.UsuarioModificacion = Guid.Parse(User.GetId());
                paciente.FechaModificacion = DateTime.Now;
                // Guardamos los cambios

                await pacienteRepository.UpdateAsync(paciente);

                return NoContent(); // Respuesta exitosa sin contenido
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

        [Authorize(Roles = "Administrador")]
        [HttpPut("{id}/reactivar")]
        public async Task<IActionResult> Reactivar(Guid id)
        {
            var response = new ResponseModel_2<GetPacienteDto>();

            try
            {
                var paciente = await pacienteRepository.GetByIdAsync(id);
                if (paciente == null)
                    return NotFound();

                paciente.Activo = true;
                paciente.UsuarioModificacion = Guid.Parse(User.GetId());
                paciente.FechaModificacion = DateTime.Now;
                await pacienteRepository.UpdateAsync(paciente);

                return NoContent();
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

        [Authorize(Roles = "Administrador")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePaciente(Guid id, [FromBody] UpdatePacienteDto dto)
        {
            var response = new ResponseModel_2<GetPacienteDto>();

            try
            {
                // Validamos que el id en la ruta coincida con el del body
                if (id != dto.Id)
                {
                    return BadRequest("El ID proporcionado no coincide.");
                }

                var paciente = await pacienteRepository.GetByIdAsync(id);
                if (paciente == null)
                {
                    return NotFound("Paciente no encontrado.");
                }

                // Mapear solo los campos permitidos del DTO a la entidad
                mapper.Map(dto, paciente);


                await pacienteRepository.UpdateAsync(paciente);

                return NoContent();
            }
            catch (Exception ex)
            {
                response.SetResponse(false, "Ocurrió un error al actualizar el paciente.");
                response.Data = ex.Message;
                return StatusCode(500, response);
            }
        }
    }
}
