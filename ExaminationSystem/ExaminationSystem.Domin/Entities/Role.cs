using ExaminationSystem.Domin.Entities.Enums;
using ExaminationSystem.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ExaminationSystem.Domin.Entities
{
    public class Role
    {
        [Key]
        public UserRoleCode Id { get; set; }
        public string Name { get; set; } = null!;

        public bool IsAvailable { get; set; } = true;
        public ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
    }
}
