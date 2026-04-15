using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.DTOs.AttemptDTOs;
using ExaminationSystem.Domin.Common.Result;
using MediatR;

namespace ExaminationSystem.Application.Feature.Attempts.Command
{
    public record CreateAttemptAnswerCommand(
        Guid AttemptId,
        Guid QuestionId,
        Guid SelectedOptionId
        ) : IRequest<RequestResult<AttemptAnswerDto>>;

}
