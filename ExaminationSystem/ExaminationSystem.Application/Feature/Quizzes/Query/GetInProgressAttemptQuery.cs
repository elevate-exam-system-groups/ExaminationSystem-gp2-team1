using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Entities;
using MediatR;

namespace ExaminationSystem.Application.Feature.Quizzes.Query
{
    public record GetInProgressAttemptQuery(Guid QuizId, Guid UserId) : IRequest<RequestResult<Guid?>>;


}
