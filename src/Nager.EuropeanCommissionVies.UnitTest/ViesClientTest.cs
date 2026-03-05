using Moq;
using Nager.EuropeanCommissionVies.Models;
using System.Diagnostics;

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

        [DataRow("DE259582878", "---")] //Bundeszentralamt für Steuern
        [DataRow("ATU37866403", "Bundesministerium für Finanzen")]
        [DataRow("IT06363391001", "AGENZIA DELLE ENTRATE")]
        [DataRow("FR91110020013", "MINISTERE DE L'ECONOMIE, DES FINANCE")]
        [DataRow("ESA28015865", "---")] //Telefónica Spain
        [TestMethod]
        public async Task CheckVatAsync_ValidVatNumber_ReturnsExpectedNameWithRetry(string vatNumber, string name)
        {
            var client = this.GetViesClient();

            VatCheckResponse vatCheckResponse = null;

            for (var i = 0; i < 10; i++)
            {
                try
                {
                    vatCheckResponse = await client.CheckVatAsync(vatNumber, TestContext.CancellationToken);
                    break;
                }
                catch (ViesException exception)
                {
                    await Task.Delay(300);
                    Trace.WriteLine($"ViesException: {exception} {exception.RawResponseBody}");
                }                
            }

            Assert.IsNotNull(vatCheckResponse);
            Assert.IsTrue(vatCheckResponse.Valid);
            Assert.AreEqual(vatNumber.Substring(2), vatCheckResponse.VatNumber);
            Assert.AreEqual(name, vatCheckResponse.Name?.Trim());
        }
    }
}
