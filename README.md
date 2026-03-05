# Nager.EuropeanCommissionVies

A lightweight .NET client for validating European VAT numbers using the official European Commission VIES (VAT Information Exchange System) API.

This library provides simple and strongly typed access to VAT validation including optional qualified checks with trader information.

## ✨ Features

- Validate VAT numbers via VIES
- Strongly typed request/response models
- Support for qualified VAT checks (trader name, address, company type)
- Async/await support
- CancellationToken support
- Clean and minimal API surface
- Exception-safe argument validation

## 📦 Installation

Install via [NuGet](https://www.nuget.org/packages/Nager.EuropeanCommissionVies):

```bash
dotnet add package Nager.EuropeanCommissionVies
````

Or via Package Manager:

```powershell
Install-Package Nager.EuropeanCommissionVies
```

## 🚀 Usage

### Basic VAT validation

```csharp
var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

var viesClient = new ViesClient(httpClientFactory);

var response = await viesClient.CheckVatAsync("DE123456789");

if (response != null && response.Valid)
{
    Console.WriteLine("VAT number is valid");
    Console.WriteLine(response.Name);
    Console.WriteLine(response.Address);
}
```

### Check if VAT number is valid (boolean shortcut)

```csharp
var isValid = await viesClient.IsValidVatAsync("DE123456789");

if (isValid)
{
    Console.WriteLine("Valid VAT number");
}
```

### Qualified VAT validation (with trader details)

```csharp
var request = new VatCheckRequest
{
    VatNumber = "123456789",
    CountryCode = "DE",
    TraderName = "Example Company",
    TraderStreet = "Example Street 1",
    TraderPostalCode = "10115",
    TraderCity = "Berlin"
};

var response = await viesClient.CheckVatAsync(request);

if (response != null)
{
    Console.WriteLine($"Valid: {response.Valid}");
    Console.WriteLine($"Name match: {response.Name}");
}
```

## Error Codes

| Code                   | Description                                                                                                      |
|------------------------|------------------------------------------------------------------------------------------------------------------|
| MS_MAX_CONCURRENT_REQ  | The maximum number of concurrent requests is not linked to a specific IP. It is a global count across all users. |

## Vat number structure

| CountryCode | Example      | Abbreviation                 |
|-------------|--------------|------------------------------|
| AT          | ATU9999999p  | UID                          |
| BE          | BE999999999p | N° TVA / BTW-nr. / MwSt.-Nr. |
| BG          | BG99999999p  | DDS (ДДС)                    |
| CY          | CY50999999L  | FPA                          |
| CZ          | CZ901231999p | DIČ                          |
| DE          | DE99999999p  | USt-IdNr.                    |
| DK          | DK99999999   | SE-Nr.                       |

https://en.wikipedia.org/wiki/VAT_identification_number

## 🌍 About VIES

The VAT Information Exchange System (VIES) is operated by the European Commission and allows validation of VAT numbers registered within the European Union.

Official information:
[https://ec.europa.eu/taxation_customs/vies/](https://ec.europa.eu/taxation_customs/vies/)

## 🛠 Requirements

* .NET 10 or later
* Internet connection for VIES API access

## 📄 License

This project is licensed under the MIT License.

## 🤝 Contributing

Contributions are welcome.
Please open an issue or submit a pull request.
