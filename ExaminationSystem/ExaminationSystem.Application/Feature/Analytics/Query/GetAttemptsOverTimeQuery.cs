using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.DTOs.AnalyticsDTOs;
using ExaminationSystem.Domin.Common.Result;
using MediatR;

namespace ExaminationSystem.Application.Feature.Analytics.Query;

public record GetAttemptsOverTimeQuery(DateTime? From, DateTime? To) : IRequest<RequestResult<List<AttemptsOverTimeDto>>>;
