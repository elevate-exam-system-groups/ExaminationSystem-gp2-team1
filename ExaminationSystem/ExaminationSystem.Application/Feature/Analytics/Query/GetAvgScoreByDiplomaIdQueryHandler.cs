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
    public class GetAvgScoreByDiplomaIdQueryHandler(IGenericRepository<QuizAttempt> _quizAttemptRepo) : IRequestHandler<GetAvgScoreByDiplomaIdQuery, RequestResult<List<AvgScoreDto>>>
    {
        public async Task<RequestResult<List<AvgScoreDto>>> Handle(GetAvgScoreByDiplomaIdQuery request, CancellationToken cancellationToken)
        {
            var avgScore = await _quizAttemptRepo.GetAll().
                Where(a =>
                   (!request.From.HasValue || a.StartedAt >= request.From) &&
                   (!request.To.HasValue || a.StartedAt <= request.To) &&
                   (!request.DiplomaId.HasValue || a.Quiz.DiplomaId == request.DiplomaId)
                )
                .GroupBy(a => a.Quiz.DiplomaId)
                .Select(g => new AvgScoreDto
                {
                    DiplomaId = g.Key,
                    AvgScore = (double)g.Average(a => a.Score ?? 0)
                })
                .ToListAsync(cancellationToken);

            return RequestResult<List<AvgScoreDto>>.Sucess(avgScore);
        }
    }
}
