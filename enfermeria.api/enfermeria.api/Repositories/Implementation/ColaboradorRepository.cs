using enfermeria.api.Data;
using enfermeria.api.Models;
using enfermeria.api.Models.Domain;
using enfermeria.api.Models.DTO.Colaborador;
using enfermeria.api.Models.DTO.Estado;
using enfermeria.api.Models.DTO.Usuarios;
using enfermeria.api.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace enfermeria.api.Repositories.Implementation
{
    public class ColaboradorRepository : IColaboradorRepository
    {
        private readonly DbAb1c8aEnfermeriaContext context;
        private readonly IConfiguration configuration;
        private readonly UserManager<IdentityUser> userManager;

        public ColaboradorRepository(DbAb1c8aEnfermeriaContext context, IConfiguration configuration, UserManager<IdentityUser> userManager)
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
                var results = await this.context.Colaboradors.Where(x => x.Id == id).ExecuteUpdateAsync(
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

        public async Task<ResponseModel> Create(CreateColaborador_Request model, string usuarioId)
        {
            ResponseModel rm = new ResponseModel();
            try
            {
                //validamos que no exista el telefono
                if (await this.context.Colaboradors.Where(x => x.Telefono == model.telefono).AnyAsync())
                {
                    rm.SetResponse(false, "El correo ya se encuentra registrado.");
                    return rm;
                }

                //validamos que no exista el correo
                if (await this.context.Colaboradors.Where(x=>x.CorreoElectronico == model.correoElectronico).AnyAsync())
                {
                    rm.SetResponse(false, "El correo ya se encuentra registrado.");
                    return rm;
                }

                //validamos que no exista el curp
                if (await this.context.Colaboradors.Where(x => x.Curp == model.curp).AnyAsync())
                {
                    rm.SetResponse(false, "El CURP ya se encuentra registrado.");
                    return rm;
                }

                //validamos que no exista el curp
                if (await this.context.Colaboradors.Where(x => x.Rfc == model.rfc).AnyAsync())
                {
                    rm.SetResponse(false, "El RFC ya se encuentra registrado.");
                    return rm;
                }
                Guid id = Guid.NewGuid();
                int no = await this.context.Colaboradors.CountAsync() + 1;
                //creamos el colaborador
                Colaborador colaborador = new Colaborador()
                {
                    Id = id,
                    No = no.ToString(),
                    Nombre = model.nombre.ToUpper(),
                    Apellidos = model.apellidos.ToUpper(),
                    Telefono = model.telefono.ToUpper(),
                    CorreoElectronico = model.telefono.ToLower(),
                    Rfc = model.rfc.ToUpper(),
                    Curp = model.curp.ToUpper(),
                    CedulaProfesional = model.cedulaProfesional.ToUpper(),
                    DomicilioCalle = model.domicilioCalle.ToUpper(),
                    DomicilioNumero = model.domicilioNumero.ToUpper(),
                    Cp = model.cp.ToUpper(),
                    Colonia = model.colonia.ToUpper(),
                    Banco = model.banco.ToUpper(),
                    Clabe = model.clabe.ToUpper(),
                    Cuenta = model.cuenta.ToUpper(),
                    EstatusColaboradorId = 1,//colocar enum
                    TipoEnfermeraId = model.tipoEnfermeraId,
                    Activo = false,
                    FechaCreacion = DateTime.Now,
                    UsuarioCreacionId = Guid.Parse(usuarioId),
                };

                List<RelEstadoColaborador> relEstadoColaboradors = new List<RelEstadoColaborador>();
                //creamos la relacion con los estados que da servicio
                foreach(var item in model.estados)
                {
                    relEstadoColaboradors.Add(new RelEstadoColaborador()
                    {
                        Id = Guid.NewGuid(),
                        ColaboradorId = id,
                        EstadoId = item
                    });

                }

                colaborador.RelEstadoColaboradors = relEstadoColaboradors;
                await this.context.Colaboradors.AddAsync(colaborador);
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
                var results = await this.context.Colaboradors.Where(x => x.Id == id).ExecuteUpdateAsync(
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
                List<GetColaborador_Response> result = await
                this.context.Colaboradors.Include(x=>x.TipoEnfermera).Select(x => new GetColaborador_Response()
                {
                    id = x.Id,
                    no = x.No,
                    nombre = x.Nombre,
                    apellidos = x.Apellidos,
                    telefono = x.Telefono,
                    correoElectronico = x.CorreoElectronico,
                    rfc = x.Rfc,
                    curp = x.Curp,
                    cedulaProfesional = x.CedulaProfesional,
                    domicilioCalle = x.DomicilioCalle,
                    domicilioNumero = x.DomicilioNumero,
                    cp = x.Cp,
                    colonia = x.Colonia,
                    banco = x.Banco,
                    clabe = x.Clabe,
                    cuenta = x.Cuenta,
                    estatusColaboradorId = x.EstatusColaboradorId,
                    estatusColaborador = x.EstatusColaborador.Descripcion,
                    tipoEnfermeraId = x.TipoEnfermeraId,
                    tipoEnfermera = x.TipoEnfermera.Descripcion,
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
                GetColaborador_Response? result = await
                this.context.Colaboradors.Include(x=>x.TipoEnfermera).Where(x => x.Id == id).Select(x => new GetColaborador_Response()
                {
                    id = x.Id,
                    no = x.No,
                    nombre = x.Nombre,
                    apellidos = x.Apellidos,
                    telefono = x.Telefono,
                    correoElectronico = x.CorreoElectronico,
                    rfc = x.Rfc,
                    curp = x.Curp,
                    cedulaProfesional = x.CedulaProfesional,
                    domicilioCalle = x.DomicilioCalle,
                    domicilioNumero = x.DomicilioNumero,
                    cp = x.Cp,
                    colonia = x.Colonia,
                    banco = x.Banco,
                    clabe = x.Clabe,
                    cuenta = x.Cuenta,
                    estatusColaboradorId = x.EstatusColaboradorId,
                    estatusColaborador = x.EstatusColaborador.Descripcion,
                    tipoEnfermeraId = x.TipoEnfermeraId,
                    tipoEnfermera = x.TipoEnfermera.Descripcion,
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

        public async Task<ResponseModel> GetColaboradores(GetColaboradores_Request model)
        {
            ResponseModel rm = new ResponseModel();
            Guid? tipo = null;
            if (model.tipo != "0") 
            { 
                tipo = Guid.Parse(model.tipo);
            }

            try
            {
                
                List<GetColaboradores_Response> result = await
                this.context.Colaboradors.Include(x=>x.EstatusColaborador).Include(x => x.TipoEnfermera)
                .Where(x =>
                (x.Nombre.Contains(model.nombre ?? "") || model.nombre == null) &&
                (x.Apellidos.Contains(model.nombre ?? "") || model.nombre == null) &&
                (x.CorreoElectronico.Contains(model.correoElectronico ?? "") || model.correoElectronico == null) &&
                (x.Telefono.Contains(model.telefono ?? "") || x.Telefono == null) &&
                (x.TipoEnfermeraId == tipo || tipo == null))
                .Select(x => new GetColaboradores_Response()
                {
                    id = x.Id,
                    no = x.No,
                    nombre = $"{x.Nombre.Trim()} {x.Apellidos.Trim()}",
                    telefono = x.Telefono,
                    correoElectronico = x.CorreoElectronico,
                    rfc = x.Rfc,
                    curp = x.Curp,
                    cedula = x.CedulaProfesional,
                    domicilio = $"{x.DomicilioCalle.Trim()} {x.DomicilioNumero.Trim()}, {x.Colonia.Trim()}",
                    estados = x.RelEstadoColaboradors.Select(x=>x.Estado.Nombre).ToList(),
                    estatus = x.EstatusColaborador.Descripcion,
                    activo = x.Activo ? 1 : 0,
                })
                .ToListAsync();

                rm.result = result;
                rm.SetResponse(true, "Datos guardados con éxito.");

            }
            catch (Exception ex)
            {
                rm.SetResponse(false, $"Ocurrio un error inesperado. [{ex.InnerException.Message}]");
            }
            return rm;
        }

        public Task<ResponseModel> Update(UpdateColaborador_Request model, Guid id, string usuarioId)
        {
            throw new NotImplementedException();
        }
    }
}
