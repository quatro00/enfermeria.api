using Microsoft.AspNetCore.Identity;

namespace enfermeria.api.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
        string CreateRestoreToken(IdentityUser user);
    }
}
