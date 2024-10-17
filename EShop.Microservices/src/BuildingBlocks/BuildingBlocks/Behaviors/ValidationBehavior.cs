using MediatR;
using BuildingBlocks.CQRS;
using FluentValidation;

namespace BuildingBlocks.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) 
                                                     : IPipelineBehavior<TRequest, TResponse>
                                                       where TRequest : ICommand<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var result = await Task.WhenAll(_validators.Select(x=> x.ValidateAsync(context, cancellationToken)));

        var failures = result.Where(x => x.Errors.Any())
                            .SelectMany(x => x.Errors)
                            .ToList();

        if (failures.Count > 0)
        {
            throw new ValidationException(failures);
        }

        return await next();
    }
}
