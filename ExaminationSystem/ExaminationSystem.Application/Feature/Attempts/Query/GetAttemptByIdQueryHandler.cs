using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.DTOs.AttemptDTOs;
using ExaminationSystem.Application.Feature.Attempts.Command;
using ExaminationSystem.Domin.Comman.Result;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Feature.Attempts.Query
{
    public class GetAttemptByIdQueryHandler(IGenericRepository<QuizAttempt> _quizRepo) : IRequestHandler<GetAttemptByIdQuery, RequestResult<QuizAttemptDTOs>>
    {
        public async Task<RequestResult<QuizAttemptDTOs>> Handle(GetAttemptByIdQuery request, CancellationToken cancellationToken)
        {
            var attempt = await _quizRepo.GetAll()
                .Where(a => a.Id == request.AttemptId)
                .Select(a => new QuizAttemptDTOs
                {
                    QuizId = a.QuizId,
                    StudentId = a.StudentId,
                    Status = a.Status,
                    StartedAt = a.StartedAt,
                    DeadlineAt = a.DeadlineAt
                }).FirstOrDefaultAsync(cancellationToken);

            if (attempt is null) 
            {
                return RequestResult<QuizAttemptDTOs>.Failure(ErrorCode.NotFound, "Attempt not found");
            }
            else
            {
                return RequestResult<QuizAttemptDTOs>.Sucess(attempt);
            } 
        }
    }
}
