using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.DTOs.QuestionsDTOs;
using ExaminationSystem.Domin.Common.Result;
using MediatR;

namespace ExaminationSystem.Application.Feature.Questions;

public record GetQuestionByIdQuery(Guid QuestionId) : IRequest<RequestResult<QuestionQuizDto>>;

