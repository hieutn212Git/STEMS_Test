using FluentValidation.Results;
using Newtonsoft.Json;

namespace Common.Exceptions
{
    public class InvalidValidationException : Exception
    {
        public readonly IList<FieldError> Errors;

        public InvalidValidationException(IList<ValidationFailure> errors)
        {
            Errors = errors.Select(error => new FieldError
            {
                PropertyName = error.PropertyName,
                ErrorCode = error.ErrorCode,
                ErrorMessage = error.ErrorMessage
            }).ToList();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(Errors);
        }
    }
}
