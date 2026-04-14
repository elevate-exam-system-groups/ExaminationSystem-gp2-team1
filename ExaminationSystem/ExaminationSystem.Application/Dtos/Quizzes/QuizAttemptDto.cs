namespace testexamination.Dtos
{
    public class QuizAttemptDto
    {
        public int AttemptId { get; set; }
        public double Score { get; set; }
        public DateTime AttemptDate { get; set; }
        public bool IsPassed { get; set; }
    }
}
