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
using enfermeria.api.Models.DTO.Servicio;
using enfermeria.api.Models.DTO.Banco;
using DocumentFormat.OpenXml.Presentation;
using enfermeria.api.Helpers.Cotizacion;
using enfermeria.api.Models.DTO.TipoEnfermera;
using enfermeria.api.Models.DTO.Estado;
using enfermeria.api.Models.DTO.Colaborador;
using enfermeria.api.Models.DTO;
using enfermeria.api.Models.Specifications;
using enfermeria.api.Models.DTO.Mail;
using System.Net.Mail;
using System.Net.Mime;
using enfermeria.api.Repositories.OpenPay;
using enfermeria.api.Repositories.Stripe;
using Microsoft.EntityFrameworkCore;

using Stripe;


using Stripe;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Stripe.Checkout;
using enfermeria.api.Models.DTO.ServicioFecha;
using enfermeria.api.Enums;
using enfermeria.api.Models.DTO.Pago;

namespace enfermeria.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IServicioRepository servicioRepository;
        private readonly IHorarioRepository horarioRepository;
        private readonly ITipoEnfermeraRepository tipoEnfermeraRepository;
        private readonly IEstadoRepository estadoRepository;
        private readonly ITipoLugarRepository tipoLugarRepository;
        private readonly IPacienteRepository pacienteRepository;
        private readonly IConfiguracionRepository configuracionRepository;
        private readonly IEmailService emailService;
        private readonly IColaboradorRepository colaboradorRepository;

        private readonly StripePaymentService stripePaymentService;
        private readonly string _stripeWebhookSecret = "whsec_I3NrLFcnZtqBwtf2pDh1LWLHICg5p8aU";

        public ServicioController(
            IServicioRepository servicioRepository, 
            IMapper mapper, 
            IHorarioRepository horarioRepository, 
            ITipoEnfermeraRepository tipoEnfermeraRepository, 
            IEstadoRepository estadoRepository, 
            ITipoLugarRepository tipoLugarRepository,
            IPacienteRepository pacienteRepository,
            IConfiguracionRepository configuracionRepository,
            IEmailService emailService,
            IColaboradorRepository colaboradorRepository
            )
        {
            this.colaboradorRepository = colaboradorRepository;
            this.servicioRepository = servicioRepository;
            this.horarioRepository = horarioRepository;
            this.tipoEnfermeraRepository = tipoEnfermeraRepository;
            this.estadoRepository = estadoRepository;
            this.tipoLugarRepository = tipoLugarRepository;
            this.pacienteRepository = pacienteRepository;
            this.emailService = emailService;
            this.configuracionRepository = configuracionRepository;
            this.mapper = mapper;
            this.stripePaymentService = new StripePaymentService("sk_test_51Qj6iqRskzWiZsX2j1arurF8bKGpmMTOfTcgrsV5i10BhW5gmEzwy3SPCTmfueEIjYqXxlXimCRQP3Fc13A1QMc500y1Ii9JOY");
        }

        [HttpPost("enviar-cotizacion/{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EnviarCotizacionPorCorreo(Guid id, [FromQuery] string correoAdicional)
        {
            // 1. Obtener la cotización con los includes necesarios
            var servicio = await servicioRepository.GetByIdAsync(
                id,
                "Id",
                "Municipio",
                "Municipio.Estado",
                "TipoLugar",
                "TipoEnfermera",
                "Paciente",
                "ServicioFechas",
                "ServicioFechas.ServicioCotizacions"
            );

            if (servicio == null)
                return NotFound("No se encontró la cotización.");

            var configuraciones = await this.configuracionRepository.ListAsync();
            decimal limiteMonto = (decimal)configuraciones.Where(x=>x.Id == 8).FirstOrDefault().ValorDecimal;
            bool excedeMonto = false;
            
            string link = "";
            string cuenta = "";
            cuenta = configuraciones.Where(x => x.Id == 9).FirstOrDefault().ValorString ?? "";

            if (servicio.Total - servicio.Descuento > limiteMonto) { excedeMonto = true; }
            
            if(excedeMonto == false)
            {
                link = await this.stripePaymentService.CreateCheckoutSessionAsync(
                servicio.Total - servicio.Descuento,
                $"Pago servicio {servicio.No.ToString()}",
                "https://tusitio.com/pago-exitoso",
                "https://tusitio.com/pago-cancelado",
                servicio.Id.ToString()
               );
            }
            
           


            // 2. Generar PDF temporalmente
            var fileName = $"Cotizacion_{servicio.No}.pdf";
            var filePath = Path.Combine(Path.GetTempPath(), fileName);
            CotizacionPdfGenerator.GenerarPdf(servicio, filePath);

            // 3. Leer PDF como adjunto
            var pdfBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            var stream = new MemoryStream(pdfBytes);
            var attachment = new Attachment(stream, fileName, MediaTypeNames.Application.Pdf);

            var destinatarios = correoAdicional.Split(",").ToList();
            //destinatarios.Add(servicio.Paciente.CorreoElectronico);
            

            // 4. Enviar correo
            var request = new EmailRequest
            {
                ToMultiple = destinatarios,
                //To = correoAdicional,
                Subject = $"Cotización de servicio #{servicio.No}",
                Body = "<p>Adjuntamos su cotización de servicio de enfermería.</p>",
                Attachments = new List<Attachment> { attachment },
                
            };

            if (excedeMonto == false)
            {
                await emailService.SendEmailAsync(request, link);
            }
            else
            {
                await emailService.SendEmailAsync_Cuenta(request, cuenta);
            }
            

            return NoContent();
        }
        [HttpPost("aplicar-descuento/{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AplicarDescuento(Guid id, [FromQuery] decimal monto)
        {
            // 1. Obtener la cotización con los includes necesarios
            var servicio = await servicioRepository.GetByIdAsync(
                id,
                "Id",
                "Estado",
                "TipoLugar",
                "TipoEnfermera",
                "Paciente",
                "ServicioFechas",
                "ServicioFechas.ServicioCotizacions"
            );

            if (servicio == null)
                return NotFound("No se encontró la cotización.");

            servicio.Descuento = monto;
            await this.servicioRepository.UpdateAsync( servicio );

            return NoContent();
        }

        [HttpGet("ObtenerCotizacion/{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ObtenerCotizacion(Guid id)
        {
            // 1. Obtener los datos de la cotización (reemplaza esto por tu lógica real)
            var servicio = await this.servicioRepository.GetByIdAsync(
                id,
               "Id",
               "Municipio",
                "Municipio.Estado",
                "TipoLugar",
                "TipoEnfermera",
                "Paciente",
                "ServicioFechas",
                "ServicioFechas.ServicioCotizacions"
            );


            if (servicio == null)
                return NotFound();

            var fileName = $"Cotizacion_{servicio.No.ToString()}.pdf";
            var filePath = Path.Combine(Path.GetTempPath(), fileName);

            // Generar el PDF
            CotizacionPdfGenerator.GenerarPdf(servicio, filePath);

            // Devolverlo como archivo descargable
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            

            // 3. Retornar el PDF como archivo descargable
            return File(fileBytes, "application/pdf", $"cotizacion-{id}.pdf");
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CrearServicio([FromBody] CrearServicioDto dto)
        {
            var response = new ResponseModel_2<Servicio>();

            // Validar si el modelo es válido
            if (!ModelState.IsValid)
            {
                User.GetId();
                response.SetResponse(false, "Modelo de datos inválido.");
                return BadRequest(response);
            }

            try
            {
                var configuraciones = await this.configuracionRepository.ListAsync();
                var configuracion = configuraciones.Where(x => x.Id == 7).FirstOrDefault();


                var servicio = mapper.Map<Servicio>(dto);
                servicio.UsuarioCreacion = Guid.Parse(User.GetId());
                servicio.Vigencia = DateTime.Now.AddDays(configuracion.ValorEntero ?? 1).Date;
                servicio.Lat = "";
                servicio.Lon = "";

                //buscamos al tipo de enfermera para ver el costo
                var horarios = await this.horarioRepository.ListAsync();
                var tipoEnfermera_response = await tipoEnfermeraRepository.Get(dto.tipoEnfermeraId);
                var estado_response = await estadoRepository.Get(dto.estadoId);
                var tipoLugar_List = await this.tipoLugarRepository.ListAsync();
                var tipoLugar = tipoLugar_List.Where(x=>x.Id == dto.tipoLugarId).FirstOrDefault();
                var paciente = await this.pacienteRepository.GetByIdAsync(dto.pacienteId);



                var serviciosCotizacion = new List<ServicioCotizacion>();

                GetTipoEnfermera_Response tipoEnfermera = tipoEnfermera_response.result;
                GetEstado_Response estado = estado_response.result;

                decimal costoPorHora = tipoEnfermera.costoHora;

                List<CrearServicioFechasFormatoDto> listaSalida = dto.servicioFechasDtos.Select(e => new CrearServicioFechasFormatoDto
                {
                    fechaInicio = DateTime.Parse($"{e.fecha}T{e.inicio}"),
                    fechaTermino = DateTime.Parse($"{e.fecha}T{e.termino}"),
                    cantidadHoras = e.horas,
                    precioHora = tipoEnfermera.costoHora,
                    descuentos = 0,
                    subTotal = tipoEnfermera.costoHora * e.horas,
                    total = tipoEnfermera.costoHora * e.horas,
                }).ToList();

                //mapeamos las fechas
                var fechas = mapper.Map<List<ServicioFecha>>(listaSalida);
                

               
                decimal cantidadHoras = 0;
                foreach (var fecha in fechas)
                {
                    fecha.UsuarioCreacion = Guid.Parse(User.GetId());
                    fecha.EstatusServicioFechaId = (int)EstatusServicioFechaEnum.PorAsignar;
                    fecha.Descuento = 0;
                }

                

                //calculamos los horarios
                var fechasHorarios = CalculoCotizacion.CalcularHorasPorTurnoPorFecha(horarios, fechas);
                //colocamos los horarios para las fechas
                decimal subTotalPropuesto = 0;
                foreach(var item in fechasHorarios)
                {
                    serviciosCotizacion = new List<ServicioCotizacion>();

                    foreach(var itemDet in item.DetallePorTurno)
                    {
                        decimal factor = 1;
                        var horario = horarios.Where(x=>x.Descripcion == itemDet.Horario).FirstOrDefault();
                        if(horario != null) { factor = horario.PorcentajeTarifa; }
                        

                        var servicioCotizacion = new ServicioCotizacion()
                        {
                            Horas = itemDet.Horas,
                            Horario = itemDet.Horario,
                            PrecioHoraBase = tipoEnfermera.costoHora,
                            PrecioHoraFinal = tipoEnfermera.costoHora * factor,
                            PrecioFinal = (tipoEnfermera.costoHora * factor) * itemDet.Horas,
                            Activo = true,
                            FechaCreacion = DateTime.Now,
                            UsuarioCreacion = Guid.Parse(User.GetId()),
                        };

                        subTotalPropuesto = subTotalPropuesto + servicioCotizacion.PrecioFinal;
                        cantidadHoras = cantidadHoras + servicioCotizacion.Horas;
                        serviciosCotizacion.Add(servicioCotizacion);
                    }
                    
                    fechas.Where(x=>x.FechaInicio.Date == item.Fecha).FirstOrDefault().ServicioCotizacions = serviciosCotizacion;
                }
                
                // Devolver la respuesta con el nuevo paciente
                servicio.ServicioFechas = fechas;
                servicio.TotalHoras = cantidadHoras;
                servicio.SubTotalPropuesto = subTotalPropuesto;
                servicio.Impuestos = 0;
                servicio.Descuento = 0;
                servicio.CostoEstimadoHora = servicio.SubTotalPropuesto/servicio.TotalHoras;
                servicio.Total = servicio.SubTotalPropuesto + servicio.Impuestos - servicio.Descuento;

                var res = await servicioRepository.AddAsync(servicio);

                // Establecer la respuesta de éxito
                response.SetResponse(true, "Paciente creado correctamente.");
                return Ok(servicio.Id);
                //// Devolver la respuesta con el nuevo paciente

                //// Generar un nombre temporal para el PDF
                //servicio.Estado = new CatEstado() { Nombre = estado.nombre, NombreCorto = estado.nombreCorto, };
                //servicio.TipoEnfermera = new CatTipoEnfermera() { Descripcion = tipoEnfermera.descripcion, CostoHora = tipoEnfermera.costoHora , };
                //servicio.TipoLugar = tipoLugar;
                //servicio.Paciente = paciente;

                //var fileName = $"Cotizacion_{servicio.No.ToString()}.pdf";
                //var filePath = Path.Combine(Path.GetTempPath(), fileName);

                //// Generar el PDF
                //CotizacionPdfGenerator.GenerarPdf(servicio, filePath);

                //// Devolverlo como archivo descargable
                //var fileBytes = System.IO.File.ReadAllBytes(filePath);
                ////return File(fileBytes, "application/pdf", fileName);
               

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
        public async Task<IActionResult> GetServicios([FromQuery] FilterGetServicio model)
        {
            
            FiltroGlobal filtro = new FiltroGlobal()
            {
                noServicio = model.No,
                Nombre = model.NombrePaciente,
                EstadoId = model.Estado,
                EstatusServicioId = model.Estatus,
            };


            //creamos la respuesta
            var response = new ResponseModel_2<List<GetCotizacionResult>>();
            if (User.IsInRole("Administrador"))
            {
                filtro.IncluirInactivos = true;
            }

            try
            {
                
                //colocamos los filtros
                var spec = new ServicioSpecification(filtro);

                //colocamos los includes
                spec.IncludeStrings = new List<string>
                    {
                         "Municipio",
                        "TipoLugar",
                        "TipoEnfermera",
                        "Paciente",
                        "ServicioFechas",
                        "ServicioFechas.ServicioCotizacions",
                        "EstatusServicio"
                    };
                
                //convertimos de la clase al dto
                var pacientes = await this.servicioRepository.ListAsync(spec);
                var pacientesDto = mapper.Map<List<GetCotizacionResult>>(pacientes);
                
                return Ok(pacientesDto);
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
        [HttpPut("{id}/cancelar-cotizacion")]
        public async Task<IActionResult> Cancelar(Guid id)
        {
            var response = new ResponseModel_2<GetPacienteDto>();

            try
            {
                // Obtener el paciente actual desde la base de datos
                //UpdateContactoDto dto;

                var contacto = await this.servicioRepository.GetByIdAsync(id);
                if (contacto == null)
                {
                    return NotFound("Contacto no encontrado.");
                }

                // Solo actualizamos el campo 'Activo' a false
                contacto.EstatusServicioId = 99;
                // Guardamos los cambios

                await servicioRepository.UpdateAsync(contacto);

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
        [HttpPost("adjuntar-referencia")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AdjuntarReferencia([FromForm] AdjuntarReferenciaDto request)
        {
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "pagos", request.ServicioId.ToString().ToUpper());
            var rutasPublicas = new Dictionary<string, string>();
            var avatar = "";
            string rutaArchivo = "";
            List<ColaboradorDocumento> colaboradorDocumentos = new List<ColaboradorDocumento>();
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            async Task GuardarArchivo(IFormFile archivo)
            {
                if (archivo != null && archivo.Length > 0)
                {

                    var ext = Path.GetExtension(archivo.FileName);
                    var fileName = $"{Guid.NewGuid().ToString().ToUpper()}{ext}";
                    var pathCompleto = Path.Combine(uploadPath, fileName);

                    try
                    {
                        using var stream = new FileStream(pathCompleto, FileMode.Create);
                        await archivo.CopyToAsync(stream);

                        rutaArchivo = pathCompleto;
                    }
                    catch (Exception ex) { }


                }
            }

            await GuardarArchivo(request.Transferencia);

            //buscamos el servicio
            var servicio = await this.servicioRepository.GetByIdAsync(request.ServicioId);
            servicio.ReferenciaTransferencia = request.Referencia;
            servicio.Transferencia = rutaArchivo;
            servicio.EstatusServicioId = 2;

            await this.servicioRepository.UpdateAsync(servicio);

            return NoContent();
        }

        [HttpGet("descargar-pago")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DescargarPago(Guid servicioId)
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
            
            //creamos la respuesta
            var response = new ResponseModel_2<List<GetPagosDto>>();
            
            try
            {
                //colocamos los filtros

                var servicio = await this.servicioRepository.GetByIdAsync(servicioId);
                var transferenciaPath = servicio.Transferencia ?? "";
                if(transferenciaPath == "")
                {
                    return BadRequest("El pago no fue hecho con transferencia.");
                }
                
                var filePath =servicio.Transferencia;


                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound();
                }

                var bytes = System.IO.File.ReadAllBytes(filePath);
                string extension = Path.GetExtension(filePath);

                return File(bytes, "application/octet-stream", servicio.ReferenciaTransferencia + extension);

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

        //-----------Pagos confirmacion-------
        // Endpoint para recibir los eventos del Webhook
        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            Event stripeEvent;

            try
            {
                // Verificar que la firma del webhook sea válida
                // El encabezado Stripe-Signature debe estar presente y el webhook secreto es necesario para la validación
                var signature = Request.Headers["Stripe-Signature"];
                if (string.IsNullOrEmpty(signature))
                {
                    return BadRequest("Stripe-Signature header is missing.");
                }

                // Verificar la firma utilizando el secreto del webhook
                stripeEvent = EventUtility.ConstructEvent(
                    json, 
                    signature, 
                    _stripeWebhookSecret,
                     throwOnApiVersionMismatch: false
                );
            }
            catch (StripeException e)
            {
                // Respuesta si la firma no es válida o si hubo un error en el procesamiento del evento
                return BadRequest($"Webhook Error: {e.Message}");
            }
            catch (Exception ex)
            {
                // Manejo de otros errores generales
                return StatusCode(500, $"Error: {ex.Message}");
            }

            // Verificar el tipo de evento recibido
            if (stripeEvent.Type == "checkout.session.completed")  // Usamos la cadena de texto directamente
            {
                var session = stripeEvent.Data.Object as Session;
                if (session != null)
                {
                    // Aquí puedes actualizar el estado de la orden en la base de datos
                    Guid orderId;
                    if (Guid.TryParse(session.ClientReferenceId, out orderId))
                    {
                        var paymentStatus = session.PaymentStatus;
                        var paymentIntentId = session.PaymentIntentId;

                        // Verificar si el pago fue exitoso
                        if (paymentStatus == "paid")
                        {
                            // Llamar a tu lógica de actualización de estado en la base de datos
                            var servicio = await this.servicioRepository.GetByIdAsync(orderId);
                            if (servicio != null)
                            {
                                servicio.ReferenciaPagoStripe = paymentIntentId;
                                servicio.EstatusServicioId = 2;
                                await this.servicioRepository.UpdateAsync(servicio);
                            }
                            else
                            {
                                // Si no se encuentra el servicio en la base de datos
                                return NotFound($"Service with ID {orderId} not found.");
                            }
                        }
                        else
                        {
                            // Si el pago no fue exitoso, puedes registrar un estado alternativo o enviar un mensaje de error
                            return BadRequest("Payment was not successful.");
                        }
                    }
                    else
                    {
                        // Si el ClientReferenceId no es un GUID válido
                        return BadRequest("Invalid ClientReferenceId.");
                    }
                }
                else
                {
                    // Si el objeto session no es válido
                    return BadRequest("Invalid session object.");
                }
            }
            else
            {
                // Si el tipo de evento no es 'checkout.session.completed', puedes retornar un error o simplemente omitirlo
                return BadRequest($"Unhandled event type: {stripeEvent.Type}");
            }

            return Ok();  // Responder a Stripe que hemos recibido el webhook correctamente
        }

    }
}
