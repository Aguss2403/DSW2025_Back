using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions; // Asegúrate de que esta carpeta exista o usa System.Exception
using Dsw2025Tpi.Application.Interfaces;
using Dsw2025Tpi.Data;
using Dsw2025Tpi.Domain.Entities; // Tus entidades personalizadas
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;

namespace Dsw2025Tpi.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly Dsw2025TpiContext _context;
    private readonly JwtTokenServices _jwtTokenServices;

    public AuthenticationService(Dsw2025TpiContext context, JwtTokenServices jwtTokenServices)
    {
        _context = context;
        _jwtTokenServices = jwtTokenServices;
    }

    public async Task<string> Login(LoginModel request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("El usuario y la contraseña son obligatorios.");

        // 1. Buscamos el usuario en TU base de datos, incluyendo su Rol
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        // 2. Verificamos si existe y si la contraseña coincide
        // NOTA: En producción, aquí deberías comparar hashes (ej: BCrypt.Verify), no texto plano.
        if (user == null || user.Password != request.Password)
            throw new InvalidCredentialException("Usuario o contraseña inválidos.");

        // 3. Generamos el token usando el nombre del Rol de tu entidad
        return _jwtTokenServices.GenerateToken(user.Username, user.Role.Name);
    }

    public async Task<string> Register(RegisterModel model)
    {
        // 1. Validaciones básicas
        if (model == null) throw new ArgumentException("Los datos de registro son obligatorios.");

        // Puedes agregar más validaciones aquí o usar DataAnnotations en el DTO
        if (!model.Email.Contains("@")) throw new ArgumentException("Formato de email inválido.");

        // 2. Verificar si el usuario ya existe (por Email o Username)
        bool userExists = await _context.Users.AnyAsync(u => u.Username == model.Username || u.Email == model.Email);
        if (userExists)
        {
            throw new InvalidOperationException("El nombre de usuario o email ya está en uso.");
        }

        // 3. Obtener el Rol "User" de la base de datos
        // (Asegúrate de haber corrido el Seeder que hicimos antes)
        var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
        if (userRole == null)
        {
            throw new InvalidOperationException("Error interno: El rol 'User' no está configurado.");
        }

        // 4. Crear la Entidad USER (Cuenta)
        // NOTA: Aquí deberías hashear la contraseña antes de pasarla al constructor.
        var newUser = new User(model.Username, model.Email, model.Password, userRole);

        // 5. Crear la Entidad CUSTOMER (Perfil) vinculada al usuario
        var newCustomer = new Customer(newUser, model.FirstName, model.LastName, model.Address, model.PhoneNumber);

        // 6. Guardar en la base de datos
        // Al agregar Customer, EF Core entiende que debe guardar también el User vinculado
        _context.Customers.Add(newCustomer);
        await _context.SaveChangesAsync();

        return "Usuario y Cliente registrados exitosamente.";
    }
}