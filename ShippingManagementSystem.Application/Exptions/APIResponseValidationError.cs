namespace ShippingManagementSystem.Application.Exptions
{
    public class APIResponseValidationError : APIResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public APIResponseValidationError(IEnumerable<string> errors) : base(400)
        {
            Errors = errors;
        }
    }
}
