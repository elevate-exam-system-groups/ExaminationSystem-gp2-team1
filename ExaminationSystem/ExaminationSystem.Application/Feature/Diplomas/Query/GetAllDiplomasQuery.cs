using ExaminationSystem.Application.ViewModels.Diplomas;
using ExaminationSystem.Domin.Common.Result;
using MediatR;

namespace ExaminationSystem.Application.Feature.Diplomas.Query
{
    public sealed record GetAllDiplomasQuery : IRequest<RequestResult<IEnumerable<DiplomaViewModel>>>;
}
