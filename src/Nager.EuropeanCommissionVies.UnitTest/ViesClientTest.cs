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
        [DataRow("BE0890082292", "NV BRUSSELS AIRPORT COMPANY")]
        [DataRow("BG175074752", "ПРОФИ КРЕДИТ  БЪЛГАРИЯ - ЕООД")]
        [DataRow("HR33392005961", "NASTAVNI ZAVOD ZA JAVNO ZDRAVSTVO DR. ANDRIJA ŠTA")]
        [TestMethod]
        public async Task CheckVatAsync_ValidVatNumber_ReturnsExpectedNameWithRetry(string vatNumber, string name)
        {
            var client = this.GetViesClient();

            VatCheckResponse? vatCheckResponse = null;

            for (var i = 0; i < 20; i++)
            {
                try
                {
                    vatCheckResponse = await client.CheckVatAsync(vatNumber, TestContext.CancellationToken);
                    break;
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
            Assert.AreEqual(name, vatCheckResponse.Name?.Trim());
        }

        [DataRow("ATU12345123", "Austria")]
        [DataRow("BE0123451234", "Belgium")]
        [DataRow("BG123451234", "Bulgaria")]
        [DataRow("HR12345123456", "Croatia")]
        [DataRow("CY12345123X", "Cyprus")]
        [DataRow("CZ12345123", "Czech Republic")]
        [DataRow("DK12345123", "Denmark")]
        [DataRow("EE123451234", "Estonia")]
        [DataRow("FI12345123", "Finland")]
        [DataRow("FR12345123456", "France")]
        [DataRow("DE123451234", "Germany")]
        [DataRow("EL123451234", "Greece")]
        [DataRow("HU12345123", "Hungary")]
        [DataRow("IE1234512X", "Ireland")]
        [DataRow("IT12345123456", "Italy")]
        [DataRow("LV12345123456", "Latvia")]
        [DataRow("LT123451234", "Lithuania")]
        [DataRow("LU12345123", "Luxembourg")]
        [DataRow("MT12345123", "Malta")]
        [DataRow("NL123451234B01", "Netherlands")]
        [DataRow("PL1234512345", "Poland")]
        [DataRow("PT123451234", "Portugal")]
        [DataRow("RO123451234", "Romania")]
        [DataRow("SK1234512345", "Slovakia")]
        [DataRow("SI12345123", "Slovenia")]
        [DataRow("ES12345123X", "Spain")]
        [DataRow("SE123451234563", "Sweden")]
        [TestMethod]
        public async Task CheckVatAsync_InvalidVatNumber_ReturnsValidFalse(string vatNumber, string countryName)
        {
            var client = this.GetViesClient();
            var stopwatch = new Stopwatch();

            VatCheckResponse? vatCheckResponse = null;

            for (var i = 0; i < 20; i++)
            {
                try
                {
                    stopwatch.Restart();
                    vatCheckResponse = await client.CheckVatAsync(vatNumber, TestContext.CancellationToken);
                    break;
                }
                catch (ViesException exception)
                {
                    await Task.Delay(500, TestContext.CancellationToken);
                    Trace.WriteLine($"ViesException: {exception} {exception.RawResponseBody}");
                }
                finally
                {
                    stopwatch.Stop();
                    Trace.WriteLine($"{countryName} - {stopwatch.Elapsed.TotalMilliseconds}ms");
                }
            }

            Assert.IsNotNull(vatCheckResponse);
            Assert.IsFalse(vatCheckResponse.Valid);
        }

        [DataRow("ATU15110001", "Austria")]
        [DataRow("BE0477472701", "Belgium")]
        [DataRow("BG202211464", "Bulgaria")]
        [DataRow("HR33392005961", "Croatia")]
        [DataRow("CY99000230P", "Cyprus")]
        [DataRow("CZ60193336", "Czech Republic")]
        [DataRow("DK12719280", "Denmark")]
        [DataRow("EE100000670", "Estonia")]
        [DataRow("FI20774740", "Finland")]
        [DataRow("FR10402571889", "France")]
        [DataRow("DE122268496", "Germany")]
        [DataRow("EL094259216", "Greece")]
        [DataRow("HU29312757", "Hungary")]
        [DataRow("IE6388047V", "Ireland")]
        [DataRow("IT06903461215", "Italy")]
        [DataRow("LV40003032949", "Latvia")]
        [DataRow("LT120296515", "Lithuania")]
        [DataRow("LU26375245", "Luxembourg")]
        [DataRow("MT18852233", "Malta")]
        [DataRow("NL002455799B11", "Netherlands")]
        [DataRow("PL7272445205", "Poland")]
        [DataRow("PT501613897", "Portugal")]
        [DataRow("RO14388698", "Romania")]
        [DataRow("SK2020798395", "Slovakia")]
        [DataRow("SI50223054", "Slovenia")]
        [DataRow("ES38076731R", "Spain")]
        [DataRow("SE556016068001", "Sweden")]
        [TestMethod]
        public async Task CheckVatAsync_ValidVatNumber_ReturnsValidTrue(string vatNumber, string countryName)
        {
            var client = this.GetViesClient();
            var stopwatch = new Stopwatch();

            VatCheckResponse? vatCheckResponse = null;

            for (var i = 0; i < 20; i++)
            {
                try
                {
                    stopwatch.Restart();
                    vatCheckResponse = await client.CheckVatAsync(vatNumber, TestContext.CancellationToken);
                    break;
                }
                catch (ViesException exception)
                {
                    await Task.Delay(500, TestContext.CancellationToken);
                    Trace.WriteLine($"ViesException: {exception} {exception.RawResponseBody}");
                }
                finally
                {
                    stopwatch.Stop();
                    Trace.WriteLine($"{countryName} - {vatNumber} {stopwatch.Elapsed.TotalMilliseconds}ms {vatCheckResponse?.Valid} {vatCheckResponse?.Name}");
                }
            }

            Assert.IsNotNull(vatCheckResponse);
            Assert.IsTrue(vatCheckResponse.Valid);
        }
    }
}
