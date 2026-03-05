using Nager.EuropeanCommissionVies.Models;
using System;
using System.Net;

namespace Nager.EuropeanCommissionVies
{
    /// <summary>
    /// Represents errors that occur during communication with the VIES (VAT Information Exchange System) REST API.
    /// </summary>
    public class ViesException : Exception
    {
        /// <summary>
        /// Gets the HTTP status code returned by the VIES API.
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Gets the raw response body from the API. 
        /// This is useful for debugging when the response cannot be deserialized into a structured object.
        /// </summary>
        public string? RawResponseBody { get; }

        /// <summary>
        /// Gets the structured error details if the API provided a valid error response in JSON format.
        /// </summary>
        public VatCheckErrorResponse? VatCheckErrorResponse { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViesException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="statusCode">The <see cref="HttpStatusCode"/> received from the API.</param>
        /// <param name="responseBody">The raw content of the API response.</param>
        /// <param name="vatCheckErrorResponse">The optional deserialized error response object.</param>
        public ViesException(
            string message,
            HttpStatusCode statusCode,
            string responseBody,
            VatCheckErrorResponse? vatCheckErrorResponse = default) : base(message)
        {
            this.StatusCode = statusCode;
            this.RawResponseBody = responseBody;
            this.VatCheckErrorResponse = vatCheckErrorResponse;
        }
    }
}
