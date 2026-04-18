using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.DTOs.OptionsDTOs;
using ExaminationSystem.Application.Common.DTOs.QuestionsDTOs;
using ExaminationSystem.Application.DTOs.QuizzesDTOs;
using ExaminationSystem.Application.Feature.Quizzes.Query;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Domin.Entities.Enums;
using ExaminationSystem.Entities;
using MediatR;

namespace ExaminationSystem.Application.Feature.Quizzes.Commond.StartQuiz;

public class StartQuizCommondHandler(IUnitOfWork _unitOfWork,IMediator _mediator) : IRequestHandler<StartQuizCommond, RequestResult<StartQuizResponse>>
{
    public async Task<RequestResult<StartQuizResponse>> Handle(StartQuizCommond request, CancellationToken cancellationToken)
    {
        // get time of starting the quiz
        var now = DateTime.UtcNow;

        // need get quiz 
        var quizResult = await _mediator.Send(new GetQuizByIdQuery(request.QuizId),cancellationToken);

        if (!quizResult.IsSucess) 
        {
            return RequestResult<StartQuizResponse>.Failure(ErrorCode.NotFound, "Quiz not found");
        }

        var quiz = quizResult.Data;

        // check existing attempt for the user and quiz
        var existingAttempt = await _mediator.Send(new GetInProgressAttemptQuery(request.QuizId, request.UserId),cancellationToken);
        if (existingAttempt.Data != null)
        {
            return RequestResult<StartQuizResponse>.Failure(
                ErrorCode.ExistingAttempt,
                $"You already have an active attempt with id {existingAttempt.Data}");
        }
            
        // check max attempts
        var hasReachedLimit = await _mediator.Send(new CheckMaxAttemptQuery(request.QuizId,request.UserId,quiz.MaxAttempts),cancellationToken);

        if (hasReachedLimit)
        {
            return RequestResult<StartQuizResponse>.Failure(
                ErrorCode.LimitReached,
                "Attempt limit reached");
        }


        var attempt = new QuizAttempt
        {
            Id = Guid.NewGuid(),
            StudentId = request.UserId,
            QuizId = request.QuizId,
            Status = QuizAttemptStatus.inProgress,
            StartedAt = now,
            TotalQuestions = quiz.Questions.Count
        };
        _unitOfWork.GetRepository<QuizAttempt>().Add(attempt);
        await _unitOfWork.SaveChangesAsync(cancellationToken);


        var questions = quiz.Questions.OrderBy(q => Guid.NewGuid())
        .Select(q => new QuestionDto
        {
            Id = q.Id,
            Text = q.Text,
            Options = q.Options
                        .OrderBy(o => Guid.NewGuid())
                        .Select(o => new OptionDto
                        {
                            Id = o.Id,
                            Text = o.Text
                        }).ToList()
        }).ToList();

        var response = new StartQuizResponse
        {
            AttemptId = attempt.Id,
            Duration = quiz.DurationMinutes,
            Questions = questions
        };

        return RequestResult<StartQuizResponse>.Sucess(response);
    }
}
