using System;

namespace Nager.EuropeanCommissionVies.Models
{
    /// <summary>
    /// Represents the response of a VAT validation request performed via the
    /// European Commission VIES (VAT Information Exchange System).
    /// </summary>
    /// <remarks>
    /// The response contains the VAT validity status and, depending on the member state,
    /// additional trader information and match indicators for qualified VAT validation.
    /// 
    /// Match fields may return values such as:
    /// VALID, INVALID, NOT_PROCESSED, or NOT_SUPPORTED.
    /// Not all member states support qualified validation.
    /// </remarks>
    public class VatCheckResponse
    {
        /// <summary>
        /// Two-letter ISO 3166-1 alpha-2 country code of the validated VAT number.
        /// </summary>
        public string? CountryCode { get; set; }

        /// <summary>
        /// VAT identification number that was validated.
        /// </summary>
        public string? VatNumber { get; set; }

        /// <summary>
        /// Date of the VAT validation request as returned by VIES.
        /// </summary>
        public DateTime RequestDate { get; set; }

        /// <summary>
        /// Indicates whether the VAT number is valid.
        /// </summary>
        public bool Valid { get; set; }

        /// <summary>
        /// Unique request identifier assigned by the member state.
        /// Can be stored for audit or compliance documentation purposes.
        /// </summary>
        public string? RequestIdentifier { get; set; }

        /// <summary>
        /// Official trader name registered for the VAT number (if provided by the member state).
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Official trader address registered for the VAT number (if provided by the member state).
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Trader name used in the qualified validation request.
        /// </summary>
        public string? TraderName { get; set; }

        /// <summary>
        /// Trader street used in the qualified validation request.
        /// </summary>
        public string? TraderStreet { get; set; }

        /// <summary>
        /// Trader postal code used in the qualified validation request.
        /// </summary>
        public string? TraderPostalCode { get; set; }

        /// <summary>
        /// Trader city used in the qualified validation request.
        /// </summary>
        public string? TraderCity { get; set; }

        /// <summary>
        /// Trader company type used in the qualified validation request.
        /// </summary>
        public string? TraderCompanyType { get; set; }

        /// <summary>
        /// Result of the name comparison for qualified validation.
        /// Possible values: VALID, INVALID, NOT_PROCESSED, NOT_SUPPORTED.
        /// </summary>
        public string? TraderNameMatch { get; set; }

        /// <summary>
        /// Result of the street comparison for qualified validation.
        /// Possible values: VALID, INVALID, NOT_PROCESSED, NOT_SUPPORTED.
        /// </summary>
        public string? TraderStreetMatch { get; set; }

        /// <summary>
        /// Result of the postal code comparison for qualified validation.
        /// Possible values: VALID, INVALID, NOT_PROCESSED, NOT_SUPPORTED.
        /// </summary>
        public string? TraderPostalCodeMatch { get; set; }

        /// <summary>
        /// Result of the city comparison for qualified validation.
        /// Possible values: VALID, INVALID, NOT_PROCESSED, NOT_SUPPORTED.
        /// </summary>
        public string? TraderCityMatch { get; set; }

        /// <summary>
        /// Result of the company type comparison for qualified validation.
        /// Possible values: VALID, INVALID, NOT_PROCESSED, NOT_SUPPORTED.
        /// </summary>
        public string? TraderCompanyTypeMatch { get; set; }
    }
}
