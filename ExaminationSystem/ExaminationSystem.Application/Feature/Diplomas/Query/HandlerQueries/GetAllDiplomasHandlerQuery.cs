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
    public class GetAllDiplomasHandlerQuery(IGenericRepository<Diploma> _diplomaRepository) : IRequestHandler<GetAllDiplomasQuery, RequestResult<IEnumerable<DiplomaViewModel>>>
    {
        public async Task<RequestResult<IEnumerable<DiplomaViewModel>>> Handle(GetAllDiplomasQuery request, CancellationToken cancellationToken)
        {
            var diplomas = _diplomaRepository.GetAll();
            if(diplomas == null) {
            return RequestResult<IEnumerable<DiplomaViewModel>>.Failure(ErrorCode.NotFound, "No diplomas found.");
            }
            var diplomaViewModels = diplomas.Select(d => new DiplomaViewModel
            {
                Title = d.Title,
                Description = d.Description,
                IsPublished = d.IsPublished,
                QuizCount = d.Quizzes.Count
            }).ToList();
            return RequestResult<IEnumerable<DiplomaViewModel>>.Sucess(diplomaViewModels);
        }
    }
}
