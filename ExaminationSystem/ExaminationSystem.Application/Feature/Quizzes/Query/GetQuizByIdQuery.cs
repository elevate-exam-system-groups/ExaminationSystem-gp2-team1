using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.DTOs.QuizzesDTOs;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Entities;
using MediatR;

namespace ExaminationSystem.Application.Feature.Quizzes.Query;

public record GetQuizByIdQuery(Guid QuizId) : IRequest<RequestResult<QuizDto>>;


