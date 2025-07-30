# How to run

Api use simple authorization middleware, which requires api key to be provided. If You are using swagger ui use "Authorization" button.  
For tools like Postman add "x-api-key" header to http request.  
Any phrase that contains "santander" word will pass validation:

- santander - OK
- 123santander123 - OK
- Santander - OK
- 123ggg4445h - NO OK

## Hosted instance on Azure

May require couple seconds to load, after inactivity period.  
<https://vrzasqsantanderascendion.azurewebsites.net/swagger>

## Docker

   ``` 
    docker run -d -p 5000:8080 vrzasq/santanderascendion
   ```

Api will be available at <http://localhost:5000/swagger>

## Build from source

Application requires .NET 8+ to run which can be downloaded [under this link](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).  
Clone git repository and from the root directory of a solution run following script:

   ```
    dotnet restore
    dotnet run --project .\src\Services\SantanderAscendion\SantanderAscendion.Api\SantanderAscendion.Api.csproj -c Release
   ```

   Application will be available at default address <http://localhost:5176/swagger>

# Assumptions

- Code is organized in a way that would be beneficial for a larger solution, when You have more than one service
- Domain layer is missing, because there is no domain object to be implemented
- Api had to be responsive so caching was added in multiple layers

# Things to improve

- Api is using in-memory cache provider, for production it would be better to use Redis or alternative solution
- Caching could be more robust:
  - Handle edge cases, like null value, when null is a valid value
  - Add cache configuration to appsettings
- There is simple parameter validation in GetBestStoriesQueryHandler.cs, however for more complex requests validation behavior could be added using similar concept to CachingBehavior and LoggingBehavior, with usage of FluentValidation library
- For a quicker bootstrapping new services, common functionality like logging, swagger etc from Program.cs could be wrapped in separate class ex. new BootstrapAppBuilder().Build().Run();
- Logging could be configurable from appsettings for example to disable or enable logging request and response payload
- Seq or ElasticSearch could be added as log sinks
- Custom resilience strategy for httpClient instead of standard one
- Api key should be put in KeyVault / GitHub Secrets or similar solution depending on hosing platform
- Add UnitTest project