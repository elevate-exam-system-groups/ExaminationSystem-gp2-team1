using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.DTOs.QuestionsDTOs;

namespace ExaminationSystem.Application.DTOs.QuizzesDTOs
{
    public class StartQuizResponse
    {
        public Guid AttemptId { get; set; }

        public int Duration { get; set; }

        public List<QuestionWithOptionsDto> Questions { get; set; } = new();
    }
}
