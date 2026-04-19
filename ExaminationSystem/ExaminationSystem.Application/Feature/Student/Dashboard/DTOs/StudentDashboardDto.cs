namespace ExaminationSystem.Application.Feature.Student.Dashboard.DTOs
{
    public class StudentDashboardDto
    {
        public List<EnrolledDiplomaDto> EnrolledDiplomas { get; set; } = [];
        public List<RecentAttemptDto> RecentAttempts { get; set; } = [];
        public OverallStatsDto OverallStats { get; set; } = new();
    }
}
