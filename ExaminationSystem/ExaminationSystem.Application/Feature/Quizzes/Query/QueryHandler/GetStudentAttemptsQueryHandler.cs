using ExaminationSystem.Application.Common.Interfaces;
using ExaminationSystem.Application.Feature.Diplomas.Query;
using ExaminationSystem.Application.Pagination;
using ExaminationSystem.Application.ViewModels.Quizzes;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Feature.Quizzes.Query.QueryHandler
{
    public class GetStudentAttemptsQueryHandler(IGenericRepository<QuizAttempt> _quizAttempRepo,IUserService _userService) : IRequestHandler<GetStudentAttemptsQuery, RequestResult<PaginatedList<StudentAttemptHistoryViewModel>>>
    {
        public async Task<RequestResult<PaginatedList<StudentAttemptHistoryViewModel>>> Handle(GetStudentAttemptsQuery request, CancellationToken cancellationToken)
        {
            var stdId = _userService.GetUserId();
            if(stdId == null) {
                return RequestResult<PaginatedList<StudentAttemptHistoryViewModel>>
                .Failure(ErrorCode.Unauthorized, "User is not authenticated.");
            }
            var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
            var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;
            pageSize = pageSize > 100 ? 100 : pageSize;

            var query = _quizAttempRepo.GetAll().AsNoTracking().Where(q => q.StudentId == stdId);

            if(request.DiplomaId.HasValue)
            {
                query.Where(q=>q.Quiz.DiplomaId == request.DiplomaId);
            }
            if(request.QuizId.HasValue){
                query.Where(q=>q.QuizId == request.QuizId);
            }
            var totalCount = await query.CountAsync(cancellationToken);
            var items = query.OrderByDescending(a => a.SubmittedAt).Skip((pageSize - 1) * pageSize).Take(pageSize)
            .Select(a=>new StudentAttemptHistoryViewModel {

            AttemptId = a.Id,
            Passed = a.IsPassed.Value,
            SubmittedAt = a.SubmittedAt,
            QuizTitle = a.Quiz.Title,
            Score = a.Score,
            Status = a.Status.ToString()
            }).ToList();
            var paginatedList = new PaginatedList<StudentAttemptHistoryViewModel>(items,totalCount,pageNumber,pageSize);

            return RequestResult<PaginatedList<StudentAttemptHistoryViewModel>>.Sucess(paginatedList);
        }
    }
}
