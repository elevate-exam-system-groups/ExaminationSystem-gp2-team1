using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ExaminationSystem.Domin.Entities.Enums;
using ExaminationSystem.Entities;

namespace ExaminationSystem.Application.Common.DTOs.QuizzesDTOs
{
    public class QuizAttemptResultDtos
    {

        public Guid Id { get; set; }
        public Guid StudentId { get; set; }

        public Guid QuizId { get; set; }

        public QuizAttemptStatus Status { get; set; }

        public DateTime StartedAt { get; set; }    

        public int TotalQuestions { get; set; }
    }
}
