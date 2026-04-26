using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Feature.Diplomas.Command.CommandHandler
{
    public class UpdateDiplomaCommandHandler : IRequestHandler<UpdateDiplomaCommand, RequestResult<bool>>
    {
        private readonly IGenericRepository<Diploma> _diplomaRepository;

        public UpdateDiplomaCommandHandler(IGenericRepository<Diploma> diplomaRepository)
        {
            _diplomaRepository = diplomaRepository;
        }

        public async Task<RequestResult<bool>> Handle(UpdateDiplomaCommand request, CancellationToken cancellationToken)
        {
            var diploma = await _diplomaRepository.GetByIdAsync(request.Id);

            if (diploma == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Diploma not found.");
            }

            diploma.Title = request.Title;
            diploma.Description = request.Description;

            await _diplomaRepository.Update(diploma);

            return RequestResult<bool>.Sucess(true);
        }
    }
}
