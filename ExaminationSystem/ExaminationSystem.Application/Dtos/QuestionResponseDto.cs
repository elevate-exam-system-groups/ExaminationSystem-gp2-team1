namespace testexamination.Dtos
{
    public class QuestionResponseDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public int OrderIndex { get; set; }
        public string? Explanation { get; set; }

        public List<OptionResponseDto> Options { get; set; }
    }
}
