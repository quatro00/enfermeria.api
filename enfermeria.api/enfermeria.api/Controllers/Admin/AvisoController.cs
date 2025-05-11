using AutoMapper;
using enfermeria.api.Helpers;
using enfermeria.api.Models.Domain;
using enfermeria.api.Models.DTO.EncuestaPlantilla;
using enfermeria.api.Models;
using enfermeria.api.Repositories.Implementation;
using enfermeria.api.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace enfermeria.api.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class AvisoController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IAvisoRepository avisoRepository;

        public AvisoController(IAvisoRepository avisoRepository, IMapper mapper)
        {
            this.avisoRepository = avisoRepository;
            this.mapper = mapper;
        }

    }
}
