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
    public class GetTopFailedQuestionsQueryHandler (IGenericRepository<AttemptAnswer> _attemptAnswerRepo): IRequestHandler<GetTopFailedQuestionsQuery, RequestResult<List<FailedQuestionDto>>>
    {
        public async Task<RequestResult<List<FailedQuestionDto>>> Handle(GetTopFailedQuestionsQuery request, CancellationToken cancellationToken)
        {
            var failedQuestions = await _attemptAnswerRepo.GetAll()
                .Where(a =>
                (!request.From.HasValue || a.AnsweredAt >= request.From) &&
                (!request.To.HasValue || a.AnsweredAt <= request.To)
            )
            .GroupBy(a => a.QuestionId)
            .Select(g => new FailedQuestionDto
            {
                QuestionId = g.Key,
                FailureRate = 1 - (g.Count(a => a.IsCorrect) / (double)g.Count())
            })
            .Where(x => x.FailureRate > 0.4)
            .OrderByDescending(x => x.FailureRate)
            .Take(5)
            .ToListAsync(cancellationToken);

            return RequestResult<List<FailedQuestionDto>>.Sucess(failedQuestions);
        }
    }
}
