using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.DTOs.OptionsDTOs;
using ExaminationSystem.Application.Common.DTOs.QuestionsDTOs;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Feature.Questions.Handlers
{
    public class GetQuestionByIdWithOptionQueryHandler(IGenericRepository<Question> _questionRepo) 
        : IRequestHandler<GetQuestionByIdWithOptionQuery, RequestResult<QuestionQuizDto>>
    {
        public async Task<RequestResult<QuestionQuizDto>> Handle(GetQuestionByIdWithOptionQuery request, CancellationToken cancellationToken)
        {
            var question = await _questionRepo.GetAll()
                 .Where(q => q.Id == request.QuestionId)
                 .Select(q => new QuestionQuizDto
                 {
                     QuestionId = q.Id,
                        
                     Text = q.Text,
                     QuizId = q.QuizId,
                     Options = q.Options.Select(o => new OptionDto
                     {
                         OptionId = o.Id,
                         Text = o.Text
                     }).ToList()
                 }).FirstOrDefaultAsync(cancellationToken);


            if (question is null)
            {
                return RequestResult<QuestionQuizDto>.Failure(ErrorCode.NotFound, "Question not found");
            }

            return RequestResult<QuestionQuizDto>.Sucess(question);
        }
    }
}
