using ExaminationSystem.Domin.Comman;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Entities
{
    public class Question : AuditableEntity
    {
        [Required]
        [ForeignKey(nameof(Quiz))]
        public Guid QuizId { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Text { get; set; } = string.Empty;

        [Required]
        public int OrderIndex { get; set; }

        [MaxLength(500)]
        public string? Explanation { get; set; }

        public virtual Quiz Quiz { get; set; } = null!;
        public virtual ICollection<QuestionOption> Options { get; set; } = new HashSet<QuestionOption>();
        public virtual ICollection<AttemptAnswer> Answers { get; set; } = new HashSet<AttemptAnswer>();
    }
}
