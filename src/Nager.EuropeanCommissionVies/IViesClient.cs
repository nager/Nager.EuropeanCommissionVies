using Nager.EuropeanCommissionVies.Models;

namespace Nager.EuropeanCommissionVies
{
    public interface IViesClient
    {
        /// <summary>
        /// Performs a VAT number check against the VIES API and returns detailed validation results.
        /// </summary>
        /// <param name="vatNumber">
        /// VAT number to validate, including the two-letter country code prefix (e.g. "DE123456789").
        /// </param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>
        /// A <see cref="VatCheckResponse"/> containing validation results and optional trader information,
        /// or <c>null</c> if the API request fails.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="vatNumber"/> is null, empty, or too short.</exception>
        /// <remarks>
        /// The VAT number must include at least a 2-letter country code and a national part.
        /// For advanced "qualified" checks, use <see cref="CheckVatAsync(VatCheckRequest, CancellationToken)"/> with trader details.
        /// </remarks>
        Task<VatCheckResponse?> CheckVatAsync(
            string vatNumber,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Performs a VAT number check using a full <see cref="VatCheckRequest"/>, allowing qualified validation with trader details.
        /// </summary>
        /// <param name="vatCheckRequest">Request object containing VAT number and optional trader information.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>
        /// A <see cref="VatCheckResponse"/> with validation results and trader match information,
        /// or <c>null</c> if the API request fails.
        /// </returns>
        /// <remarks>
        /// Use this method to perform qualified VAT validation by providing trader name, address, and company type.
        /// </remarks>
        Task<VatCheckResponse?> CheckVatAsync(
            VatCheckRequest vatCheckRequest,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks whether the specified VAT number is valid.
        /// </summary>
        /// <param name="vatNumber">
        /// VAT number to validate, including the two-letter country code prefix (e.g. "DE123456789").
        /// </param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns><c>true</c> if the VAT number is valid; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="vatNumber"/> is null, empty, or too short.</exception>
        Task<bool> IsValidVatAsync(
            string vatNumber,
            CancellationToken cancellationToken = default);
    }
}
