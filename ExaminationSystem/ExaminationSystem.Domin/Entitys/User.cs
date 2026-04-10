using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Domin.Comman;

namespace ExaminationSystem.Domin.Entitys
{
    public class User : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string HashPassword { get; set; }
        public string Role { get; set; } // student, admin
        public string Status { get; set; } // pending ,active
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }


        public virtual ICollection<Enrollment> Enrollments { get; set; } = new HashSet<Enrollment>();
        public virtual ICollection<Diploma> Diplomas { get; set; } = new HashSet<Diploma>();
        public virtual ICollection<Attempt> Attempts { get; set; } = new HashSet<Attempt>();

    }
}
