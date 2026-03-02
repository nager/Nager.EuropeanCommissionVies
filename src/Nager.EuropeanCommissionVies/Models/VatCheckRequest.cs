namespace Nager.EuropeanCommissionVies.Models
{
    /// <summary>
    /// Represents a VAT validation request for the European Commission VIES (VAT Information Exchange System) service.
    /// </summary>
    /// <remarks>
    /// A basic validation requires only <see cref="CountryCode"/> and <see cref="VatNumber"/>.
    /// 
    /// For a qualified VAT validation (recommended for intra-EU tax-exempt supplies),
    /// additional trader information can be provided to verify whether the VAT number
    /// matches the supplied company details.
    /// </remarks>
    /// <example>
    /// Example of a qualified VAT validation request:
    /// <code>
    /// var request = new VatCheckRequest
    /// {
    ///     CountryCode = "DE",
    ///     VatNumber = "123456789",
    ///     RequesterMemberStateCode = "AT",
    ///     RequesterNumber = "U12345678",
    ///     TraderName = "Muster GmbH",
    ///     TraderStreet = "Musterstraße 1",
    ///     TraderPostalCode = "10115",
    ///     TraderCity = "Berlin",
    ///     TraderCompanyType = "GmbH"
    /// };
    /// </code>
    /// </example>
    public class VatCheckRequest
    {
        /// <summary>
        /// Two-letter ISO 3166-1 alpha-2 country code of the VAT number to validate (e.g. "DE", "FR").
        /// </summary>
        public required string CountryCode { get; set; }

        /// <summary>
        /// VAT identification number to validate (without the country code prefix).
        /// </summary>
        public required string VatNumber { get; set; }

        /// <summary>
        /// Two-letter ISO 3166-1 alpha-2 country code of the requesting member state.
        /// Required for qualified VAT validation requests.
        /// </summary>
        public string? RequesterMemberStateCode { get; set; }

        /// <summary>
        /// VAT identification number of the requesting trader.
        /// Required for qualified VAT validation requests.
        /// </summary>
        public string? RequesterNumber { get; set; }

        /// <summary>
        /// Name of the trader being validated.
        /// Used for qualified validation in supported member states.
        /// </summary>
        public string? TraderName { get; set; }

        /// <summary>
        /// Street and house number of the trader being validated.
        /// Used for qualified validation in supported member states.
        /// </summary>
        public string? TraderStreet { get; set; }

        /// <summary>
        /// Postal code of the trader being validated.
        /// Used for qualified validation in supported member states.
        /// </summary>
        public string? TraderPostalCode { get; set; }

        /// <summary>
        /// City of the trader being validated.
        /// Used for qualified validation in supported member states.
        /// </summary>
        public string? TraderCity { get; set; }

        /// <summary>
        /// Company type or legal form of the trader (if applicable).
        /// Optional and only evaluated by certain member states.
        /// </summary>
        public string? TraderCompanyType { get; set; }
    }
}
