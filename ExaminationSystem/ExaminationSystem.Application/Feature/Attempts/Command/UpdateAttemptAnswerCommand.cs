using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace ExaminationSystem.Application.Feature.Attempts.Command;

public record UpdateAttemptAnswerCommand(Guid AnswerId, Guid SelectedOptionId) : IRequest<bool>;

