# Azure Function .NET base

## Azure function structure

- host.setting.json contains environment variables
- IConfiguration is setting with following priority descendent:
    - Read environment variables
    - Read appsetings.{environmentName}.json
    - Read appsettings.json

- TimerTrigger using CronExpression: https://crontab.cronhub.io/

## EFCore usage:

Run command in powershell in solution root folder.

- Add migration:

  ```
  dotnet ef migrations add InitialCreate `
    -p ./src/Core.Database `
    -s ./src/WebAPI.Infra `
    -c AppDbContext
  ```

- Update database
  ```
  dotnet ef databae update `
    -p ./src/Core.Database `
    -s ./src/WebAPI.Infra `
    -c AppDbContext
  ```
