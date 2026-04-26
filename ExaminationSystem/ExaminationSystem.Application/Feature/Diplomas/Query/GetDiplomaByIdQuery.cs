using ExaminationSystem.Application.ViewModels.Diplomas;
using ExaminationSystem.Domin.Common.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Feature.Diplomas.Query
{
    public sealed record GetDiplomaByIdQuery(Guid Id) : IRequest<RequestResult<DiplomaViewModel>>;
   
}
