using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.DTOs.AttemptDTOs;
using ExaminationSystem.Domin.Common.Result;
using MediatR;

namespace ExaminationSystem.Application.Feature.Attempts.Query;

public record GetAttemptAnswerQuery(Guid AttemptId, Guid QuestionId) : IRequest<RequestResult<AttemptAnswerDto>>;
