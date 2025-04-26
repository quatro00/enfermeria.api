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
using enfermeria.api.Models.DTO.Pago;
using enfermeria.api.Enums;
using Microsoft.EntityFrameworkCore;
using enfermeria.api.Data;
using enfermeria.api.Models.PagoLote;
using enfermeria.api.Models.DTO;
using enfermeria.api.Models.Specifications;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace enfermeria.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagoLoteController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IPagoLoteRepository pagoLoteRepository;
        private readonly IServicioFechaRepository servicioFechasRepository;
        private readonly IPagoRepository pagoRepository;
        private readonly DbContext _context;

        public PagoLoteController(IPagoLoteRepository pagoLoteRepository, IMapper mapper, IServicioFechaRepository servicioFechasRepository, DbAb1c8aEnfermeriaContext context, IPagoRepository pagoRepository)
        {
            this.pagoLoteRepository = pagoLoteRepository;
            this.servicioFechasRepository = servicioFechasRepository;
            this.pagoRepository = pagoRepository;
            _context = context;
            this.mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CrearPagoLote([FromBody] CrearPagoLoteDto dto)
        {
            var response = new ResponseModel_2<Paciente>();

            if(dto.FechaInicio > dto.FechaFin)
            {
                return BadRequest("La fecha de inicio no puede ser mayor a la fecha de termino.");
            }
            
            // Validar si el modelo es válido
            if (!ModelState.IsValid)
            {
                response.SetResponse(false, "Modelo de datos inválido.");
                return BadRequest(response);
            }

            try
            {
                //iniciamos la transaccion
                using var transaction = await this._context.Database.BeginTransactionAsync();
                // Mapea el DTO a la entidad Paciente
                var result = mapper.Map<PagoLote>(dto);
                result.UsuarioCreacion = Guid.Parse(User.GetId());
                result.Pagos = new List<Pago>();
                
                foreach (var item in dto.Pagos)
                {
                    var servicioFecha = await this.servicioFechasRepository.GetByIdAsync(item);
                    result.Pagos.Add(new Pago() {
                        ServicioFechaId = servicioFecha.Id,
                        ImporteBruto = servicioFecha.ImporteBruto,
                        Comision = servicioFecha.Comision,
                        Retencion = servicioFecha.Retenciones,
                        CostoOperativo = servicioFecha.CostosOperativos,
                        Total = servicioFecha.Total,
                        EstatusPagoId = (int)EstatusPagoEnum.PorPagar,
                        Activo = true,
                        FechaCreacion = DateTime.Now,
                        UsuarioCreacion =Guid.Parse(User.GetId()),
                        
                    });

                    //marcamos pagado el serviciofecha
                    servicioFecha.EstatusServicioFechaId = (int)EstatusServicioFechaEnum.Pagado;
                    await this.servicioFechasRepository.UpdateAsync(servicioFecha);
                }

                // Devolver la respuesta con el nuevo paciente
                
                await this.pagoLoteRepository.AddAsync(result);
                await transaction.CommitAsync(); // 💾 Confirma todo si no falló nada
                return Ok();
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
        public async Task<IActionResult> GetPagosLote([FromQuery] GetPagoLoteDto model)
        {

            DateTime? fechaInicio = null;
            DateTime? fechaFin = null;

            if(model.Periodo != null)
            {
                fechaInicio = new DateTime(model.Periodo.Value.Year, model.Periodo.Value.Month, 1);
                fechaFin = fechaInicio.Value.AddMonths(1).AddDays(-1);
            }
            FiltroGlobal filtro = new FiltroGlobal()
            {
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                EstatusPagoLoteId= model.EstatusPagoLoteId
            };


            //creamos la respuesta
            var response = new ResponseModel_2<List<GetPagoLoteResponse>>();
            filtro.IncluirInactivos = false;

            try
            {
                //colocamos los filtros
                var spec = new PagoLoteSpecification(filtro);

                //colocamos los includes
                spec.IncludeStrings = new List<string> { "EstatosPagoLote", "Pagos", "Pagos.ServicioFecha", "Pagos.ServicioFecha.ColaboradorAsignado" };

                //convertimos de la clase al dto
                var result = await this.pagoLoteRepository.ListAsync(spec);
                var resultDto = mapper.Map<List<GetPagoLoteResponse>>(result);

                //seteamos el resultado
                response.SetResponse(true, "");
                response.Result = resultDto;

                return Ok(resultDto);
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

        [HttpPost("subir-deposito")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> SubirDeposito([FromForm] SubirPagoRequest dto)
        {
            var response = new ResponseModel_2<Paciente>();
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "pagos", dto.PagoLoteId.ToString());
            var rutaPublica = "";
            var avatar = "";

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            if (dto.Documento != null && dto.Documento.Length > 0)
            {

                var ext = Path.GetExtension(dto.Documento.FileName);
                var fileName = $"{Guid.NewGuid().ToString()}{ext}";
                var pathCompleto = Path.Combine(uploadPath, fileName);

                try
                {
                    using var stream = new FileStream(pathCompleto, FileMode.Create);
                    await dto.Documento.CopyToAsync(stream);

                    rutaPublica = pathCompleto;
                    


                }
                catch (Exception ex) { }




            }

            //buscamos todos los pagos correspondientes al lote y colaborador seleccionado
            FiltroGlobal filtro = new FiltroGlobal()
            {
                PagoLoteId = dto.PagoLoteId//,
                //ColaboradorAsignadoId = dto.ColaboradorId
            };


            // Validar si el modelo es válido
            if (!ModelState.IsValid)
            {
                response.SetResponse(false, "Modelo de datos inválido.");
                return BadRequest(response);
            }

            try
            {
                decimal totalPago = 0;
                var spec = new PagoSpecification(filtro);

                spec.IncludeStrings = new List<string> { "ServicioFecha" };
                
                var result = await this.pagoRepository.ListAsync(spec);
                var resultFiltrado = result.Where(x => x.ServicioFecha.ColaboradorAsignadoId == dto.ColaboradorId);

                totalPago = resultFiltrado.Sum(x => x.Total);

                if(result.Where(x=>x.Referencia == dto.Referencia).Any())
                {
                    return BadRequest("La referencia ya se encuentra registrada.");
                }
                if(totalPago != dto.Monto)
                {
                    return BadRequest($"El monto del pago { dto.Monto.ToString("C")} no corresponde al monto del deposito calculado {totalPago.ToString("C")}.");
                }
                //iniciamos la transaccion
                using var transaction = await this._context.Database.BeginTransactionAsync();
                foreach(var pago in resultFiltrado)
                {
                    pago.FechaPago = DateTime.Now;
                    pago.Comprobante = rutaPublica;
                    pago.Referencia = dto.Referencia;
                    pago.EstatusPagoId = (int)EstatusPagoEnum.Pagado;
                    await this.pagoRepository.UpdateAsync(pago);
                }

                result = await this.pagoRepository.ListAsync(spec);
                if(result.Where(x=>x.EstatusPagoId == 2).Count() == result.Count())
                {
                    var pagoLote = await this.pagoLoteRepository.GetByIdAsync(dto.PagoLoteId);
                    pagoLote.EstatosPagoLoteId = (int)EstatusPagoLoteEnum.Pagado;
                    await this.pagoLoteRepository.UpdateAsync(pagoLote);
                }
                await transaction.CommitAsync(); // 💾 Confirma todo si no falló nada
                return Ok();
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, manejar el error
                response.SetResponse(false, "Ocurrió un error al crear el pago.");

                // Puedes registrar el error o manejarlo como desees, por ejemplo:
                // Log.Error(ex, "Error al crear paciente");

                // Devolver una respuesta con el error
                response.Data = ex.Message; // Puedes agregar más detalles del error si lo deseas
                return StatusCode(500, response); // O devolver un BadRequest(400) si el error es de entrada
            }
        }


        [HttpGet("descargar-deposito")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DescargarDeposito(Guid pagoLoteId, string referencia)
        {
            /*
            var filePath = Path.Combine("Ruta/Donde/Guardaste/Los/Archivos", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, "application/octet-stream", fileName);
            */
            //-----------------------------
            FiltroGlobal filtro = new FiltroGlobal()
            {
                PagoLoteId = pagoLoteId
            };


            //creamos la respuesta
            var response = new ResponseModel_2<List<GetPagosDto>>();
            filtro.IncluirInactivos = false;

            try
            {
                //colocamos los filtros
                var spec = new PagoSpecification(filtro);

                //colocamos los includes
                spec.IncludeStrings = new List<string> { "ServicioFecha", "ServicioFecha.ColaboradorAsignado", "EstatusPago" };

                //convertimos de la clase al dto
                var result = await this.pagoRepository.ListAsync(spec);
                var pago = result.Where(x=>x.Referencia == referencia).FirstOrDefault();


                var filePath = pago.Comprobante;

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound();
                }

                var bytes = System.IO.File.ReadAllBytes(filePath);
                string extension = Path.GetExtension(filePath);

                return File(bytes, "application/octet-stream", referencia + extension);

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
