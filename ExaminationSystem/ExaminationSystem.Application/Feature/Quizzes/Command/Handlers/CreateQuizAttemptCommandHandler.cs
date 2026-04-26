using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.DTOs.QuizzesDTOs;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Domin.Entities.Enums;
using ExaminationSystem.Entities;
using MediatR;

namespace ExaminationSystem.Application.Feature.Quizzes.Command.Handlers
{
    public class CreateQuizAttemptCommandHandler(IGenericRepository<QuizAttempt> _quizAttemptRepo)
        : IRequestHandler<CreateQuizAttemptCommand, RequestResult<QuizAttemptResultDtos>>
    {
        public async Task<RequestResult<QuizAttemptResultDtos>> Handle(CreateQuizAttemptCommand request, CancellationToken cancellationToken)
        {
            var quizAttempt = new QuizAttempt
            {
                QuizId = request.QuizId,
                StudentId = request.StudentId,
                StartedAt = DateTime.UtcNow,
                Status = QuizAttemptStatus.inProgress
            };

            _quizAttemptRepo.Add(quizAttempt);
            // don't need savechanges here cuz we don't use transaction 

            // return Dto object metadata
            var quizAttemptResult = new QuizAttemptResultDtos
            {
                Id = quizAttempt.Id,
                QuizId = quizAttempt.QuizId,
                StudentId = quizAttempt.StudentId,
                StartedAt = quizAttempt.StartedAt,
                Status = quizAttempt.Status
            };
            return RequestResult<QuizAttemptResultDtos>.Sucess(quizAttemptResult);
        }
    }
}
