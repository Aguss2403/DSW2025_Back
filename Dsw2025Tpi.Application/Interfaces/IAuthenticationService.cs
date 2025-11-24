using Dsw2025Tpi.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Interfaces;

public interface IAuthenticationService
{
    Task<string> Login(LoginModel request);
    Task<string> Register(RegisterModel request);
}
