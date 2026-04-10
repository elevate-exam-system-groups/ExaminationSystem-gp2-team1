using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Domin.Comman;

namespace ExaminationSystem.Domin.Entitys
{
    public class Enrollment : BaseEntity
    {
        public int UserId { get; set; }
        public int DiplomaId { get; set; }

        public DateTime EnrolledAt { get; set; }
        public string Status { get; set; } // active / completed

        public DateTime CreatedAt { get; set; }


        public User User { get; set; }
        public Diploma Diploma { get; set; }
    }
}
