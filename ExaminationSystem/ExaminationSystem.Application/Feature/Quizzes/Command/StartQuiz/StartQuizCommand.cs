using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.DTOs.QuizzesDTOs;
using ExaminationSystem.Domin.Common.Result;
using MediatR;

namespace ExaminationSystem.Application.Feature.Quizzes.Commond.StartQuiz;

public record StartQuizCommond(Guid QuizId, Guid UserId) : IRequest<RequestResult<StartQuizResponse>>;

