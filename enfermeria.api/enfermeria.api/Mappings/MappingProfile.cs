namespace enfermeria.api.Mappings
{
    using AutoMapper;
    using enfermeria.api.Models.Domain;
    using enfermeria.api.Models.DTO;
    using enfermeria.api.Models.DTO.Paciente;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //convertir de paciente a pacientedto
            CreateMap<Paciente, GetPacienteDto>();
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
