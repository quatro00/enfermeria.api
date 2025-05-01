using AutoMapper;
using enfermeria.api.Models.DTO.Paciente;
using enfermeria.api.Models;
using enfermeria.api.Repositories.Implementation;
using enfermeria.api.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using enfermeria.api.Models.DTO.Contacto;
using enfermeria.api.Helpers;

namespace enfermeria.api.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class ContactoController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IContactoRepository contactoRepository;

        public ContactoController(IContactoRepository contactoRepository, IMapper mapper)
        {
            this.contactoRepository = contactoRepository;
            this.mapper = mapper;
        }

        [HttpGet("GetByPacienteId/{pacienteId}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetByPacienteId(Guid pacienteId)
        {
            var response = new ResponseModel_2<List<GetContactoDto>>();

            try
            {
                var contactos = await contactoRepository.GetByPaciente(pacienteId);
                if (contactos == null)
                {
                    response.SetResponse(false, "Contactos no encontrados.");
                    return NotFound(response);
                }

                response.SetResponse(true, "");

                var dto = mapper.Map<List<GetContactoDto>>(contactos);
                response.Result = dto;

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
                //UpdateContactoDto dto;

                var contacto = await contactoRepository.GetByIdAsync(id);
                if (contacto == null)
                {
                    return NotFound("Contacto no encontrado.");
                }

                // Solo actualizamos el campo 'Activo' a false
                contacto.Activo = false;
                contacto.UsuarioModificacionId = Guid.Parse(User.GetId());
                contacto.FechaModificacion = DateTime.Now;
                // Guardamos los cambios

                await contactoRepository.UpdateAsync(contacto);

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
                // Obtener el paciente actual desde la base de datos
                //UpdateContactoDto dto;

                var contacto = await contactoRepository.GetByIdAsync(id);
                if (contacto == null)
                {
                    return NotFound("Contacto no encontrado.");
                }

                // Solo actualizamos el campo 'Activo' a false
                contacto.Activo = true;
                contacto.UsuarioModificacionId = Guid.Parse(User.GetId());
                contacto.FechaModificacion = DateTime.Now;
                // Guardamos los cambios

                await contactoRepository.UpdateAsync(contacto);

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
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContacto(Guid id, [FromBody] UpdateContactoDto dto)
        {
            var response = new ResponseModel_2<GetContactoDto>();

            try
            {
                // Validamos que el id en la ruta coincida con el del body
                if (id != dto.Id)
                {
                    return BadRequest("El ID proporcionado no coincide.");
                }

                var paciente = await contactoRepository.GetByIdAsync(id);
                if (paciente == null)
                {
                    return NotFound("Paciente no encontrado.");
                }

                // Mapear solo los campos permitidos del DTO a la entidad
                mapper.Map(dto, paciente);


                await contactoRepository.UpdateAsync(paciente);

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
