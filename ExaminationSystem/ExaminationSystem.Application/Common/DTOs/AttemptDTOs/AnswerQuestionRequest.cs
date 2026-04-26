using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Common.DTOs.AttemptDTOs;

/// <summary>
/// why cuz we don't need change this data 
/// record is immutable data structure 
/// </summary>
/// <param name="UserId"></param>
/// <param name="QuestionId"></param>
/// <param name="SelectedOptionId"></param>
public record AnswerQuestionRequest(
Guid UserId,
Guid QuestionId,
Guid SelectedOptionId);


