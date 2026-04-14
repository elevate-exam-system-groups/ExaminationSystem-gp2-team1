namespace testexamination.Dtos
{
    public class UpdateQuestionDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public List<UpdateOptionDto> Options { get; set; }
    }
}
