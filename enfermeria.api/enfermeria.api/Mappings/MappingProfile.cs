namespace enfermeria.api.Mappings
{
    using AutoMapper;
    using enfermeria.api.Enums;
    using enfermeria.api.Helpers.Cotizacion;
    using enfermeria.api.Models.Domain;
    using enfermeria.api.Models.DTO;
    using enfermeria.api.Models.DTO.Banco;
    using enfermeria.api.Models.DTO.Colaborador;
    using enfermeria.api.Models.DTO.Contacto;
    using enfermeria.api.Models.DTO.Paciente;
    using enfermeria.api.Models.DTO.Servicio;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //mapeo creacion de servicio
            CreateMap<Servicio, GetCotizacionResult>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.No, opt => opt.MapFrom(src => src.No))
               .ForMember(dest => dest.Paciente, opt => opt.MapFrom(src => src.Paciente.Nombre + " " + src.Paciente.Apellidos))
               .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado.Nombre))
               .ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src.Direccion))
               .ForMember(dest => dest.Motivo, opt => opt.MapFrom(src => src.PrincipalRazon))
               .ForMember(dest => dest.TipoEnfermera, opt => opt.MapFrom(src => src.TipoEnfermera.Descripcion))
               .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.ServicioFechas.SelectMany(f=>f.ServicioCotizacions).Sum(x=>x.PrecioFinal)))
               .ForMember(dest => dest.Descuento, opt => opt.MapFrom(src => src.Descuento))
               .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.ServicioFechas.SelectMany(f => f.ServicioCotizacions).Sum(x => x.PrecioFinal) - src.Descuento))
               .ForMember(dest => dest.Estatus, opt => opt.MapFrom(src => src.EstatusServicio.Descripcion))
               .ForMember(dest => dest.Horas, opt => opt.MapFrom(src => src.ServicioFechas.Sum(x=>x.CantidadHoras)));

            CreateMap<CrearServicioDto, Servicio>()
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.EstatusServicioId, opt => opt.MapFrom(src => 1))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.DispositivosMedicos, opt => opt.MapFrom(src => src.cuentaConDispositivosMedicos))
                .ForMember(dest => dest.DispositivosMedicosDesc, opt => opt.MapFrom(src => src.cuentaConDispositivosMedicosDesc))
                .ForMember(dest => dest.RequiereCuidadosCriticos, opt => opt.MapFrom(src => src.cuidadosCriticos))
                .ForMember(dest => dest.RequiereCuidadosCriticosDesc, opt => opt.MapFrom(src => src.cuidadosCriticosDesc))
                .ForMember(dest => dest.TomaMedicamento, opt => opt.MapFrom(src => src.tomaAlgunMedicamento))
                .ForMember(dest => dest.TomaMedicamentoDesc, opt => opt.MapFrom(src => src.tomaAlgunMedicamentoDesc))
                .ForMember(dest => dest.PrincipalRazon, opt => opt.MapFrom(src => src.motivo));
            CreateMap<CrearServicioFechasFormatoDto, ServicioFecha>()
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => DateTime.Now));
            //mapeo colaborador
            CreateMap<Colaborador, ColaboradorDto>()
                .ForMember(dest => dest.Estatus, opt => opt.MapFrom(src => src.EstatusColaborador.Descripcion))
                .ForMember(dest => dest.Estados, opt => opt.MapFrom(src => src.RelEstadoColaboradors.Select(x=>x.Estado.Nombre).ToList().Order()));
            CreateMap<CrearColaboradorDto, Colaborador>()
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.EstatusColaboradorId, opt => opt.MapFrom(src => (int)EstatusColaboradorEnum.PreRegistro))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.CuentaCreada, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => ""))
                ;
            //mapeo banco
            CreateMap<CatBanco, BancoDto>();
            //mapeo contacto
            CreateMap<UpdateContactoDto, Contacto>();
            CreateMap<Contacto, GetContactoDto>();
            //convertir de paciente a pacientedto
            CreateMap<Paciente, GetPacienteDto>();
            CreateMap<UpdatePacienteDto, Paciente>()
                 .ForMember(dest => dest.FechaModificacion, opt => opt.MapFrom(src => DateTime.Now));


            // Mapeo de PacienteCreateDto a Paciente
            CreateMap<CrearPacienteDto, Paciente>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // El ID lo genera la BD
                .ForMember(dest => dest.No, opt => opt.Ignore()) // El ID lo genera la BD
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Apellidos, opt => opt.MapFrom(src => src.Apellidos))
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono))
                .ForMember(dest => dest.CorreoElectronico, opt => opt.MapFrom(src => src.CorreoElectronico))
                .ForMember(dest => dest.FechaNacimiento, opt => opt.MapFrom(src => src.FechaNacimiento))
                .ForMember(dest => dest.Genero, opt => opt.MapFrom(src => src.Genero))
                .ForMember(dest => dest.Peso, opt => opt.MapFrom(src => src.Peso))
                .ForMember(dest => dest.Estatura, opt => opt.MapFrom(src => src.Estatura))
                .ForMember(dest => dest.Discapacidad, opt => opt.MapFrom(src => src.Discapacidad))
                .ForMember(dest => dest.DescripcionDiscapacidad, opt => opt.MapFrom(src => src.DescripcionDiscapacidad))
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UsuarioCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.Contactos, opt => opt.MapFrom(src => src.Contactos)); // Mapea la lista de contactos

            // Mapeo de ContactDto a Contacto
            CreateMap<CrearPacienteContactoDto, Contacto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // El ID lo genera la BD
                .ForMember(dest => dest.PacienteId, opt => opt.Ignore())
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono))
                .ForMember(dest => dest.CorreoElectronico, opt => opt.MapFrom(src => src.CorreoElectronico))
                .ForMember(dest => dest.Parentezco, opt => opt.MapFrom(src => src.Parentezco))
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UsuarioCreacionId, opt => opt.Ignore())
                .ForMember(dest => dest.FechaModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioModificacionId, opt => opt.Ignore())
                ; // Asegúrate de que el PacienteId se maneje de manera adecuada
        }
    }
}
