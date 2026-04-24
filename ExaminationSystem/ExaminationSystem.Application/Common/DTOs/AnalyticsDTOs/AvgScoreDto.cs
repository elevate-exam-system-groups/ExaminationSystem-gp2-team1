using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Common.DTOs.AnalyticsDTOs
{
    public class AvgScoreDto
    {
        public Guid DiplomaId { get; set; }
        public double AvgScore { get; set; }
    }
}
