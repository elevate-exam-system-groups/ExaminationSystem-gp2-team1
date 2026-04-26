using ExaminationSystem.Application.ViewModels.Quizzes;
using ExaminationSystem.Domin.Common.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Feature.Quizzes.Query
{
    public record GetAttemptTimerQuery(Guid AttemptId)
    : IRequest<RequestResult<TimerViewModel>>;
}
