using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Feature.Attempts.Command;
using ExaminationSystem.Application.Feature.Attempts.Query;
using ExaminationSystem.Application.Feature.Questions;
using ExaminationSystem.Domin.Comman.Result;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Domin.Entities.Enums;
using ExaminationSystem.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Feature.Attempts.Orcherstrator
{
    public class AnswerQuestionOrcherstratorHandler(
        IMediator _mediator
        ,IUnitOfWork _unitOfWork)
        : IRequestHandler<AnswerQuestionOrcherstrator, RequestResult<bool>>
    {

        public async Task<RequestResult<bool>> Handle(AnswerQuestionOrcherstrator request, CancellationToken cancellationToken)
        {

            var attempt = await _mediator.Send(new GetAttemptByIdQuery(request.AttemptId));

            if (!attempt.IsSucess)
            {
                return RequestResult<bool>.Failure(attempt.ErrorCode, attempt.Message);
            }

            var attemptData = attempt.Data;


            if (attemptData.StudentId != request.UserId)
            {
                return RequestResult<bool>.Failure(ErrorCode.Forbidden, "Not your attempt");
            }

            if (attemptData.DeadlineAt.HasValue && attemptData.DeadlineAt < DateTime.UtcNow)
            {
                return RequestResult<bool>.Failure(ErrorCode.Expired, "Attempt expired");

            }
            if (attemptData.Status == QuizAttemptStatus.submitted)
            {
                return RequestResult<bool>.Failure(ErrorCode.Conflict, "Attempt already submitted");
            }


            var question = await _mediator.Send(new GetQuestionByIdWithOptionQuery(request.QuestionId));

            if(!question.IsSucess) 
            {
                return RequestResult<bool>.Failure(question.ErrorCode, question.Message);
            }
            var questionData = question.Data;

           

            if (questionData.QuizId != attemptData.QuizId)
                return RequestResult<bool>.Failure(ErrorCode.Unprocessable, "Question not part of this quiz");


            var existingAnswer = await _mediator.Send(new GetAttemptAnswerQuery(request.AttemptId, request.QuestionId));

            if (!existingAnswer.IsSucess) 
            {
                return RequestResult<bool>.Failure(existingAnswer.ErrorCode, existingAnswer.Message);
            }

            var existingAnswerData = existingAnswer.Data;

            if (existingAnswerData != null)
            {               
                //await _attemptAnswerRepo.Update(existingAnswerData);
                // need adding data or update to track changes and update the entity in the database
                // so this is comand 
                await _mediator.Send(new UpdateAttemptAnswerCommand(existingAnswerData.Id,request.SelectedOptionId));
            }
            else
            {

                await _mediator.Send(new CreateAttemptAnswerCommand(request.AttemptId, request.QuestionId, request.SelectedOptionId));
  
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return RequestResult<bool>.Sucess(true);
        }
    }
}
