using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Feature.Attempts.Command
{
    public class UpdateAttemptAnswerCommandHandler(IGenericRepository<AttemptAnswer>_attemptRepo/*,IUnitOfWork _unitOfWork*/) : IRequestHandler<UpdateAttemptAnswerCommand, bool>
    {
        public async Task<bool> Handle(UpdateAttemptAnswerCommand request, CancellationToken cancellationToken)
        {
            var answer = await _attemptRepo.GetAll()
            .FirstOrDefaultAsync(a => a.Id == request.AnswerId, cancellationToken);

            if (answer == null)
            {
                return false;
            }

            answer.SelectedOptionId = request.SelectedOptionId;
            answer.AnsweredAt = DateTime.UtcNow;

            await _attemptRepo.Update(answer);
            //await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
