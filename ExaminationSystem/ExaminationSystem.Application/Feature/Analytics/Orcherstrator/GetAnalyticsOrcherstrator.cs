using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.DTOs.AnalyticsDTOs;
using ExaminationSystem.Domin.Common.Result;
using MediatR;

namespace ExaminationSystem.Application.Feature.Analytics.Orcherstrator;

public record GetAnalyticsOrcherstrator (DateTime ?From,DateTime ?To, Guid? DiplomaId) : IRequest<RequestResult<AnalyticsDto>>;
