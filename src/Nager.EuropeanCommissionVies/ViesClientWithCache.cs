using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Nager.EuropeanCommissionVies.Models;
using System;
using System.Net.Http;
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
    public class ViesClientWithCache : ViesClientBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly MemoryCacheEntryOptions _memoryCacheEntryOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViesClientWithCache"/> class.
        /// </summary>
        /// <param name="httpClientFactory">
        /// Factory to create <see cref="HttpClient"/> instances. 
        /// The client is configured with the VIES API base address.
        /// </param>
        /// <param name="memoryCache"></param>
        /// <param name="options"></param>
        public ViesClientWithCache(
            IHttpClientFactory httpClientFactory,
            IMemoryCache memoryCache,
            IOptions<ViesClientOptions> options) : base(httpClientFactory)
        {
            this._memoryCache = memoryCache;

            this._memoryCacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(options.Value.CacheMinutes));
        }

        /// <inheritdoc />
        public async override Task<VatCheckResponse> CheckVatAsync(
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

            if (this._memoryCache.TryGetValue<VatCheckResponse>(vatNumber, out var vatCheckResponse))
            {
                if (vatCheckResponse is not null)
                {
                    return vatCheckResponse;
                }
            }

            vatCheckResponse = await this.CheckVatNumberAsync(vatCheckRequest, cancellationToken);

            this._memoryCache.Set(vatNumber, vatCheckResponse, this._memoryCacheEntryOptions);

            return vatCheckResponse;
        }

        /// <inheritdoc />
        public async override Task<VatCheckResponse> CheckVatAsync(
            VatCheckRequest vatCheckRequest,
            CancellationToken cancellationToken = default)
        {
            if (this._memoryCache.TryGetValue<VatCheckResponse>(vatCheckRequest.VatNumber, out var vatCheckResponse))
            {
                if (vatCheckResponse is not null)
                {
                    return vatCheckResponse;
                }
            }

            vatCheckResponse = await this.CheckVatNumberAsync(vatCheckRequest, cancellationToken);

            this._memoryCache.Set(vatCheckRequest.VatNumber, vatCheckResponse);

            return vatCheckResponse;
        }

        /// <inheritdoc />
        public async override Task<bool> IsValidVatAsync(
            string vatNumber,
            CancellationToken cancellationToken = default)
        {
            var response = await this.CheckVatAsync(vatNumber, cancellationToken);
            return response.Valid;
        }
    }
}
