using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.DTOs.AnalyticsDTOs;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace ExaminationSystem.Application.Feature.Analytics.Query
{
    public class GetAttemptsOverTimeQueryHandler(IGenericRepository<QuizAttempt> _quizAttemptRepo) : IRequestHandler<GetAttemptsOverTimeQuery, RequestResult<List<AttemptsOverTimeDto>>>
    {
        public async Task<RequestResult<List<AttemptsOverTimeDto>>> Handle(GetAttemptsOverTimeQuery request, CancellationToken cancellationToken)
        {
            var attemptsOverTime = await _quizAttemptRepo.GetAll()
                  .Where(a =>
                    (!request.From.HasValue || a.StartedAt >= request.From) &&
                    (!request.To.HasValue || a.StartedAt <= request.To)
                )
                .GroupBy(a => a.StartedAt.Date)
                .Select(g => new AttemptsOverTimeDto
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => x.Date)
                .ToListAsync(cancellationToken);

            return RequestResult<List<AttemptsOverTimeDto>>.Sucess(attemptsOverTime);
        }
    }
}
