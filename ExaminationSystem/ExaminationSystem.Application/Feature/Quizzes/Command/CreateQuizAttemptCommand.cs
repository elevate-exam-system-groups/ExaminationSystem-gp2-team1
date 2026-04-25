using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.DTOs.QuizzesDTOs;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Entities.Enums;
using MediatR;

namespace ExaminationSystem.Application.Feature.Quizzes.Command
{
    public record CreateQuizAttemptCommand(Guid StudentId,
        Guid QuizId
        ) : IRequest<RequestResult<QuizAttemptResultDtos>>
    {
    }
}
