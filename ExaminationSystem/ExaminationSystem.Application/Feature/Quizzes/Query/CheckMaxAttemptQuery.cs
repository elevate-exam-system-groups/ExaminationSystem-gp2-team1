using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace ExaminationSystem.Application.Feature.Quizzes.Query;

public record CheckMaxAttemptQuery(Guid QuizId, Guid UserId, int? MaxAttempts) : IRequest<bool>;

