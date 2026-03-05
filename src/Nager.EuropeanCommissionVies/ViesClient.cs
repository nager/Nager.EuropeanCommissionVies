using Nager.EuropeanCommissionVies.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Nager.EuropeanCommissionVies
{
    /// <summary>
    /// Client for accessing the European Commission VIES (VAT Information Exchange System) REST API.
    /// </summary>
    /// <remarks>
    /// This client allows you to check the validity of EU VAT numbers and optionally perform
    /// qualified VAT checks with additional trader information.
    /// It uses an injected <see cref="IHttpClientFactory"/> to create <see cref="HttpClient"/> instances.
    /// </remarks>
    public class ViesClient : IViesClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViesClient"/> class.
        /// </summary>
        /// <param name="httpClientFactory">
        /// Factory to create <see cref="HttpClient"/> instances. 
        /// The client is configured with the VIES API base address.
        /// </param>
        public ViesClient(IHttpClientFactory httpClientFactory)
        {
            this._httpClient = httpClientFactory.CreateClient();
            this._httpClient.BaseAddress = new Uri("https://ec.europa.eu/taxation_customs/vies/rest-api/");

            this._jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        /// <inheritdoc />
        public async Task<bool> IsValidVatAsync(
            string vatNumber,
            CancellationToken cancellationToken = default)
        {
            var response = await this.CheckVatAsync(vatNumber, cancellationToken);
            return response.Valid;
        }

        /// <inheritdoc />
        public async Task<VatCheckResponse> CheckVatAsync(
            string vatNumber,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(vatNumber))
            {
                throw new ArgumentNullException(nameof(vatNumber), "VAT number cannot be null.");
            }

            if (vatNumber.Length < 3)
            {
                throw new ArgumentException("VAT number must include at least a 2-letter country code plus national part.", nameof(vatNumber));
            }

            var countryCode = vatNumber[..2];
            var nationalPart = vatNumber[2..];

            var vatCheckRequest = new VatCheckRequest
            {
                CountryCode = countryCode,
                VatNumber = nationalPart,
            };

            using var httpResponseMessage = await this._httpClient.PostAsJsonAsync("check-vat-number", vatCheckRequest, this._jsonSerializerOptions, cancellationToken);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                var errorContent = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);

                throw new ViesException(
                    $"VIES API returned {(int)httpResponseMessage.StatusCode} ({httpResponseMessage.ReasonPhrase})",
                    httpResponseMessage.StatusCode,
                    errorContent
                );
            }

            var jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
            if (jsonResponse.Contains("errorWrappers", StringComparison.OrdinalIgnoreCase))
            {
                var vatCheckErrorResponse = JsonSerializer.Deserialize<VatCheckErrorResponse>(jsonResponse, this._jsonSerializerOptions);

                throw new ViesException(
                    $"VIES API returned {(int)httpResponseMessage.StatusCode} ({httpResponseMessage.ReasonPhrase})",
                    httpResponseMessage.StatusCode,
                    jsonResponse,
                    vatCheckErrorResponse
                );
            }

            var checkResponse = JsonSerializer.Deserialize<VatCheckResponse>(jsonResponse, this._jsonSerializerOptions);
            if (checkResponse is null)
            {
                throw new ViesException(
                    "VIES API returned a successful status code, but the response body could not be deserialized or was empty.",
                    httpResponseMessage.StatusCode,
                    jsonResponse
                );
            }

            return checkResponse;
        }

        /// <inheritdoc />
        public async Task<VatCheckResponse> CheckVatAsync(
            VatCheckRequest vatCheckRequest,
            CancellationToken cancellationToken = default)
        {
            using var httpResponseMessage = await this._httpClient.PostAsJsonAsync("check-vat-number", vatCheckRequest, this._jsonSerializerOptions, cancellationToken);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                var errorContent = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);

                throw new ViesException(
                    $"VIES API returned {(int)httpResponseMessage.StatusCode} ({httpResponseMessage.ReasonPhrase})",
                    httpResponseMessage.StatusCode,
                    errorContent
                );
            }

            var jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
            if (jsonResponse.Contains("errorWrappers", StringComparison.OrdinalIgnoreCase))
            {
                var vatCheckErrorResponse = JsonSerializer.Deserialize<VatCheckErrorResponse>(jsonResponse, this._jsonSerializerOptions);

                throw new ViesException(
                    $"VIES API returned {(int)httpResponseMessage.StatusCode} ({httpResponseMessage.ReasonPhrase})",
                    httpResponseMessage.StatusCode,
                    jsonResponse,
                    vatCheckErrorResponse
                );
            }

            var checkResponse = JsonSerializer.Deserialize<VatCheckResponse>(jsonResponse, this._jsonSerializerOptions);
            if (checkResponse is null)
            {
                throw new ViesException(
                    "VIES API returned a successful status code, but the response body could not be deserialized or was empty.",
                    httpResponseMessage.StatusCode,
                    jsonResponse
                );
            }

            return checkResponse;
        }
    }
}
