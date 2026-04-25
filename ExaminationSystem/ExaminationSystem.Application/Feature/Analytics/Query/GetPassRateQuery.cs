using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.DTOs.AnalyticsDTOs;
using ExaminationSystem.Domin.Common.Result;
using MediatR;

namespace ExaminationSystem.Application.Feature.Analytics.Query;

public record GetPassRateQuery (DateTime? From, DateTime? To) : IRequest<RequestResult<List<PassRateDto>>>;
