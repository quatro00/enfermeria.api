using AutoMapper;
using enfermeria.api.Helpers;
using enfermeria.api.Models.Domain;
using enfermeria.api.Models.DTO.Paciente;
using enfermeria.api.Models;
using enfermeria.api.Repositories.Implementation;
using enfermeria.api.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using enfermeria.api.Models.DTO.Colaborador;
using enfermeria.api.Enums;
using enfermeria.api.Models.DTO;
using enfermeria.api.Models.Specifications;
using System;
using DocumentFormat.OpenXml.Office.CustomUI;

namespace enfermeria.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColaboradorController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IColaboradorRepository colaboradorRepository;
        private readonly IColaboradorDocumentoRepository colaboradorDocumentoRepository;

        public ColaboradorController(IColaboradorRepository colaboradorRepository, IMapper mapper, IColaboradorDocumentoRepository colaboradorDocumentoRepository)
        {
            this.colaboradorRepository = colaboradorRepository;
            this.mapper = mapper;
            this.colaboradorDocumentoRepository = colaboradorDocumentoRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CrearColaborador([FromBody] CrearColaboradorDto dto)
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
                var colaborador = mapper.Map<Colaborador>(dto);
                colaborador.UsuarioCreacionId = Guid.Parse(User.GetId());

                List<RelEstadoColaborador> estadoColaboradors = new List<RelEstadoColaborador>();
                foreach(var item in dto.Estados)
                {
                    estadoColaboradors.Add(new RelEstadoColaborador() { 
                        EstadoId = item
                    });
                }

                colaborador.RelEstadoColaboradors = estadoColaboradors;


                //validaciones

                //correo no se repita
                var existeCorreo = await colaboradorRepository
                .AnyAsync(p => p.CorreoElectronico == dto.CorreoElectronico);

                if (existeCorreo)
                {
                    response.SetResponse(false, "Ya existe un colaborador registrado con ese correo electronico.");
                    return BadRequest(response);
                }

                // Agregar el paciente al repositorio
                await colaboradorRepository.AddAsync(colaborador);

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
        public async Task<IActionResult> GetColaboradores([FromQuery] FilterGetColaborador model)
        {
            if (model.Tipo == "0")
            {
                model.Tipo = null;
            }

            FiltroGlobal filtro = new FiltroGlobal()
            {
                CorreoElectronico = model.CorreoElectronico,
                Nombre = model.Nombre,
                Telefono = model.Telefono,
                TipoEnfermeraId = model.Tipo,
            };

            
            //creamos la respuesta
            var response = new ResponseModel_2<List<ColaboradorDto>>();
            if (User.IsInRole("Administrador"))
            {
                filtro.IncluirInactivos = true;
            }

            
            
            try
            {
                //colocamos los filtros
                var spec = new ColaboradorSpecification(filtro);

                //colocamos los includes
                spec.IncludeStrings = new List<string>
                    {
                        "EstatusColaborador",
                        "RelEstadoColaboradors.Estado"
                    };

                //convertimos de la clase al dto
                var pacientes = await colaboradorRepository.ListAsync(spec);
                var pacientesDto = mapper.Map<List<ColaboradorDto>>(pacientes);

                //seteamos el resultado
                response.SetResponse(true, "");
                response.Result = pacientesDto;

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

        [HttpPost("AdjuntarDocumentacion")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AdjuntarDocumentacion([FromForm] AdjuntarDocumentacionDto request)
        {
            
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "documentacion", request.Id.ToString().ToUpper());
            var rutasPublicas = new Dictionary<string, string>();
            var avatar = "";

            List<ColaboradorDocumento> colaboradorDocumentos = new List<ColaboradorDocumento>();
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            async Task GuardarArchivo(IFormFile archivo, string clave, int tipoDocumentoId)
            {
                if (archivo != null && archivo.Length > 0)
                {
                    
                    var ext = Path.GetExtension(archivo.FileName);
                    var fileName = $"{clave}{ext}";
                    var pathCompleto = Path.Combine(uploadPath, fileName);

                    try
                    {
                        using var stream = new FileStream(pathCompleto, FileMode.Create);
                        await archivo.CopyToAsync(stream);

                        var rutaPublica = $"{Request.Scheme}://{Request.Host}/uploads/{request.Id.ToString().ToUpper()}/{fileName}";
                        rutasPublicas[clave] = rutaPublica;

                        if(clave == "fotografia")
                        {
                            avatar = rutaPublica;
                        }
                        colaboradorDocumentos.Add(new ColaboradorDocumento()
                        {
                            ColaboradorId = request.Id,
                            TipoDocumentoId = tipoDocumentoId,
                            Descripcion = archivo.FileName,
                            Ruta = rutaPublica,
                            RutaFisica = pathCompleto,
                            NombreArchivo = archivo.FileName,
                            Activo = true,
                            FechaCreacion = DateTime.Now,
                            UsuarioCreacion = Guid.Parse(User.GetId()),
                        });

                        
                    }
                    catch (Exception ex) { }

                   
                }
            }


            await GuardarArchivo(request.Titulo, "titulo", (int)TipoDocumentoEnum.Titulo);
            await GuardarArchivo(request.Identificacion, "identificacion", (int)TipoDocumentoEnum.Identificacion);
            await GuardarArchivo(request.ComprobanteDeDomicilio, "comprobanteDomicilio", (int)TipoDocumentoEnum.ComprobanteDeDomicilio);
            await GuardarArchivo(request.Cedula, "cedula", (int)TipoDocumentoEnum.CedulaProfesional);
            await GuardarArchivo(request.ContratoFirmado, "contratoFirmado", (int)TipoDocumentoEnum.ContratoFirmado);
            await GuardarArchivo(request.Fotografia, "fotografia", (int)TipoDocumentoEnum.Fotografia);
            
            //buscamos el usurio para cambiar el estatus y agregar los documentos
            var colaborador = await this.colaboradorRepository.GetByIdAsync(request.Id);
            colaborador.Avatar = avatar;
            colaborador.EstatusColaboradorId = (int)EstatusColaboradorEnum.RegistroCompleto;
            
            colaborador.ColaboradorDocumentos = colaboradorDocumentos;
            await this.colaboradorRepository.UpdateAsync(colaborador);
            

            // Aquí guardas las URLs públicas en tu base de datos

            return Ok(new
            {
                request.Id,
                rutas = rutasPublicas
            });
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("{id}/ActivarColaborador")]
        public async Task<IActionResult> ActivarColaborador(Guid id)
        {
            var response = new ResponseModel_2<GetPacienteDto>();

            try
            {

                // Obtener el paciente actual desde la base de datos
                //UpdateContactoDto dto;
                var colaborador = await this.colaboradorRepository.GetByIdAsync(id);
                colaborador.EstatusColaboradorId = (int)EstatusColaboradorEnum.Activo;

                await this.colaboradorRepository.UpdateAsync(colaborador);
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

    }
}
