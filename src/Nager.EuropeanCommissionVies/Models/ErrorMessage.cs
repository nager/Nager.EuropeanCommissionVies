namespace Nager.EuropeanCommissionVies.Models
{
    /// <summary>
    /// Represents a detailed error message provided by the VIES API.
    /// </summary>
    public class ErrorMessage
    {
        /// <summary>
        /// Gets or sets the specific VIES error code (e.g., 'INVALID_INPUT', 'SERVICE_UNAVAILABLE').
        /// </summary>
        public string? Error { get; set; }
    }
}
