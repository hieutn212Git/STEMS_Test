using FluentValidation;

namespace STEMS.MediatR
{
    public abstract class BaseValidator<TRequest> : AbstractValidator<TRequest>
    {
    }
}
