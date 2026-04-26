using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.DTOs.AnalyticsDTOs;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Entities;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace ExaminationSystem.Application.Feature.Analytics.Query
{
    public class GetPassRateQueryHandler(IGenericRepository<QuizAttempt> _quizAttemptRepo) : IRequestHandler<GetPassRateQuery, RequestResult<List<PassRateDto>>>
    {
        public async Task<RequestResult<List<PassRateDto>>> Handle(GetPassRateQuery request, CancellationToken cancellationToken)
        {
            var passRate = await _quizAttemptRepo.GetAll()
                .Where(a => (!request.From.HasValue || a.StartedAt >= request.From) &&
                            (!request.To.HasValue || a.StartedAt <= request.To))
                    .GroupBy(a => new { a.QuizId, a.Quiz.Title })
                    .Select(g => new PassRateDto
                    {
                        QuizId = g.Key.QuizId,
                        QuizTitle = g.Key.Title,
                        PassRate = (double)g.Count(a => a.IsPassed == true) / g.Count() * 100
                    }).ToListAsync(cancellationToken);


            return RequestResult<List<PassRateDto>>.Sucess(passRate);
        }
    }
}
