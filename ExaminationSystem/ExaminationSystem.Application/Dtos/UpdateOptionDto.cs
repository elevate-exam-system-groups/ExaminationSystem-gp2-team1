namespace testexamination.Dtos
{
    public class UpdateOptionDto
    {
        public int Id { get; set; } 
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public int OrderIndex { get; set; }
    }
}
