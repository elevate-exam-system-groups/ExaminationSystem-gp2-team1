using ExaminationSystem.Application.Pagination;
using ExaminationSystem.Application.ViewModels.Quizzes;
using ExaminationSystem.Domin.Common.Result;
using MediatR;

namespace ExaminationSystem.Application.Feature.Quizzes.Query
{
    public sealed record GetStudentAttemptsQuery( Guid? QuizId ,Guid? DiplomaId,int PageNumber = 1,int PageSize = 10) : IRequest<RequestResult<PaginatedList<StudentAttemptHistoryViewModel>>>;

}
