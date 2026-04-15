using ExaminationSystem.Domin.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Entities
{
    public class Quiz : AuditableEntity
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [ForeignKey(nameof(Diploma))]
        public Guid DiplomaId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int DurationMinutes { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal PassScore { get; set; } = 60;

        public int? MaxAttempts { get; set; }

        [Required]
        public bool IsPublished { get; set; } = false;

        [MaxLength(1000)]
        public string? Instructions { get; set; }

        public virtual Diploma Diploma { get; set; } = null!;
        public virtual ICollection<Question> Questions { get; set; } = new HashSet<Question>();
        public virtual ICollection<QuizAttempt> Attempts { get; set; } = new HashSet<QuizAttempt>();
    }



}