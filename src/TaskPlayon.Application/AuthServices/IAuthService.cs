

using TaskPlayon.Domain.Model.Auth;

namespace TaskPlayon.Application.AuthServices;

public interface IAuthService
{
    Task<LoginResponse> Login(LoginModel model);
    Task<RegistrationResponse> Register(RegistrationModel model);
}
