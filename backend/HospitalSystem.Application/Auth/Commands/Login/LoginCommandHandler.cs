using HospitalSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Application.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginCommandHandler(IApplicationDbContext context, IJwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // 1. Buscar al usuario por correo e incluir su Rol (necesario para el Token)
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        // 2. Validar que exista y que la contraseña coincida 
        // (Nota: En un entorno real usaremos BCrypt para hashear, por ahora comparamos texto plano)
        if (user == null || user.PasswordHash != request.Password)
        {
            throw new UnauthorizedAccessException("Credenciales inválidas.");
        }

        // 3. Generar y devolver el Token
        return _jwtTokenGenerator.GenerateToken(user, user.Role);
    }
}