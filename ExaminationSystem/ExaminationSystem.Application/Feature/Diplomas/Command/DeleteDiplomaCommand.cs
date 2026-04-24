using ExaminationSystem.Domin.Common.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Feature.Diplomas.Command
{
    public sealed record DeleteDiplomaCommand(Guid Id) : IRequest<RequestResult<bool>>;
    
}
