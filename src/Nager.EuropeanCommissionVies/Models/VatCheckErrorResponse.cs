namespace Nager.EuropeanCommissionVies.Models
{
    /// <summary>
    /// Represents the error response returned by the VIES API when a request fails.
    /// </summary>
    public class VatCheckErrorResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether the action succeeded. 
        /// In an error response, this is typically false.
        /// </summary>
        public bool ActionSucceed { get; set; }

        /// <summary>
        /// Gets or sets a collection of error wrappers containing specific error messages and codes.
        /// </summary>
        public ErrorMessage[]? ErrorWrappers { get; set; }
    }
}