using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Common.DTOs.AnalyticsDTOs
{
    public class PassRateDto
    {
        public Guid QuizId { get; set; }
        public string QuizTitle { get; set; } 
        public double PassRate { get; set; }
    }
}
