namespace TradeSphere.API.Errors
{
    public class ApiResponse(int statusCode, string? message = null)
    {
        public int StatusCode { get; set; } = statusCode;
        public string Message { get; set; } = message ?? GetMessage(statusCode);
        private static string GetMessage(int statusCode)
            => statusCode switch
            {
                400 => "Bad Request — The server could not process your request due to client error.",
                401 => "Unauthorized — Authentication is required or has failed.",
                403 => "Forbidden — You don't have permission to access this resource.",
                404 => "Not Found — The requested resource could not be found.",
                405 => "Method Not Allowed — The HTTP method is not supported for this resource.",
                408 => "Request Timeout — The server timed out waiting for the request.",
                409 => "Conflict — The request could not be completed due to a conflict.",
                422 => "Unprocessable Entity — The request was well-formed but could not be processed.",
                500 => "Internal Server Error — An unexpected condition occurred on the server.",
                502 => "Bad Gateway — Invalid response from the upstream server.",
                503 => "Service Unavailable — The server is temporarily unable to handle the request.",
                504 => "Gateway Timeout — The server did not receive a timely response.",
                _ => "An unexpected error occurred. Please try again later."
            };
    }
}
