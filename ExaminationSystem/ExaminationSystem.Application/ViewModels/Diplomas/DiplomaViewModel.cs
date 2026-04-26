namespace ExaminationSystem.Application.ViewModels.Diplomas
{
    public class DiplomaViewModel
    {
        public string Title { get; init; } = string.Empty;
        public string? Description { get; init; }
        public bool IsPublished { get; init; }
        public int QuizCount { get; init; }
    }
}