using enfermeria.api.Helpers;
using enfermeria.api.Models.DTO.Auth;
using enfermeria.api.Models.DTO.Usuarios;
using enfermeria.api.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace enfermeria.api.Controllers
{
    [Route("api/[controller]")]
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
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto request)
        {
            var identityUser = await userManager.FindByEmailAsync(request.Email);

            if (identityUser is not null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(identityUser);
                var response = await aspNetUsersRepository.ForgotPassword(request, token);

                if (!response.response)
                {
                    ModelState.AddModelError("error", response.message);
                    return ValidationProblem(ModelState);
                }

                return Ok(response.result);

            }

            ModelState.AddModelError("error", "Usuario no encontrado.");
            return ValidationProblem(ModelState);
        }

        [HttpPatch]
        [Route("restore-password")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> RestorePassword([FromBody] RestorePasswordRequestDto request)
        {
            var identityUser = await userManager.FindByIdAsync(User.GetId());
            var response = await userManager.ResetPasswordAsync(identityUser, request.token, request.newPassword);

            if (response.Errors.Count() > 0)
            {
                ModelState.AddModelError("error", "");
                return ValidationProblem(ModelState);
            }

            return Ok(response.Succeeded);
        }


        [HttpPost]
        [Route("change-password")]


        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto request)
        {
            var identityUser = await userManager.FindByNameAsync(request.username);
            var changuePassword = await userManager.ChangePasswordAsync(identityUser!, request.currentPassword, request.newPassword);

            return Ok(changuePassword);
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
