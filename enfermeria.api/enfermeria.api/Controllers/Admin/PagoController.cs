using AutoMapper;
using enfermeria.api.Data;
using enfermeria.api.Models.DTO;
using enfermeria.api.Models.PagoLote;
using enfermeria.api.Models.Specifications;
using enfermeria.api.Models;
using enfermeria.api.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using enfermeria.api.Models.DTO.Pago;

namespace enfermeria.api.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class PagoController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IPagoRepository pagoRepository;
        private readonly DbContext _context;

        public PagoController(IPagoRepository pagoRepository, IMapper mapper, DbAb1c8aEnfermeriaContext context)
        {
            this.pagoRepository = pagoRepository;
            _context = context;
            this.mapper = mapper;
        }

        [HttpGet("ver-depositos")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetDepositos([FromQuery] GetPagoRequest model)
        {

            FiltroGlobal filtro = new FiltroGlobal()
            {
                PagoLoteId = model.PagoLoteId
            };


            //creamos la respuesta
            var response = new ResponseModel_2<List<GetDepositosDto>>();
            filtro.IncluirInactivos = false;

            try
            {
                //colocamos los filtros
                var spec = new PagoSpecification(filtro);

                //colocamos los includes
                spec.IncludeStrings = new List<string> { "ServicioFecha", "ServicioFecha.ColaboradorAsignado", "ServicioFecha.ColaboradorAsignado.Banco", "EstatusPago" };

                //convertimos de la clase al dto
                var result = await pagoRepository.ListAsync(spec);
                var resultDto = mapper.Map<List<GetDepositosDto>>(result);

                //seteamos el resultado
                response.SetResponse(true, "");
                response.Result = resultDto;

                var agrupados = resultDto
                .GroupBy(x => new
                {
                    x.PagoLoteId,
                    x.ColaboradorId,
                    x.Banco,
                    x.Clabe,
                    x.Beneficiario,
                    x.Referencia,
                    x.Pagado
                })
                .Select(g => new GetDepositosDto
                {
                    PagoLoteId = g.Key.PagoLoteId,
                    ColaboradorId = g.Key.ColaboradorId,
                    Banco = g.Key.Banco,
                    Clabe = g.Key.Clabe,
                    Beneficiario = g.Key.Beneficiario,
                    Monto = g.Sum(p => p.Monto),
                    Referencia = g.Key.Referencia,
                    Pagado = g.Key.Pagado
                })
                .ToList();


                return Ok(agrupados);
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
        public async Task<IActionResult> GetPagosLote([FromQuery] GetPagoRequest model)
        {

            FiltroGlobal filtro = new FiltroGlobal()
            {
                PagoLoteId = model.PagoLoteId
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
                var result = await pagoRepository.ListAsync(spec);
                var resultDto = mapper.Map<List<GetPagosDto>>(result);

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
    }
}
