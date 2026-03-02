namespace Nager.EuropeanCommissionVies
{
    /// <summary>
    /// Provides helper data related to European Union (EU) member states.
    /// </summary>
    public static class EuCountryCodes
    {
        /// <summary>
        /// Array of two-letter ISO 3166-1 alpha-2 country codes for all current
        /// European Union (EU) member states.
        /// </summary>
        /// <remarks>
        /// Useful for validating VAT numbers, performing EU-specific operations,
        /// or filtering data by EU countries.
        /// Example: "DE" for Germany, "FR" for France, "AT" for Austria.
        /// </remarks>
        public static readonly string[] CountryCodes =
        [
            "AT","BE","BG","HR","CY","CZ","DK",
            "EE","FI","FR","DE","GR","HU","IE",
            "IT","LV","LT","LU","MT","NL","PL",
            "PT","RO","SK","SI","ES","SE"
        ];

        /// <summary>
        /// Checks whether a given two-letter country code belongs to a European Union member state.
        /// </summary>
        /// <param name="countryCode">Two-letter ISO 3166-1 alpha-2 country code to check.</param>
        /// <returns><c>true</c> if the code belongs to an EU member state; otherwise, <c>false</c>.</returns>
        public static bool IsEuCountry(string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode))
            {
                return false;
            }

            return CountryCodes.Contains(countryCode.Trim().ToUpperInvariant());
        }
    }
}
