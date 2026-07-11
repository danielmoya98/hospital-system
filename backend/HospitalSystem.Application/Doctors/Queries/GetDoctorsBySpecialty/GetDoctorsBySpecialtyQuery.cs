using HospitalSystem.Application.Doctors.Queries.DTOs;
using MediatR;

namespace HospitalSystem.Application.Doctors.Queries.GetDoctorsBySpecialty;

// Recibe el ID de la especialidad y devuelve una lista de DoctorDto
public record GetDoctorsBySpecialtyQuery(Guid SpecialtyId) : IRequest<List<DoctorDto>>;