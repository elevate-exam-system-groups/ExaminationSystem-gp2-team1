using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Domin.Entities.Enums;

namespace ExaminationSystem.Application.Common.DTOs.AttemptDTOs
{
    public class AttemptQuizDto
    {
        public Guid QuizId { get; set; }
        public Guid StudentId { get; set; }

        public QuizAttemptStatus Status { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? DeadlineAt { get; set; }
    }
}
