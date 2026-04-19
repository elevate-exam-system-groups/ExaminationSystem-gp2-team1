using ExaminationSystem.Application.Abstractions;
using ExaminationSystem.Application.Feature.Student.Dashboard.DTOs;
using ExaminationSystem.Domin.Comman.Result;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace ExaminationSystem.Application.Feature.Student.Dashboard.Queries
{
    public class GetStudentDashboardHandler
    : IRequestHandler<GetStudentDashboardQuery, Result<StudentDashboardDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cache;

        public GetStudentDashboardHandler(IUnitOfWork unitOfWork, ICacheService cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<Result<StudentDashboardDto>> Handle(
            GetStudentDashboardQuery request,
            CancellationToken cancellationToken)
        {
            var cacheKey = $"dashboard:{request.StudentId}";

            var cached = await _cache.GetAsync<StudentDashboardDto>(cacheKey);
            if (cached is not null)
                return cached;

            // 🟢 Attempts
            var attempts = await _unitOfWork
                .GetRepository<QuizAttempt>()
                .GetAll()
                .Where(a => a.StudentId == request.StudentId)
                .Select(a => new
                {
                    a.Score,
                    a.IsPassed,
                    a.SubmittedAt,
                    QuizTitle = a.Quiz.Title,
                    DiplomaId = a.Quiz.DiplomaId
                })
                .ToListAsync(cancellationToken);

            // 🟢 Enrollments
            var enrollments = await _unitOfWork
                .GetRepository<StudentDiplomaEnrollment>()
                .GetAll()
                .Where(e => e.StudentId == request.StudentId)
                .Select(e => new
                {
                    e.DiplomaId,
                    e.Diploma.Title,
                    TotalQuizzes = e.Diploma.Quizzes.Count(q => q.IsPublished)
                })
                .ToListAsync(cancellationToken);

            var dashboard = new StudentDashboardDto();

            // ✔️ Recent Attempts
            dashboard.RecentAttempts = attempts
                .OrderByDescending(a => a.SubmittedAt)
                .Take(5)
                .Select(a => new RecentAttemptDto
                {
                    QuizTitle = a.QuizTitle,
                    Score = a.Score,
                    Passed = a.IsPassed,
                    SubmittedAt = a.SubmittedAt
                })
                .ToList();

            // ✔️ Stats
            dashboard.OverallStats = new OverallStatsDto
            {
                AverageScore = attempts.Where(a => a.Score.HasValue)
                                       .Select(a => a.Score.Value)
                                       .DefaultIfEmpty(0)
                                       .Average(),

                TotalAttempts = attempts.Count,

                PassedCount = attempts.Count(a => a.IsPassed == true)
            };

            // ✔️ Diplomas
            dashboard.EnrolledDiplomas = enrollments
                .Select(e => new EnrolledDiplomaDto
                {
                    Title = e.Title,
                    TotalQuizzes = e.TotalQuizzes,

                    CompletedQuizzes = attempts
                        .Where(a => a.DiplomaId == e.DiplomaId && a.IsPassed == true)
                        .Select(a => a.QuizTitle)
                        .Distinct()
                        .Count()
                })
                .ToList();

            await _cache.SetAsync(cacheKey, dashboard, TimeSpan.FromSeconds(60));

            return dashboard;
        }
    }
}
