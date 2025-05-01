using AutoMapper;
using enfermeria.api.Models.DTO.EncuestaPlantilla;
using enfermeria.api.Models.DTO.Paciente;
using enfermeria.api.Models;
using enfermeria.api.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using enfermeria.api.Models.DTO.EncuestaPlantillaPregunta;
using enfermeria.api.Helpers;
using enfermeria.api.Models.Domain;
using DocumentFormat.OpenXml.Office.PowerPoint.Y2021.M06.Main;

namespace enfermeria.api.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class EncuestaPlantillaPreguntaController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IEncuestaPlantillaPreguntaRepository encuestaPlantillaPreguntaRepository;

        public EncuestaPlantillaPreguntaController(IEncuestaPlantillaPreguntaRepository encuestaPlantillaPreguntaRepository, IMapper mapper)
        {
            this.encuestaPlantillaPreguntaRepository = encuestaPlantillaPreguntaRepository;
            this.mapper = mapper;
        }

        [HttpGet("{plantillaId}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetEncuestaPlantillaPregunta(Guid plantillaId)
        {
            //creamos la respuesta
            var response = new ResponseModel_2<List<GetEncuestaPlantillaPreguntaDto>>();

            try
            {
                //convertimos de la clase al dto
                var pacientes = await encuestaPlantillaPreguntaRepository.ListAsync();
                pacientes = pacientes.Where(x => x.PlantillaId == plantillaId).ToList();

                var pacientesDto = mapper.Map<List<GetEncuestaPlantillaPreguntaDto>>(pacientes);

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

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CrearEncuestaPlantillaPregunta([FromBody] CrearEncuestaPlantillaPreguntaDto dto)
        {
            var response = new ResponseModel_2<CrearEncuestaPlantillaPreguntaDto>();

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
                var paciente = mapper.Map<EncuestaPlantillaPreguntum>(dto);
                paciente.UsuarioCreacion = Guid.Parse(User.GetId());
                paciente.FechaCreacion = DateTime.Now;
                // Agregar el paciente al repositorio
                await encuestaPlantillaPreguntaRepository.AddAsync(paciente);

                // Establecer la respuesta de éxito
                response.SetResponse(true, "Pregunta creada correctamente.");

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
        [Authorize(Roles = "Administrador")]
        [HttpPut("{id}/desactivar")]
        public async Task<IActionResult> Desactivar(Guid id)
        {
            var response = new ResponseModel_2<GetPacienteDto>();

            try
            {
                // Obtener el paciente actual desde la base de datos
                UpdatePacienteDto dto;

                var paciente = await encuestaPlantillaPreguntaRepository.GetByIdAsync(id);
                if (paciente == null)
                {
                    return NotFound("Paciente no encontrado.");
                }

                // Solo actualizamos el campo 'Activo' a false
                paciente.Activo = false;
                paciente.UsuarioModificacion = Guid.Parse(User.GetId());
                paciente.FechaModificacion = DateTime.Now;
                // Guardamos los cambios

                await encuestaPlantillaPreguntaRepository.UpdateAsync(paciente);
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
                var encuestas = await encuestaPlantillaPreguntaRepository.ListAsync();

                var paciente = await encuestaPlantillaPreguntaRepository.GetByIdAsync(id);
                if (paciente == null)
                    return NotFound();

                paciente.Activo = true;
                paciente.UsuarioModificacion = Guid.Parse(User.GetId());
                paciente.FechaModificacion = DateTime.Now;
                await encuestaPlantillaPreguntaRepository.UpdateAsync(paciente);

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
        public async Task<IActionResult> UpdateEncuestaPregunta(Guid id, [FromBody] CrearEncuestaPlantillaPreguntaDto dto)
        {
            var response = new ResponseModel_2<GetPacienteDto>();

            try
            {
                // Validamos que el id en la ruta coincida con el del body
                var paciente = await encuestaPlantillaPreguntaRepository.GetByIdAsync(id);
                if (paciente == null)
                {
                    return NotFound("Paciente no encontrado.");
                }

                // Mapear solo los campos permitidos del DTO a la entidad
                mapper.Map(dto, paciente);


                await encuestaPlantillaPreguntaRepository.UpdateAsync(paciente);

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
