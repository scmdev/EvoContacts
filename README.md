# EvoContacts

A simple ASP.NET Core Web API for managing basic Contacts.

## Minimum requirements:

- You will need Visual Studio 2017 (15.8) and the .NET Core 2.1 SDK.
- The latest SDK and tools can be downloaded from https://dot.net/core.

## Technologies implemented:

- ASP.NET Core 2.1
- ASP.NET WebApi Core
- JWT Authentication
- Entity Framework Core 2.1
- .NET Core Native DI
- AutoMapper
- Swagger UI
- xUnit
- Moq

## Architecture:

- Full architecture with separation of concerns, SOLID and Clean Code
- Repository and Generic Repository

## Projects included in solution:

- src/**EvoContacts.API**
- src/**EvoContacts.ApplicationCore**
- src/**EvoContacts.Infrastructure**
- tests/**EvoContacts.IntegrationTests**
- tests/**EvoContacts.UnitTests**

## Steps to run the application locally:

1. Clone or download the project from GitHub.
2. Open EvoContacts.sln in Visual Studio 2017.
3. Build Solution.
4. Select StartUp Project = EvoContacts.API.
5. Select Debug Profile = IIS Express.
6. Click Start Debugging or hit F5. Application will run and display Swagger UI.
7. Use the **Auth/token** endpoint to request an OAuth authorization token.
8. Click the "Authorize" button at the top of the UI. Enter "Bearer {Token}" into the Value field (without quotations and substituting the token you just received). Click "Authorize" to save and close the dialog.
9. You should now be authorized to use all other endpoints.

Note that the application runs using an in-memory database by default. 

If you wish to use a real SQL Server database complete the following steps:

1. Uncomment **EvoContacts.API\Startup.cs line 59** ConfigureProductionServices(services);
2. Comment out **EvoContacts.API\Startup.cs line 60** ConfigureTestingServices(services);
3. Configure ConnectionString "DefaultConnection" in **EvoContacts.API\appsettings.json line 6**
4. Configure ConnectionString "DefaultConnection" in **EvoContacts.API\appsettings.Development.json line 6**
5. Open **Package Manager Console** window.
6. Select Default project = "EvoContacts.Infrastructure".
7. Type **Update-Database** into Package Manager Console and hit enter to apply migrations to the configured database.
8. Re-run the application as detailed above.

## About:

This project was developed by [Stephen Murphy](https://www.linkedin.com/in/stephen-murphy-63074816b).
