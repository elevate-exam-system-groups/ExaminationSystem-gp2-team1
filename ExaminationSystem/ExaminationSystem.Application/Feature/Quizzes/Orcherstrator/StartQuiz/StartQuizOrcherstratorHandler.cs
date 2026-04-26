using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.DTOs.OptionsDTOs;
using ExaminationSystem.Application.Common.DTOs.QuestionsDTOs;
using ExaminationSystem.Application.Common.DTOs.QuizzesDTOs;
using ExaminationSystem.Application.Feature.Quizzes.Query;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Domin.Entities.Enums;
using ExaminationSystem.Entities;
using MediatR;

namespace ExaminationSystem.Application.Feature.Quizzes.Command.StartQuiz;

public class StartQuizOrcherstratorHandler(IMediator _mediator) : IRequestHandler<StartQuizOrcherstrator, RequestResult<StartQuizResponse>>
{
    public async Task<RequestResult<StartQuizResponse>> Handle(StartQuizOrcherstrator request, CancellationToken cancellationToken)
    {
        // need get quiz 
        var quiz = await _mediator.Send(new GetQuizByIdQuery(request.QuizId),cancellationToken);

        if (!quiz.IsSucess) 
        {
            return RequestResult<StartQuizResponse>.Failure(ErrorCode.NotFound, "Quiz not found");
        }

        var quizData = quiz.Data;

        // check existing attempt for the user and quiz
        var existingAttempt = await _mediator.Send(new GetInProgressAttemptQuery(request.QuizId, request.UserId),cancellationToken);
        if (existingAttempt.Data != null)
        {
            return RequestResult<StartQuizResponse>.Failure(
                ErrorCode.ExistingAttempt,
                $"You already have an active attempt with id {existingAttempt.Data}");
        }
            
        // check max attempts
        var hasReachedLimit = await _mediator.Send(new CheckMaxAttemptQuery(request.QuizId,request.UserId,quizData.MaxAttempts),cancellationToken);

        if (hasReachedLimit)
        {
            return RequestResult<StartQuizResponse>.Failure(
                ErrorCode.LimitReached,
                "Attempt limit reached");
        }

        var attempt = await _mediator.Send(new CreateQuizAttemptCommand(request.UserId,request.QuizId));

        if (!attempt.IsSucess) 
        {
            return RequestResult<StartQuizResponse>.Failure(ErrorCode.Conflict, "Failed to create quiz attempt");
        }
        var attemptData = attempt.Data;


        /// shuffle questions and options
        var questions = ShuffleQuestions(quizData.Questions);

        var response = new StartQuizResponse
        {
            AttemptId = attemptData.Id,
            Duration = quizData.DurationMinutes,
            Questions = questions
        };

        return RequestResult<StartQuizResponse>.Sucess(response);
    }


    private List<QuestionWithOptionsDto> ShuffleQuestions(IEnumerable<QuestionWithOptionsDto> questions)
    {
        return questions
            .OrderBy(q => Guid.NewGuid())
            .Select(q => new QuestionWithOptionsDto
            {
                Id = q.Id,
                QuestionId = q.QuestionId,
                Text = q.Text,
                Options = q.Options
                    .OrderBy(o => Guid.NewGuid())
                    .Select(o => new OptionDto
                    {
                        Id = o.Id,
                        OptionId = o.OptionId,
                        Text = o.Text
                    }).ToList()
            }).ToList();
    }

}
