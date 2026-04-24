namespace ExaminationSystem.Application.ViewModels.Quizzes
{
    public class StudentAttemptHistoryViewModel
    {
        public Guid AttemptId { get; set; }
        public string QuizTitle { get; set; } = string.Empty;
        public decimal? Score { get; set; }
        public bool Passed { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? SubmittedAt { get; set; }

    }
}
