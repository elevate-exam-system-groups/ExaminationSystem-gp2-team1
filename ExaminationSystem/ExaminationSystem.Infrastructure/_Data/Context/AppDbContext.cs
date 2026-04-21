using ExaminationSystem.Domin.Entities;
using ExaminationSystem.Domin.Entities;
using ExaminationSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace ExaminationSystem.Infrastructure._Data.Context
{
    public class AppDbContext : DbContext
    {    
        // DbSets
        public DbSet<UserRole> UserRoles { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<OtpRecord> OtpRecords { get; set; } = null!;
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public DbSet<Diploma> Diplomas { get; set; } = null!;
        public DbSet<Quiz> Quizzes { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<QuestionOption> QuestionOptions { get; set; } = null!;
        public DbSet<QuizAttempt> QuizAttempts { get; set; } = null!;
        public DbSet<AttemptAnswer> AttemptAnswers { get; set; } = null!;
        public DbSet<StudentDiplomaEnrollment> StudentDiplomaEnrollments { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(log => Debug.WriteLine(log), LogLevel.Information);
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }

}
