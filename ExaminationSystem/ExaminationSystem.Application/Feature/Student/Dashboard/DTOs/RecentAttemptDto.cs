namespace ExaminationSystem.Application.Feature.Student.Dashboard.DTOs
{
    public class RecentAttemptDto
    {
        public string QuizTitle { get; set; }
        public decimal? Score { get; set; }
        public bool? Passed { get; set; }
        public DateTime? SubmittedAt { get; set; }
    }
}
