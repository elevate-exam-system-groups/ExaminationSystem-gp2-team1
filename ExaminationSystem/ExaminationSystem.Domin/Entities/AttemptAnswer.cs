using ExaminationSystem.Domin.Comman;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Entities
{
    public class AttemptAnswer : AuditableEntity
    {
        [Required]
        [ForeignKey(nameof(Attempt))]
        public Guid AttemptId { get; set; }

        [Required]
        [ForeignKey(nameof(Question))]
        public Guid QuestionId { get; set; }

        [ForeignKey(nameof(SelectedOption))]
        public Guid? SelectedOptionId { get; set; }

        public DateTime AnsweredAt { get; set; } 

        public bool IsCorrect { get; set; } = false;

        // Navigation properties
        public virtual QuizAttempt Attempt { get; set; } = null!;
        public virtual Question Question { get; set; } = null!;
        public virtual QuestionOption? SelectedOption { get; set; }
    }
}