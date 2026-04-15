using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.DTOs.AttemptDTOs;
using ExaminationSystem.Domin.Comman.Result;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Feature.Attempts.Query
{
    public class GetAttemptAnswerQueryHandler (IGenericRepository<AttemptAnswer> _attempRepo): IRequestHandler<GetAttemptAnswerQuery, RequestResult<AttemptAnswerDto>>
    {
        public async Task<RequestResult<AttemptAnswerDto>> Handle(GetAttemptAnswerQuery request, CancellationToken cancellationToken)
        {
            var existingAnswer = await _attempRepo.GetAll()
                .Where(a => a.AttemptId == request.AttemptId &&
                a.QuestionId == request.QuestionId).Select(a => new AttemptAnswer
                {
                    Id = a.Id,
                    SelectedOptionId = a.SelectedOptionId,
                    AnsweredAt = DateTime.UtcNow
                }).FirstOrDefaultAsync(cancellationToken);



            if(existingAnswer == null) 
            {
                return RequestResult<AttemptAnswerDto>.Failure(ErrorCode.NotFound, "Answer not found for the given attempt and question.");
            }

            var existingAnswerDto = new AttemptAnswerDto
            {
                Id = existingAnswer.Id,
                AttemptId = request.AttemptId,
                QuestionId = request.QuestionId,
                SelectedOptionId = existingAnswer.SelectedOptionId,
                AnsweredAt = existingAnswer.AnsweredAt
            };

            return RequestResult<AttemptAnswerDto>.Sucess(existingAnswerDto);
        }
    }
}
