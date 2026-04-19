using ExaminationSystem.Application.Feature.Student.Dashboard.DTOs;
using ExaminationSystem.Domin.Comman.Result;
using MediatR;

namespace ExaminationSystem.Application.Feature.Student.Dashboard.Queries
{
    public record GetStudentDashboardQuery(Guid StudentId)
     : IRequest<Result<StudentDashboardDto>>;
}
