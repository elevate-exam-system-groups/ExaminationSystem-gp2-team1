using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.DTOs.AnalyticsDTOs;
using ExaminationSystem.Domin.Common.Result;
using MediatR;

namespace ExaminationSystem.Application.Feature.Analytics.Query;

public record GetAvgScoreByDiplomaIdQuery(DateTime? From, DateTime? To, Guid? DiplomaId) : IRequest<RequestResult<List<AvgScoreDto>>>;