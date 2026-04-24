using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.DTOs.AnalyticsDTOs;
using ExaminationSystem.Application.Feature.Analytics.Query;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Feature.Analytics.Orcherstrator
{
    public class GetAnalyticsOrcherstratorHandler(IMediator _mediator) : IRequestHandler<GetAnalyticsOrcherstrator, RequestResult<AnalyticsDto>>
    {
        public async Task<RequestResult<AnalyticsDto>> Handle(GetAnalyticsOrcherstrator request, CancellationToken cancellationToken)
        {
            //// cal pass rate 

            //var passRate = await _mediator.Send(new GetPassRateQuery(request.From,request.To));



            //// avg score by diploma

            //var avgScore = await _mediator.Send(new GetAvgScoreByDiplomaIdQuery(request.From,request.To,request.DiplomaId));


            ////Attempts Over Time

            //var attemptsOverTime = await _mediator.Send(new GetAttemptsOverTimeQuery(request.From,request.To));

            ////Top Failed Questions

            //var topFailedQuestions = await _mediator.Send(new GetTopFailedQuestionsQuery(request.From,request.To));



            ///
            /// i think pralllel execution will be better here to reduce the time of response
            ///

            var passRateTask = _mediator.Send(new GetPassRateQuery(request.From, request.To));
            var avgScoreTask = _mediator.Send(new GetAvgScoreByDiplomaIdQuery(request.From, request.To, request.DiplomaId));
            var attemptsTask = _mediator.Send(new GetAttemptsOverTimeQuery(request.From, request.To));
            var failedTask = _mediator.Send(new GetTopFailedQuestionsQuery(request.From, request.To));

            await Task.WhenAll(passRateTask, avgScoreTask, attemptsTask, failedTask);


            return RequestResult<AnalyticsDto>.Sucess(new AnalyticsDto
            {
                PassRateByQuiz = passRateTask.Result.Data,
                AvgScoreByDiploma = avgScoreTask.Result.Data,
                AttemptsOverTime = attemptsTask.Result.Data,
                TopFailedQuestions = failedTask.Result.Data
            });


        }
    }
}
