using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.DTOs.QuizzesDTOs;
using ExaminationSystem.Domin.Common.Result;
using MediatR;

namespace ExaminationSystem.Application.Feature.Quizzes.Query;

public class GetQuizByIdQueryHandler : 
    IRequestHandler<GetQuizByIdQuery, RequestResult<QuizDto>>
{
    Task<RequestResult<QuizDto>> IRequestHandler<GetQuizByIdQuery, RequestResult<QuizDto>>.Handle(GetQuizByIdQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
