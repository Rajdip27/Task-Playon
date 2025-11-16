using TaskPlayon.Domain.Model.Auth;

namespace TaskPlayon.Application.AuthServices;

public class AuthService : IAuthService
{
    public Task<LoginResponse> Login(LoginModel model)
    {
        throw new NotImplementedException();
    }

    public Task<RegistrationResponse> Register(RegistrationModel model)
    {
        throw new NotImplementedException();
    }
}
