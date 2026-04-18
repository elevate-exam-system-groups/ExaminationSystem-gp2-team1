using ExaminationSystem.Domin.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Entities
{
    public class QuestionOption : AuditableEntity
    {
        [Required]
        [ForeignKey(nameof(Question))]
        public Guid QuestionId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Text { get; set; } = string.Empty;

        [Required]
        public bool IsCorrect { get; set; } = false;

        [Required]
        public int OrderIndex { get; set; }

        public virtual Question Question { get; set; } = null!;
    }
}