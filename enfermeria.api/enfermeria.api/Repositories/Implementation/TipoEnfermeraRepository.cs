using enfermeria.api.Data;
using enfermeria.api.Models;
using enfermeria.api.Models.Domain;
using enfermeria.api.Models.DTO.Estado;
using enfermeria.api.Models.DTO.TipoEnfermera;
using enfermeria.api.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace enfermeria.api.Repositories.Implementation
{
    public class TipoEnfermeraRepository : ITipoEnfermeraRepository
    {
        private readonly DbAb1c8aEnfermeriaContext context;
        private readonly IConfiguration configuration;
        private readonly UserManager<IdentityUser> userManager;

        public TipoEnfermeraRepository(DbAb1c8aEnfermeriaContext context, IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.configuration = configuration;
            this.userManager = userManager;
        }

        public async Task<ResponseModel> Activar(Guid id)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                var results = await this.context.CatTipoEnfermeras.Where(x => x.TipoEnfermeraId == id).ExecuteUpdateAsync(
                   s => s
                    .SetProperty(t => t.Activo, t => true)
                );

                await context.SaveChangesAsync();

                rm.result = results;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, $"Ocurrio un error inesperado. [{ex.InnerException.Message}]");
            }
            return rm;
        }

        public async Task<ResponseModel> Create(CreateTipoEnfermera_Request model, string usuarioId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                CatTipoEnfermera tipoEnfermera = new CatTipoEnfermera()
                {
                    TipoEnfermeraId = Guid.NewGuid(),
                    No = model.no,
                    Descripcion = model.descripcion.ToUpper(),
                    Valor = model.valor,
                    CostoHora = model.costoHora,
                    Activo = false,
                };

                await this.context.CatTipoEnfermeras.AddAsync(tipoEnfermera);
                await this.context.SaveChangesAsync();

                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, $"Ocurrio un error inesperado. [{ex.InnerException.Message}]");
            }
            return rm;
        }

        public async Task<ResponseModel> Desactivar(Guid id)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                var results = await this.context.CatTipoEnfermeras.Where(x => x.TipoEnfermeraId == id).ExecuteUpdateAsync(
                   s => s
                    .SetProperty(t => t.Activo, t => false)
                );

                await context.SaveChangesAsync();

                rm.result = results;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, $"Ocurrio un error inesperado. [{ex.InnerException.Message}]");
            }
            return rm;
        }

        public async Task<ResponseModel> Get()
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                List<GetTipoEnfermera_Response> result = await
                this.context.CatTipoEnfermeras.Select(x => new GetTipoEnfermera_Response()
                {
                    id = x.TipoEnfermeraId,
                    no = x.No,
                    descripcion = x.Descripcion,
                    valor = x.Valor,
                    costoHora = x.CostoHora,
                    activo = x.Activo ? 1 : 0,
                }).ToListAsync();

                rm.result = result;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, $"Ocurrio un error inesperado. [{ex.InnerException.Message}]");
            }
            return rm;
        }

        public async Task<ResponseModel> Get(Guid id)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                GetTipoEnfermera_Response? result = await
                this.context.CatTipoEnfermeras.Where(x => x.TipoEnfermeraId == id).Select(x => new GetTipoEnfermera_Response()
                {
                    id = x.TipoEnfermeraId,
                    no = x.No,
                    descripcion = x.Descripcion,
                    valor = x.Valor,
                    costoHora = x.CostoHora,
                    activo = x.Activo ? 1 : 0,
                }).FirstOrDefaultAsync();

                rm.result = result;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, $"Ocurrio un error inesperado. [{ex.InnerException.Message}]");
            }
            return rm;
        }

        public async Task<ResponseModel> GetActivos()
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                List<GetTipoEnfermera_Response> result = await
                this.context.CatTipoEnfermeras.Where(x=>x.Activo == true).Select(x => new GetTipoEnfermera_Response()
                {
                    id = x.TipoEnfermeraId,
                    no = x.No,
                    descripcion = x.Descripcion,
                    valor = x.Valor,
                    costoHora = x.CostoHora,
                    activo = x.Activo ? 1 : 0,
                }).ToListAsync();

                rm.result = result;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, $"Ocurrio un error inesperado. [{ex.InnerException.Message}]");
            }
            return rm;
        }

        public async Task<ResponseModel> Update(UpdateTipoEnfermera_Request model, Guid id, string usuarioId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                var results = await this.context.CatTipoEnfermeras.Where(x => x.TipoEnfermeraId == id).ExecuteUpdateAsync(
                   s => s
                    .SetProperty(t => t.No, t => model.no)
                    .SetProperty(t => t.Descripcion, t => model.descripcion)
                    .SetProperty(t => t.Valor, t => model.valor)
                    .SetProperty(t => t.CostoHora, t => model.costoHora)
                );

                await context.SaveChangesAsync();

                rm.result = results;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, $"Ocurrio un error inesperado. [{ex.InnerException.Message}]");
            }
            return rm;
        }
    }
}
