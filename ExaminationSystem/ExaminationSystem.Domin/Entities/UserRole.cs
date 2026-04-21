using ExaminationSystem.Domin.Common;
using ExaminationSystem.Entities;
using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Domin.Entities
{
    /// <summary>
    /// Junction table that represents the many-to-many relationship between users and roles.
    /// Enables users to have multiple roles (e.g., a user can be both a Student and Admin).
    /// </summary>
    public class UserRole : AuditableEntity
    {
        /// <summary>
        /// Gets or sets the user ID for this role assignment.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the user entity reference.
        /// </summary>
        public User User { get; set; } = null!;

        /// <summary>
        /// Gets or sets the role ID for this assignment.
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// Gets or sets the role entity reference.
        /// </summary>
        public Role Role { get; set; } = null!;
    }
}