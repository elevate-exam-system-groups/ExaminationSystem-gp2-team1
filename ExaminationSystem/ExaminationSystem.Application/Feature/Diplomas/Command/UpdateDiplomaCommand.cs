using ExaminationSystem.Domin.Common.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Feature.Diplomas.Command
{
    public sealed record UpdateDiplomaCommand(Guid Id, string Title, string Description) : IRequest<RequestResult<bool>>;
    
}
