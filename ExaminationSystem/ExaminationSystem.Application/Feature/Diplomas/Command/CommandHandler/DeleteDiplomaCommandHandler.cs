using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Entities;
using MediatR;

namespace ExaminationSystem.Application.Feature.Diplomas.Command.CommandHandler
{
    public class DeleteDiplomaCommandHandler(IGenericRepository<Diploma> _diplomaRepository) : IRequestHandler<DeleteDiplomaCommand, RequestResult<bool>>
    {
        public Task<RequestResult<bool>> Handle(DeleteDiplomaCommand request, CancellationToken cancellationToken)
        {
            var diploma = _diplomaRepository.GetByIdAsync(request.Id).Result;
            if (diploma is null)
            {
                return Task.FromResult(RequestResult<bool>.Failure(ErrorCode.NotFound, "Diploma not found"));
            }
            _diplomaRepository.SoftDelete(diploma);
            return Task.FromResult(RequestResult<bool>.Sucess(true));
        }
    }
}
