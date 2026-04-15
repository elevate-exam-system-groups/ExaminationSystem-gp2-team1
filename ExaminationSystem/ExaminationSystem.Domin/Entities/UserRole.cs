using ExaminationSystem.Domin.Common;
using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Entities
{
    /// <summary>
    /// Lookup table for user roles: "Student" and "Admin"
    /// </summary>
    public class UserRole : AuditableEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }

        // Navigation property
        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
    }
}