using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.DTOs.OptionsDTOs;

namespace ExaminationSystem.Application.Common.DTOs.QuestionsDTOs
{
    public class QuestionDto
    {
        public Guid Id { get; set; }

        public string Text { get; set; } = null!;

        public List<OptionDto> Options { get; set; } = new();
    }
}
