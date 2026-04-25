using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Common.DTOs.AnalyticsDTOs
{
    public class FailedQuestionDto
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; }
        public double FailureRate { get; set; }
    }
}
