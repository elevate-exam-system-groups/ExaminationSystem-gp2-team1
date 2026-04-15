using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Domin.Entities.Enums;
using ExaminationSystem.Entities;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace ExaminationSystem.Application.Feature.Quizzes.Query
{
    public class GetInProgressAttemptQueryHandler(IGenericRepository<QuizAttempt> _Repo) : IRequestHandler<GetInProgressAttemptQuery, RequestResult<Guid?>>
    {
        public async Task<RequestResult<Guid?>> Handle(GetInProgressAttemptQuery request, CancellationToken cancellationToken)
        {
            var attempt = await _Repo.GetAll().Where(a =>
                                    a.StudentId == request.UserId &&
                                     a.QuizId == request.QuizId &&
                                     a.Status == QuizAttemptStatus.inProgress).Select(a => a.Id)
                                     .FirstOrDefaultAsync();

            return RequestResult<Guid?>.Sucess(attempt);

        }
    }
}
