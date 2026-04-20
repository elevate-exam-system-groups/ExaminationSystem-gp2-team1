using ExaminationSystem.Domin.Common;
using ExaminationSystem.Domin.Entities;
using ExaminationSystem.Domin.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Entities
{
    /// <summary>
    /// Lookup table for user roles: "Student" and "Admin"
    /// </summary>
    public class UserRole : AuditableEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public UserRoleCode RoleId { get; set; }
        public Role Role { get; set; } = null!;
    }
}