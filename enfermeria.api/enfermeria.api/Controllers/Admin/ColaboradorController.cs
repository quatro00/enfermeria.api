﻿using AutoMapper;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using enfermeria.api.Data;

namespace enfermeria.api.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class ColaboradorController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IColaboradorRepository colaboradorRepository;
        private readonly IColaboradorDocumentoRepository colaboradorDocumentoRepository;
        private readonly UserManager<IdentityUser> userManager;

        public ColaboradorController(IColaboradorRepository colaboradorRepository, IMapper mapper, IColaboradorDocumentoRepository colaboradorDocumentoRepository, UserManager<IdentityUser> userManager)
        {
            this.colaboradorRepository = colaboradorRepository;
            this.mapper = mapper;
            this.colaboradorDocumentoRepository = colaboradorDocumentoRepository;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CrearColaborador([FromBody] CrearColaboradorDto dto)
        {
            var response = new ResponseModel_2<Paciente>();
            dto.Rfc = dto.Rfc.ToUpper();
            dto.Curp = dto.Curp.ToUpper();
            dto.CedulaProfesional = dto.CedulaProfesional.ToUpper();
            dto.Cuenta = dto.Cuenta.ToUpper();
            dto.Clabe = dto.Clabe.ToUpper();
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
                var colaborador = mapper.Map<enfermeria.api.Models.Domain.Colaborador>(dto);
                colaborador.UsuarioCreacionId = Guid.Parse(User.GetId());

                List<RelEstadoColaborador> estadoColaboradors = new List<RelEstadoColaborador>();
                foreach (var item in dto.Estados)
                {
                    estadoColaboradors.Add(new RelEstadoColaborador()
                    {
                        EstadoId = item
                    });
                }

                colaborador.RelEstadoColaboradors = estadoColaboradors;


                //validaciones

                //correo no se repita
                var existeCorreo = await colaboradorRepository
                .AnyAsync(p => p.CorreoElectronico == dto.CorreoElectronico);

                if (existeCorreo) { return BadRequest("Ya existe un colaborador registrado con ese correo electronico."); }

                //validamos que el telefono no se repita
                var existeTelefono = await colaboradorRepository
                .AnyAsync(p => p.Telefono == dto.Telefono);

                if (existeTelefono) { return BadRequest("Ya existe un colaborador registrado con ese teléfono."); }

                //Validamos que el rfc no se repita
                var existeRfc = await colaboradorRepository
                .AnyAsync(p => p.Rfc == dto.Rfc);

                if (existeRfc) { return BadRequest("Ya existe un colaborador registrado con ese rfc."); }

                //Validamos que el rfc no se repita
                var existeCurp = await colaboradorRepository
                .AnyAsync(p => p.Curp == dto.Curp);

                if (existeCurp) { return BadRequest("Ya existe un colaborador registrado con ese curp."); }



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

                        if (clave == "fotografia")
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
            var colaborador = await colaboradorRepository.GetByIdAsync(request.Id);
            colaborador.Avatar = avatar;
            colaborador.EstatusColaboradorId = (int)EstatusColaboradorEnum.RegistroCompleto;

            colaborador.ColaboradorDocumentos = colaboradorDocumentos;
            await colaboradorRepository.UpdateAsync(colaborador);


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
                var colaborador = await colaboradorRepository.GetByIdAsync(id);
                colaborador.EstatusColaboradorId = (int)EstatusColaboradorEnum.Activo;

                await colaboradorRepository.UpdateAsync(colaborador);
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
        [HttpPut("{id}/CrearCuenta")]
        public async Task<IActionResult> CrearCuenta(Guid id)
        {
            var response = new ResponseModel_2<GetPacienteDto>();

            try
            {
                var colaborador = await this.colaboradorRepository.GetByIdAsync(id);
                if (colaborador == null)
                    return NotFound("Colaborador no encontrado");

                var usuarioExistente = await this.userManager.FindByEmailAsync(colaborador.CorreoElectronico);
                if (usuarioExistente != null)
                    return BadRequest("Ya existe una cuenta con este correo.");

                var usuario = new IdentityUser
                {
                    UserName = colaborador.CorreoElectronico,
                    Email = colaborador.CorreoElectronico,
                    NormalizedEmail = colaborador.CorreoElectronico.ToUpper(),
                    NormalizedUserName = colaborador.CorreoElectronico.ToUpper(),
                };

                var resultado = await this.userManager.CreateAsync(usuario, "password");

                if (!resultado.Succeeded)
                    return BadRequest(resultado.Errors);

                await this.userManager.AddToRoleAsync(usuario, "Colaborador");

                colaborador.UserId = usuario.Id;
                colaborador.CuentaCreada = true;
                colaborador.EstatusColaboradorId = (int)EstatusColaboradorEnum.Activo;

                await this.colaboradorRepository.UpdateAsync(colaborador);

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
        

    }
}
