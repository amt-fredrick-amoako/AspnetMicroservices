using FluentValidation;
using MediatR;
using ValidationException = Ordering.Application.Exceptions.ValidationException;

namespace Ordering.Application.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators ?? throw new ArgumentNullException(nameof(validators));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request); // create a validationcontext
                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken))); // run all rules in the validator class
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f!=null).ToList();
                if (failures.Any()) throw new ValidationException(failures);
                
            }
            return await next();
        }
    }
}
