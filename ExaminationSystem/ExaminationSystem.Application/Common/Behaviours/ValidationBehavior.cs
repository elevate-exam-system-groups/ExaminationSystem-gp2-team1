using MediatR;
using FluentValidation;
using ExaminationSystem.Domin.Common.Result;
namespace ExaminationSystem.Application.Common.Behaviours;


public class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null)
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IResult
{
    private readonly IValidator<TRequest>? _validator = validator;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        if (_validator is null)
        {
            return await next(ct);
        }

        var validationResult = await _validator.ValidateAsync(request, ct);

        if (validationResult.IsValid)
        {
            return await next(ct);
        }

        var errors = validationResult.Errors
            .Select(x => new Error(
                ErrorCode.ValidationError,
                $"invalid {x.PropertyName} due To: \n{x.ErrorMessage}\n"
            ))
            .ToList();


        return (dynamic)errors;
    }

    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        throw new NotImplementedException();
    }
}