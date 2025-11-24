using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // <-- Nueva línea


namespace Dsw2025Tpi.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JwtTokenServices _jwtTokenServices;

        public AuthenticationService(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            JwtTokenServices jwtTokenServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenServices = jwtTokenServices;
        }

        public async Task<string> Login(LoginModel request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Username and password are required.");

            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
                throw new InvalidCredentialException("Invalid username or password.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
                throw new InvalidCredentialsException("Invalid username or password.");

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "User";

            return _jwtTokenServices.GenerateToken(request.Username, role);
        }

        public async Task<string> Register(RegisterModel model)
        {
            if (model == null) throw new ArgumentException("Register model cannot be null.");

            if (string.IsNullOrWhiteSpace(model.Username) ||
                string.IsNullOrWhiteSpace(model.Password) ||
                string.IsNullOrWhiteSpace(model.Email))
                throw new ArgumentException("All fields are required.");

            if (!model.Email.Contains("@"))
                throw new ArgumentException("Invalid email format.");

            var user = new IdentityUser
            {
                UserName = model.Username,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Dsw2025Tpi.Application.Exceptions.ApplicationException($"Registration failed: {errors}"); // Usa tu propia ApplicationException
            }

            await _userManager.AddToRoleAsync(user, "User");

            return "User registered successfully.";
        }
    }
}
