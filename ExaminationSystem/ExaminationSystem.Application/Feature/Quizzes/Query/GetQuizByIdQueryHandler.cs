using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.DTOs.OptionsDTOs;
using ExaminationSystem.Application.Common.DTOs.QuestionsDTOs;
using ExaminationSystem.Application.Common.DTOs.QuizzesDTOs;

using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Feature.Quizzes.Query
{
    public class GetQuizByIdQueryHandler(IGenericRepository<Quiz> _quizRepo) : IRequestHandler<GetQuizByIdQuery, RequestResult<QuizDto>>
    {
        async Task<RequestResult<QuizDto>> IRequestHandler<GetQuizByIdQuery, RequestResult<QuizDto>>.Handle(GetQuizByIdQuery request, CancellationToken cancellationToken)
        {
            var quiz = await _quizRepo.GetAll().Where(q => q.Id == request.QuizId)
                .Select(q => new QuizDto 
                {
                    Id = q.Id,
                    Title = q.Title,
                    DurationMinutes = q.DurationMinutes,
                    MaxAttempts = q.MaxAttempts,
                    Questions = q.Questions
                        .OrderBy(qq => qq.OrderIndex)
                        .Select(qq => new QuestionWithOptionsDto
                        {
                            Id = qq.Id,
                            QuestionId = qq.Id,
                            Text = qq.Text,
                            Options = qq.Options
                                .OrderBy(o => o.OrderIndex)
                                .Select(o => new OptionDto
                                {
                                    Id = o.Id,
                                    OptionId = o.Id,
                                    Text = o.Text
                                }).ToList()
                        }).ToList()

                }).FirstOrDefaultAsync(cancellationToken);

            if (quiz == null) 
            {
                return RequestResult<QuizDto>.Failure(ErrorCode.NotFound, "Quiz not found");
            }
            return RequestResult<QuizDto>.Sucess(quiz);
        }
    }
}
