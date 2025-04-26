namespace enfermeria.api.Mappings
{
    using AutoMapper;
    using enfermeria.api.Enums;
    using enfermeria.api.Helpers.Cotizacion;
    using enfermeria.api.Models.Domain;
    using enfermeria.api.Models.DTO.Banco;
    using enfermeria.api.Models.DTO.Colaborador;
    using enfermeria.api.Models.DTO.Contacto;
    using enfermeria.api.Models.DTO.EncuestaPlantilla;
    using enfermeria.api.Models.DTO.EncuestaPlantillaPregunta;
    using enfermeria.api.Models.DTO.Paciente;
    using enfermeria.api.Models.DTO.Pago;
    using enfermeria.api.Models.DTO.Servicio;
    using enfermeria.api.Models.DTO.ServicioFecha;
    using enfermeria.api.Models.DTO.ServicioFechaOferta;
    using enfermeria.api.Models.PagoLote;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Pago, GetDepositosDto>()
               .ForMember(dest => dest.PagoLoteId, opt => opt.MapFrom(src => src.PagoLoteId))
               .ForMember(dest => dest.ColaboradorId, opt => opt.MapFrom(src => src.ServicioFecha.ColaboradorAsignadoId))
               .ForMember(dest => dest.Banco, opt => opt.MapFrom(src => src.ServicioFecha.ColaboradorAsignado.Banco.Nombre))
               .ForMember(dest => dest.Clabe, opt => opt.MapFrom(src => src.ServicioFecha.ColaboradorAsignado.Clabe))
               .ForMember(dest => dest.Beneficiario, opt => opt.MapFrom(src => src.ServicioFecha.ColaboradorAsignado.Nombre + " " + src.ServicioFecha.ColaboradorAsignado.Apellidos))
               .ForMember(dest => dest.Monto, opt => opt.MapFrom(src => src.Total))
               .ForMember(dest => dest.Referencia, opt => opt.MapFrom(src => src.Referencia))
               .ForMember(dest => dest.Pagado, opt => opt.MapFrom(src => src.EstatusPagoId))
             ;

            CreateMap<Pago, GetPagosDto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.No, opt => opt.MapFrom(src => src.No))
               .ForMember(dest => dest.Referencia, opt => opt.MapFrom(src => src.Referencia))
               .ForMember(dest => dest.Beneficiario, opt => opt.MapFrom(src => src.ServicioFecha.ColaboradorAsignado.Nombre + " " + src.ServicioFecha.ColaboradorAsignado.Apellidos))
               .ForMember(dest => dest.Monto, opt => opt.MapFrom(src => src.Total))
               .ForMember(dest => dest.FechaPago, opt => opt.MapFrom(src => src.FechaPago))
               .ForMember(dest => dest.EstatusPago, opt => opt.MapFrom(src => src.EstatusPago.Descripcion))
             ;

            CreateMap<PagoLote, GetPagoLoteResponse>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.No, opt => opt.MapFrom(src => src.No))
               .ForMember(dest => dest.TotalLote, opt => opt.MapFrom(src => src.Pagos.Sum(x => x.Total) ))
               .ForMember(dest => dest.NumeroPagos, opt => opt.MapFrom(src => src.Pagos.Count()))
               .ForMember(dest => dest.Colaboradores, opt => opt.MapFrom(src => src.Pagos.Select(p=>p.ServicioFecha.ColaboradorAsignadoId).Distinct().Count()))
               .ForMember(dest => dest.EstatusPagoLote, opt => opt.MapFrom(src => src.EstatosPagoLote.Descripcion))
             ;

            CreateMap<CrearPagoLoteDto, PagoLote>()
                .ForMember(dest => dest.Etiqueta, opt => opt.MapFrom(src => src.Concepto))
                .ForMember(dest => dest.FechaInicio, opt => opt.MapFrom(src => src.FechaInicio))
                .ForMember(dest => dest.FechaFin, opt => opt.MapFrom(src => src.FechaFin))
                .ForMember(dest => dest.Csv, opt => opt.MapFrom(src => ""))
                .ForMember(dest => dest.EstatosPagoLoteId, opt => opt.MapFrom(src => (int)EstatusPagoLoteEnum.PorPagar))
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Pagos, opt => opt.MapFrom(src => new List<Pago>()))
              ;

            CreateMap<ServicioFecha, GetServicioFechaFiltrosResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Colaborador, opt => opt.MapFrom(src => src.ColaboradorAsignado.Nombre + " " + src.ColaboradorAsignado.Apellidos))
                .ForMember(dest => dest.Inicio, opt => opt.MapFrom(src => src.FechaInicio))
                .ForMember(dest => dest.Termino, opt => opt.MapFrom(src => src.FechaTermino))
                .ForMember(dest => dest.Horas, opt => opt.MapFrom(src => src.CantidadHoras))
                .ForMember(dest => dest.ImporteBruto, opt => opt.MapFrom(src => src.ImporteBruto))
                .ForMember(dest => dest.ImporteSolicitado, opt => opt.MapFrom(src => src.ImporteSolicitado))
                .ForMember(dest => dest.Comision, opt => opt.MapFrom(src => src.Comision))
                .ForMember(dest => dest.CostosOperativos, opt => opt.MapFrom(src => src.CostosOperativos))
                .ForMember(dest => dest.Retenciones, opt => opt.MapFrom(src => src.Retenciones))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total))
                .ForMember(dest => dest.EstatusServicioFecha, opt => opt.MapFrom(src => src.EstatusServicioFecha.Descripcion))
              ;

            CreateMap<CrearEncuestaPlantillaPreguntaDto, EncuestaPlantillaPreguntum>()
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.Activo))
              ;

            CreateMap<EncuestaPlantillaPreguntum, GetEncuestaPlantillaPreguntaDto>()
              ;

            CreateMap<EncuestaPlantilla, GetEncuestasPlantillaDto>()
               .ForMember(dest => dest.No, opt => opt.MapFrom(src => src.No))
               .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
               .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
               .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.Activo))
               ;

            CreateMap<CrearEncuestaPlantillaDto, EncuestaPlantilla>()
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.TipoServicio, opt => opt.MapFrom(src => ""))
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => DateTime.Now))
                ;

            CreateMap<ServicioFecha, GetGuardiasDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NoServicio, opt => opt.MapFrom(src => src.Servicio.No))
                .ForMember(dest => dest.Colaborador, opt => opt.MapFrom(src =>
                    src.ColaboradorAsignado != null
                        ? src.ColaboradorAsignado.Nombre + " " + src.ColaboradorAsignado.Apellidos
                        : string.Empty))
                .ForMember(dest => dest.FechaInicio, opt => opt.MapFrom(src => src.FechaInicio))
                .ForMember(dest => dest.FechaFin, opt => opt.MapFrom(src => src.FechaTermino))
                .ForMember(dest => dest.EstatusServicioFecha, opt => opt.MapFrom(src => src.EstatusServicioFecha.Descripcion))
                .ForMember(dest => dest.CantidadHoras, opt => opt.MapFrom(src => src.CantidadHoras))
                ;

            CreateMap<ServicioFechasOfertum, GetServicioFechaOfertaDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Colaborador, opt => opt.MapFrom(src => $"{src.Colaborador.Nombre} {src.Colaborador.Apellidos}"))
                .ForMember(dest => dest.FechaInicio, opt => opt.MapFrom(src => src.ServicioFecha.FechaInicio))
                .ForMember(dest => dest.FechaTermino, opt => opt.MapFrom(src => src.ServicioFecha.FechaTermino))
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Colaborador.Telefono))
                .ForMember(dest => dest.Correo, opt => opt.MapFrom(src => src.Colaborador.CorreoElectronico))
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Colaborador.TipoEnfermera.Descripcion))
                .ForMember(dest => dest.Comentario, opt => opt.MapFrom(src => src.Comentario))
                .ForMember(dest => dest.Monto, opt => opt.MapFrom(src => src.MontoSolicitado))
                .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => src.FechaCreacion))
                ;

            CreateMap<ServicioFecha, ServicioFechaDto>()
                .ForMember(dest => dest.EstatusServicioFecha, opt => opt.MapFrom(src => src.EstatusServicioFecha.Descripcion))
                .ForMember(dest => dest.Ofertas, opt => opt.MapFrom(src => src.ServicioFechasOferta.Where(x=>x.Activo).Count()))
                .ForMember(dest => dest.No, opt => opt.MapFrom(src => src.Servicio.No))
                ;

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
               .ForMember(dest => dest.Horas, opt => opt.MapFrom(src => src.ServicioFechas.Sum(x=>x.CantidadHoras)))
               .ForMember(dest => dest.GuardiasAsignadas, opt => opt.MapFrom(src => src.ServicioFechas.Where(x => x.ColaboradorAsignadoId != null).Count()))
               .ForMember(dest => dest.GuardiasPorAsignar, opt => opt.MapFrom(src => src.ServicioFechas.Where(x => x.ColaboradorAsignadoId == null).Count()))
               ;

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
