using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.DTOs.AttemptDTOs;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Feature.Attempts.Command.Handlers
{
    public class UpdateAttemptAnswerCommandHandler(IGenericRepository<AttemptAnswer>_attemptRepo) 
        : IRequestHandler<UpdateAttemptAnswerCommand, RequestResult<AttemptAnswerDto>>
    {
        public async Task<RequestResult<AttemptAnswerDto>> Handle(UpdateAttemptAnswerCommand request, CancellationToken cancellationToken)
        {
            //var answer = await _attemptRepo.GetAll()
            //.FirstOrDefaultAsync(a => a.Id == request.AnswerId, cancellationToken);
            var answer = await _attemptRepo.GetAll()
                .Where(a=>a.Id == request.AnswerId)
                .Select( a=> new AttemptAnswer
                {
                    Id = a.Id,
                    AttemptId = a.AttemptId,
                    QuestionId = a.QuestionId,
                    SelectedOptionId = a.SelectedOptionId,
                    AnsweredAt = a.AnsweredAt
                }
                ).FirstOrDefaultAsync(cancellationToken);

            if (answer == null)
            {
                return RequestResult<AttemptAnswerDto>.Failure(ErrorCode.NotFound, "Answer not found");
            }


            answer.SelectedOptionId = request.SelectedOptionId;
            answer.AnsweredAt = DateTime.UtcNow;

            await _attemptRepo.Update(answer);
            //await _unitOfWork.SaveChangesAsync(cancellationToken);
            var answerDto = new AttemptAnswerDto
            {
                Id = answer.Id,
                AttemptId = answer.AttemptId,
                QuestionId = answer.QuestionId,
                SelectedOptionId = answer.SelectedOptionId,
                AnsweredAt = answer.AnsweredAt
            };

            return RequestResult<AttemptAnswerDto>.Sucess(answerDto);
        }
    }
}
