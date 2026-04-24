using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.DTOs.AnalyticsDTOs;
using ExaminationSystem.Domin.Common.Result;
using MediatR;

namespace ExaminationSystem.Application.Feature.Analytics.Query;

public record GetTopFailedQuestionsQuery(DateTime? From, DateTime? To) : IRequest<RequestResult<List<FailedQuestionDto>>>;
