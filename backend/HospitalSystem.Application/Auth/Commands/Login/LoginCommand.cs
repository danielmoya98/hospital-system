using MediatR;

namespace HospitalSystem.Application.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<string>;