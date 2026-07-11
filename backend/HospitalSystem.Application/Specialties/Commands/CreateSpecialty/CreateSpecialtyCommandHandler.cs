using HospitalSystem.Application.Common.Interfaces;
using HospitalSystem.Domain.Entities;
using MediatR;

namespace HospitalSystem.Application.Specialties.Commands.CreateSpecialty;

public class CreateSpecialtyCommandHandler : IRequestHandler<CreateSpecialtyCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateSpecialtyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateSpecialtyCommand request, CancellationToken cancellationToken)
    {
        var specialty = new Specialty
        {
            Name = request.Name,
            Description = request.Description
        };

        _context.Specialties.Add(specialty);
        await _context.SaveChangesAsync(cancellationToken);

        return specialty.Id;
    }
}