using enfermeria.api.Models;
using enfermeria.api.Models.DTO.Auth;

namespace enfermeria.api.Repositories.Interface
{
    public interface IAspNetUsersRepository
    {
        Task<ResponseModel> GetUserById(Guid id);
        Task<ResponseModel> ForgotPassword(ForgotPasswordRequestDto model, string token);
    }
}
