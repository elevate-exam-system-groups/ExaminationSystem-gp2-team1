using ExaminationSystem.Domin.Common;
using ExaminationSystem.Domin.Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Entities
{
    public class QuizAttempt : AuditableEntity
    {
        [Required]
        [ForeignKey(nameof(Student))]
        public Guid StudentId { get; set; }

        [Required]
        [ForeignKey(nameof(Quiz))]
        public Guid QuizId { get; set; }

        [Required]
        public QuizAttemptStatus Status { get; set; } = QuizAttemptStatus.inProgress;

        public DateTime StartedAt { get; set; } 

        public DateTime? SubmittedAt { get; set; }

        public DateTime? DeadlineAt { get; set; }

        public decimal? Score { get; set; }

        
        public bool? IsPassed { get; set; }

        public int? CorrectAnswerCount { get; set; }
       
        public int TotalQuestions { get; set; }

        // Navigation properties
        public virtual User Student { get; set; } = null!;
        public virtual Quiz Quiz { get; set; } = null!;
        public virtual ICollection<AttemptAnswer> Answers { get; set; } = new HashSet<AttemptAnswer>();
    }
}