using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.DTOs.AttemptDTOs;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Entities;
using MediatR;

namespace ExaminationSystem.Application.Feature.Attempts.Command;

public record GetAttemptByIdQuery(Guid AttemptId) : IRequest<RequestResult<QuizAttemptDTOs>>;

