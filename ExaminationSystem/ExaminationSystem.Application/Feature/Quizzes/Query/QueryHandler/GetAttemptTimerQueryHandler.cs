using ExaminationSystem.Application.ViewModels.Quizzes;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Domin.Entities.Enums;
using ExaminationSystem.Entities;
using MediatR;

namespace ExaminationSystem.Application.Feature.Quizzes.Query.QueryHandler
{
    public class GetAttemptTimerQueryHandler(
        IGenericRepository<QuizAttempt> _attemptRepository)
        : IRequestHandler<GetAttemptTimerQuery, RequestResult<TimerViewModel>>
    {
        public async Task<RequestResult<TimerViewModel>> Handle(
            GetAttemptTimerQuery request,
            CancellationToken cancellationToken)
        {
            var attempt = await _attemptRepository.GetByIdAsync(request.AttemptId);

            if (attempt == null)
            {
                return RequestResult<TimerViewModel>
                    .Failure(ErrorCode.NotFound, "Attempt not found.");
            }

            if (attempt.DeadlineAt == null)
            {
                return RequestResult<TimerViewModel>
                    .Failure(ErrorCode.NotFound, "Attempt deadline not found.");
            }

            if (attempt.Status != QuizAttemptStatus.inProgress)
            {
                return RequestResult<TimerViewModel>
                    .Failure(ErrorCode.NotFound, "Attempt is not in progress.");
            }

            var now = DateTime.UtcNow;

            if (now >= attempt.DeadlineAt.Value)
            {
                attempt.Status = QuizAttemptStatus.timedOut;
                attempt.SubmittedAt = now;

                await _attemptRepository.Update(attempt);

                return RequestResult<TimerViewModel>
                    .Failure(ErrorCode.NotFound, "Attempt time expired.");
            }

            var secondsRemaining = (int)(attempt.DeadlineAt.Value - now).TotalSeconds;

            return RequestResult<TimerViewModel>
                .Sucess(new TimerViewModel
                {
                    SecondsRemaining = secondsRemaining
                });
        }
    }
}
