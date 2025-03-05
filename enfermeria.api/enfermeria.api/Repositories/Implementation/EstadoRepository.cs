using enfermeria.api.Data;
using enfermeria.api.Models;
using enfermeria.api.Models.Domain;
using enfermeria.api.Models.DTO.Estado;
using enfermeria.api.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace enfermeria.api.Repositories.Implementation
{
    public class EstadoRepository : IEstadoRepository
    {
        private readonly DbAb1c8aEnfermeriaContext context;
        private readonly IConfiguration configuration;
        private readonly UserManager<IdentityUser> userManager;

        public EstadoRepository(DbAb1c8aEnfermeriaContext context, IConfiguration configuration, UserManager<IdentityUser> userManager)
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
                var results = await this.context.CatEstados.Where(x => x.Id == id).ExecuteUpdateAsync(
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

        public async Task<ResponseModel> Create(CreateEstado_Request model, string usuarioId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                CatEstado estado = new CatEstado()
                {
                    Id = Guid.NewGuid(),
                    Nombre = model.nombre.ToUpper(),
                    NombreCorto = model.nombreCorto.ToUpper(),
                    Activo = false,
                };

                await this.context.CatEstados.AddAsync(estado);
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
                var results = await this.context.CatEstados.Where(x => x.Id == id).ExecuteUpdateAsync(
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
                List<GetEstado_Response> result = await
                this.context.CatEstados.Select(x => new GetEstado_Response()
                {
                    id = x.Id,
                    nombre = x.Nombre,
                    nombreCorto = x.NombreCorto,
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
                GetEstado_Response? result = await
                this.context.CatEstados.Where(x => x.Id == id).Select(x => new GetEstado_Response()
                {
                    id = x.Id,
                    nombre = x.Nombre,
                    nombreCorto = x.NombreCorto,
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

        public async Task<ResponseModel> Update(UpdateEstado_Request model, Guid id, string usuarioId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                var results = await this.context.CatEstados.Where(x => x.Id == id).ExecuteUpdateAsync(
                   s => s
                    .SetProperty(t => t.Nombre, t => model.nombre)
                    .SetProperty(t => t.NombreCorto, t => model.nombreCorto)
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
