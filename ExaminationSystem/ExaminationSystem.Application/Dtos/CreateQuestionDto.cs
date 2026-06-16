namespace testexamination.Dtos
{
    public class CreateQuestionDto
    {

        public Guid QuizId { get; set; }
        public string Text { get; set; }
        public int OrderIndex { get; set; }
        public string? Explanation { get; set; }

        public List<CreateOptionDto> Options { get; set; }
    }
}
