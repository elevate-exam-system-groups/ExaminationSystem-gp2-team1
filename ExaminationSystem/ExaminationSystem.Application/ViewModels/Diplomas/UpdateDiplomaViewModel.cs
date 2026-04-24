using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.ViewModels.Diplomas
{
    public class UpdateDiplomaViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
