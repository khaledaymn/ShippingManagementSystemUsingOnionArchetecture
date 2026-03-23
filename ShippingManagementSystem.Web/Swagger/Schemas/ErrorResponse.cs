namespace ShippingManagementSystem.Web.Swagger.Schemas
{
    public class ErrorResponse
    {
        /// <summary>
        /// HTTP status code of the error.
        /// </summary>
        /// <example>400</example>
        public int StatusCode { get; set; }

        /// <summary>
        /// Descriptive error message.
        /// </summary>
        /// <example>Email or Password is incorrect!</example>
        public string Message { get; set; }

        /// <summary>
        /// Technical details (only populated in 500 errors).
        /// </summary>
        /// <example>NullReferenceException at Service...</example>
        public string? Details { get; set; }
    }
}
