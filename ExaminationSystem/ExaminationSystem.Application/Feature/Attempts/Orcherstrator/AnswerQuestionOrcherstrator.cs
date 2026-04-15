using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Domin.Common.Result;
using MediatR;

namespace ExaminationSystem.Application.Feature.Attempts.Orcherstrator;

public record AnswerQuestionOrcherstrator(
    Guid AttemptId,
    Guid UserId,
    Guid QuestionId,
    Guid SelectedOptionId) : IRequest<RequestResult<bool>>;


