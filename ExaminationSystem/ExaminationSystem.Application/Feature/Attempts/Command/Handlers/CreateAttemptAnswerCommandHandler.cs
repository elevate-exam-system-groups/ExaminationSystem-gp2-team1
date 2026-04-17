using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.DTOs.AttemptDTOs;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Entities;
using MediatR;

namespace ExaminationSystem.Application.Feature.Attempts.Command.Handlers
{
    public class CreateAttemptAnswerCommandHandler(IGenericRepository<AttemptAnswer> _attemptRepo/*,IUnitOfWork _unitOfWork*/)
        : IRequestHandler<CreateAttemptAnswerCommand, RequestResult<AttemptAnswerDto>>
    {
        public async Task<RequestResult<AttemptAnswerDto>> Handle(CreateAttemptAnswerCommand request, CancellationToken cancellationToken)
        {
            var AttemptAnswer = new AttemptAnswer
            {
                AttemptId = request.AttemptId,
                QuestionId = request.QuestionId,
                SelectedOptionId = request.SelectedOptionId,
                AnsweredAt = DateTime.UtcNow
            };
            _attemptRepo.Add(AttemptAnswer);

            //await _unitOfWork.SaveChangesAsync(cancellationToken);

            var attemptAnswerDto = new AttemptAnswerDto
            {
                AttemptId = AttemptAnswer.AttemptId,
                QuestionId = AttemptAnswer.QuestionId,
                SelectedOptionId = AttemptAnswer.SelectedOptionId,
                AnsweredAt = AttemptAnswer.AnsweredAt
            };

            return RequestResult<AttemptAnswerDto>.Sucess(attemptAnswerDto);
        }
    }
}
