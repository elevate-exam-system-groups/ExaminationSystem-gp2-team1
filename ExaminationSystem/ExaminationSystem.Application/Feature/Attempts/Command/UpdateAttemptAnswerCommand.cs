using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace ExaminationSystem.Application.Feature.Attempts.Command;

// need refactor this return type to RequestResult<object>
public record UpdateAttemptAnswerCommand(Guid AnswerId, Guid SelectedOptionId) : IRequest<bool>;

