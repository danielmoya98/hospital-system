using HospitalSystem.Application.Doctors.Queries.DTOs;
using MediatR;

namespace HospitalSystem.Application.Doctors.Queries.GetDoctorSchedule;

public record GetDoctorScheduleQuery(Guid DoctorId) : IRequest<List<DoctorScheduleDto>>;