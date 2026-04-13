using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Feature.Quizzes.Query
{
    public class CheckMaxAttemptQueryHandler (IGenericRepository<QuizAttempt> _repo) : IRequestHandler<CheckMaxAttemptQuery, bool>
    {
        public async Task<bool> Handle(CheckMaxAttemptQuery request, CancellationToken cancellationToken)
        {
            if (!request.MaxAttempts.HasValue)
                return false;

            var count = await _repo.GetAll()
            .Where(a =>
                a.StudentId == request.UserId &&
                a.QuizId == request.QuizId)
            .CountAsync(cancellationToken);

            return count >= request.MaxAttempts.Value;
        }
    }
}
