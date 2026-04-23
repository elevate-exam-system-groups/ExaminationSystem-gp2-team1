using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.DTOs.QuizzesDTOs;
using ExaminationSystem.Domin.Comman.Result;
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
                    MaxAttempts = q.MaxAttempts

                }).FirstOrDefaultAsync(cancellationToken);

            if (quiz == null) 
            {
                return RequestResult<QuizDto>.Failure(ErrorCode.NotFound, "Quiz not found");
            }
            return RequestResult<QuizDto>.Sucess(quiz);
        }
    }
}
