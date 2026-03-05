using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using Nager.EuropeanCommissionVies.Models;
using System.Diagnostics;

namespace Nager.EuropeanCommissionVies.UnitTest
{
    [TestClass]
    public sealed class ViesClientWithCacheTest
    {
        public TestContext TestContext { get; set; }

        private ViesClientWithCache GetViesClient()
        {
            var options = Options.Create(new ViesClientOptions { CacheMinutes = 30 });

            var memoryCacheOptions = Options.Create(new MemoryCacheOptions());
            var memoryCache = new MemoryCache(memoryCacheOptions);

            var httpClient = new HttpClient();

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock
                .Setup(_ => _.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            return new ViesClientWithCache(httpClientFactoryMock.Object, memoryCache, options);
        }

        [TestMethod]
        public async Task CheckVatAsync_ValidVatNumber_ReturnsExpectedNameWithRetry()
        {
            var vatNumber = "DE259582878";
            var client = this.GetViesClient();

            VatCheckResponse? vatCheckResponse = null;

            for (var i = 0; i < 20; i++)
            {
                try
                {
                    vatCheckResponse = await client.CheckVatAsync(vatNumber, TestContext.CancellationToken);
                }
                catch (ViesException exception)
                {
                    await Task.Delay(500, TestContext.CancellationToken);
                    Trace.WriteLine($"ViesException: {exception} {exception.RawResponseBody}");
                }                
            }

            Assert.IsNotNull(vatCheckResponse);
            Assert.IsTrue(vatCheckResponse.Valid);
            Assert.AreEqual(vatNumber.Substring(2), vatCheckResponse.VatNumber);
            //Assert.AreEqual(name, vatCheckResponse.Name?.Trim());
        }
    }
}
