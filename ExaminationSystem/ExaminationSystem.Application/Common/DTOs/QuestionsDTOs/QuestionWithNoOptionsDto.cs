using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Common.DTOs.QuestionsDTOs
{
    public class QuestionWithNoOptionsDto
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }

        public string Text { get; set; } = null!;
    }
}
