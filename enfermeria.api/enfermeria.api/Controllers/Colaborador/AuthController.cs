using AutoMapper;
using enfermeria.api.Helpers;
using enfermeria.api.Models.Domain;
using enfermeria.api.Models.DTO;
using enfermeria.api.Models.DTO.Auth;
using enfermeria.api.Models.DTO.Usuarios;
using enfermeria.api.Repositories.Implementation;
using enfermeria.api.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace enfermeria.api.Controllers.Colaborador
{
    [Route("api/colaborador/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;
        private readonly IAspNetUsersRepository aspNetUsersRepository;
        private readonly IColaboradorRepository colaboradorRepository;
        private readonly IEmailService emailService;
        private readonly IMapper mapper;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository, IAspNetUsersRepository aspNetUsersRepository, IColaboradorRepository colaboradorRepository, IEmailService emailService, IMapper mapper)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.aspNetUsersRepository = aspNetUsersRepository;
            this.colaboradorRepository = colaboradorRepository;
            this.emailService = emailService;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            //checamos el email
            var identityUser = await userManager.FindByNameAsync(request.username);

            if (identityUser is not null)
            {
                var cuenta = await aspNetUsersRepository.GetUserById(Guid.Parse(identityUser.Id));
                UsuarioDto cuentaDto = cuenta.result;

                if (cuentaDto.activo == false)
                {
                    ModelState.AddModelError("error", "El usuario se encuentra bloqueado.");
                    return ValidationProblem(ModelState);
                }
                var checkPasswordResult = await userManager.CheckPasswordAsync(identityUser, request.password);

                if (checkPasswordResult)
                {
                    var roles = await userManager.GetRolesAsync(identityUser);
                    if (roles.IndexOf("Colaborador") == -1)
                    {
                        ModelState.AddModelError("error", "Email o password incorrecto.");
                        return ValidationProblem(ModelState);
                    }
                    var jwtToken = tokenRepository.CreateJwtToken(identityUser, roles.ToList());
                    var response = new LoginResponseDto()
                    {
                        Email = identityUser.Email,
                        Roles = roles.ToList(),
                        Token = jwtToken,
                        Nombre = "Nombre",
                        Apellidos = "Apellido",
                        Username = identityUser.UserName
                    };


                    return Ok(response);
                }

            }

            ModelState.AddModelError("error", "Email o password incorrecto.");
            return ValidationProblem(ModelState);
        }

        [HttpGet("ver-perfil")]
        [Authorize(Roles = "Colaborador")]
        public async Task<IActionResult> GetPerfil()
        {
            try
            {
                var userid = User.GetId();
                var colaborador = await this.colaboradorRepository.GetByUserIdAsync(userid);

                FiltroGlobal filtro = new FiltroGlobal()
                {
                   
                };

                var pacientes = await this.colaboradorRepository.GetByIdAsync(colaborador.Id, "Id", "Banco",
                        "RelEstadoColaboradors",
                        "RelEstadoColaboradors.Estado");
                
                var pacientesDto = mapper.Map<GetPerfilDto>(pacientes);

                return Ok(pacientesDto);
            }
            catch (Exception ex)
            {
                BadRequest("Ocurrio un error inesperado.");
            }
            return Ok();

        }

        [HttpPost("confirmar-reset")]
        public async Task<IActionResult> ConfirmarReset([FromBody] ResetPasswordDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("Usuario no encontrado");

            var decodedToken = System.Net.WebUtility.UrlDecode(model.Token);

            var result = await userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Contraseña actualizada correctamente");
        }

        [HttpPost("recuperar-contrasena")]
        public async Task<IActionResult> RecuperarContrasena([FromBody] RecuperarContrasenaDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Ok(); // no revelar si el email existe

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var encodedToken = System.Net.WebUtility.UrlEncode(token);

            // Aquí puedes enviar un correo real con un enlace al frontend
            // Ejemplo: https://tusitio.com/reset?email=...&token=...

            string resetLink = $"https://tusitio.com/reset?email={user.Email}&token={encodedToken}";
            await emailService.SendEmailAsync_RecuperarPassword(resetLink);
            // En producción: envía el correo. En dev: puedes devolver el link
            return Ok(new { resetLink });
        }
    }
}
