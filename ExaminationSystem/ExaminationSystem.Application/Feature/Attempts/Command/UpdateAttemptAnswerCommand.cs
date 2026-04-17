using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.DTOs.AttemptDTOs;
using ExaminationSystem.Domin.Common.Result;
using MediatR;

namespace ExaminationSystem.Application.Feature.Attempts.Command;

// need refactor this return type to RequestResult<object>
public record UpdateAttemptAnswerCommand(Guid AnswerId, Guid SelectedOptionId) : IRequest<RequestResult<AttemptAnswerDto>>;

