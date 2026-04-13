using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.DTOs.QuestionsDTOs;

namespace ExaminationSystem.Application.Common.DTOs.QuizzesDTOs
{
    public class QuizDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public int DurationMinutes { get; set; }
        public int? MaxAttempts { get; set; }

        public List<QuestionDto> Questions { get; set; } = new();
    }
}
