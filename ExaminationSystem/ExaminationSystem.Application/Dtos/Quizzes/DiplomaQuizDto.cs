namespace testexamination.Dtos
{
    public class DiplomaQuizDto
    {
        public int QuizId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int TotalQuestions { get; set; }
        public double PassingScore { get; set; }
        public List<QuizAttemptDto> ScoreHistory { get; set; } = new();
    }
}
