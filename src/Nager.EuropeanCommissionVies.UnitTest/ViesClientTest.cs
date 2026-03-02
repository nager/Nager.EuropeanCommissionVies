using Moq;

namespace Nager.EuropeanCommissionVies.UnitTest
{
    [TestClass]
    public sealed class ViesClientTest
    {
        public TestContext TestContext { get; set; }

        private ViesClient GetViesClient()
        {
            var httpClient = new HttpClient();

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock
                .Setup(_ => _.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            return new ViesClient(httpClientFactoryMock.Object);
        }

        [TestMethod]
        public async Task CheckVatAsync_ShouldThrowArgumentNullException_WhenVatNumberIsEmpty()
        {
            var client = this.GetViesClient();

            var argumentNullException = await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () =>
            {
                await client.CheckVatAsync("", TestContext.CancellationToken);
            });

            Assert.StartsWith("VAT number cannot be null.", argumentNullException.Message);
        }

        [TestMethod]
        public async Task CheckVatAsync_ShouldReturnValidResponse_WhenVatNumberIsValid()
        {
            var client = this.GetViesClient();

            var vatCheckResponse = await client.CheckVatAsync("ATU36801500", TestContext.CancellationToken);

            Assert.IsNotNull(vatCheckResponse);
            Assert.IsTrue(vatCheckResponse.Valid);
            Assert.AreEqual("U36801500", vatCheckResponse.VatNumber);
            Assert.AreEqual("Bundeshauptstadt Wien", vatCheckResponse.Name);
            Assert.AreEqual("Ebendorferstraße 2\nAT-1010 Wien", vatCheckResponse.Address);
        }

        [TestMethod]
        public async Task CheckVatAsync_ShouldReturnInvalidResponse_WhenVatNumberIsNotValid()
        {
            var client = this.GetViesClient();

            var vatCheckResponse = await client.CheckVatAsync("ATU36801501", TestContext.CancellationToken);

            Assert.IsNotNull(vatCheckResponse);
            Assert.IsFalse(vatCheckResponse.Valid);
            Assert.AreEqual("U36801501", vatCheckResponse.VatNumber);            
        }
    }
}
