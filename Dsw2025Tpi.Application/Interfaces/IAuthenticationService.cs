using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Domain.Entities;

namespace Dsw2025Tpi.Application.Interfaces;

public interface IAuthenticationService
{
    Task<ResponseLoginModel> Login(RequestLoginModel request);
    Task<string> Register(RegisterModel model);
}