using ChicStreetwear.Shared;
using FluentValidation;
using MediatR;

namespace ChicStreetwear.Application.Common.Behaviours;
public sealed class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v =>
                    v.ValidateAsync(context, cancellationToken)));

            var errors = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors.ConvertAll(e => Error.Validation(e.PropertyName, e.ErrorMessage)))
                .ToList();

            if (errors.Any())
            {
                return (dynamic)errors;
            }
        }
        return await next();
    }
}