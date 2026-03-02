using Newtonsoft.Json;
using System.Net;

namespace Common
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message, bool success)
        {
            StatusCode = statusCode;
            Message = message;
            Success = success;
        }

        public ApiResponse() : this(HttpStatusCode.OK.GetHashCode(), nameof(HttpStatusCode.OK), true)
        {
        }

        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("errors")]
        public IEnumerable<ValidationErrorResponse> Errors { get; set; }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public ApiResponse(int statusCode, string message, T result, bool success)
        {
            StatusCode = statusCode;
            Message = message;
            Result = result;
            Success = success;
        }

        public ApiResponse(T result) : this(HttpStatusCode.OK.GetHashCode(), nameof(HttpStatusCode.OK), result, true)
        {
        }

        public ApiResponse() : this(default!)
        {
        }

        public T Result { get; set; }
    }

    public class ValidationErrorResponse
    {
        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }
        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }
        [JsonProperty("propertyName")]
        public string PropertyName { get; set; }
    }
}
