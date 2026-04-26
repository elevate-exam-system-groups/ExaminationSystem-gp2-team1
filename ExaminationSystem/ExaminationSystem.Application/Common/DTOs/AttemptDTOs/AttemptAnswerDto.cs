using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Common.DTOs.AttemptDTOs
{
    public class AttemptAnswerDto
    {
        public Guid Id { get; set; }
        public Guid AttemptId { get; set; }
        public Guid QuestionId { get; set; }

        public Guid? SelectedOptionId { get; set; }

        public DateTime AnsweredAt { get; set; }

    }
}
