using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Feature.Diplomas.Command.CommandHandler
{
    public class AddDiplomaCommandHandler : IRequestHandler<AddDiplomaCommand, RequestResult<bool>>
    {
        private readonly IGenericRepository<Diploma> _diplomaRepository;

        public AddDiplomaCommandHandler(IGenericRepository<Diploma> diplomaRepository)
        {
            _diplomaRepository = diplomaRepository;
        }

        public async Task<RequestResult<bool>> Handle(AddDiplomaCommand request, CancellationToken cancellationToken)
        {
            var diploma = new Diploma
            {
                Title = request.Title,
                Description = request.Description,
            };
            
             _diplomaRepository.Add(diploma);
            return RequestResult<bool>.Sucess(true);
        }
    }
}