namespace ExaminationSystem.Application.Feature.Student.Dashboard.DTOs
{
    public class OverallStatsDto
    {
        public decimal AverageScore { get; set; }
        public int TotalAttempts { get; set; }
        public int PassedCount { get; set; }
    }
}
