using ExaminationSystem.Domin.Comman;
using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Entities
{
    public class Diploma : AuditableEntity
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "draft";

        public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    }
}