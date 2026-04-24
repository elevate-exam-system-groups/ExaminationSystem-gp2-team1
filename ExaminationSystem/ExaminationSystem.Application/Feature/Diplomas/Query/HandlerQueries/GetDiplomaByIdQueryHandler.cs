using ExaminationSystem.Application.ViewModels.Diplomas;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Feature.Diplomas.Query.HandlerQueries
{
    public class GetDiplomaByIdQueryHandler(IGenericRepository<Diploma> _diplomaRepository) : IRequestHandler<GetDiplomaByIdQuery, RequestResult<DiplomaViewModel>>
    {
        public async Task<RequestResult<DiplomaViewModel>> Handle(GetDiplomaByIdQuery request, CancellationToken cancellationToken)
        {
            var diploma = await _diplomaRepository.GetByIdAsync(request.Id);
            if (diploma == null)
            {
                return RequestResult<DiplomaViewModel>.Failure(ErrorCode.NotFound,"Diploma not found");
            }

            var diplomaViewModel = new DiplomaViewModel
            {
               Title = diploma.Title,
                Description = diploma.Description,
                IsPublished = diploma.IsPublished,
                QuizCount = diploma.Quizzes.Count,
            };

            return RequestResult<DiplomaViewModel>.Sucess(diplomaViewModel);
        }
    }
}
