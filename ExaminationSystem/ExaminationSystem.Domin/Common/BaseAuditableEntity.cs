using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Domin.Common
{
    public class BaseAuditableEntity : BaseEntity
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
