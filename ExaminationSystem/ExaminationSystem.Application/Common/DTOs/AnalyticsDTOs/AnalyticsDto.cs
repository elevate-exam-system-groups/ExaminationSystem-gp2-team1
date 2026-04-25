using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Common.DTOs.AnalyticsDTOs
{
    public class AnalyticsDto
    {
        public List<PassRateDto> PassRateByQuiz { get; set; } = new List<PassRateDto>();
        public List<AvgScoreDto> AvgScoreByDiploma { get; set; } =  new List<AvgScoreDto>();
        public List<AttemptsOverTimeDto> AttemptsOverTime { get; set; } = new List<AttemptsOverTimeDto>();
        public List<FailedQuestionDto> TopFailedQuestions { get; set; } = new List<FailedQuestionDto>();
    }
}
