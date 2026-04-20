using ExaminationSystem.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Domin.Entities
{
    public class Student
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid id { get; set; }
        public string University { get; set; } = string.Empty;
        public string Major { get; set; } = string.Empty;
    }
}
