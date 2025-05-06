using enfermeria.api.Models.DTO.Auth;
using enfermeria.api.Models.DTO.Usuarios;
using enfermeria.api.Repositories.Interface;
using Microsoft.AspNetCore.Http;
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

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository, IAspNetUsersRepository aspNetUsersRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.aspNetUsersRepository = aspNetUsersRepository;
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
    }
}
