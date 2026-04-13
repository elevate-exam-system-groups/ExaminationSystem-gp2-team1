using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Common.DTOs.OptionsDTOs
{
    public class OptionDto
    {
        public Guid Id { get; set; }

        public string Text { get; set; } = null!;
    }
}
