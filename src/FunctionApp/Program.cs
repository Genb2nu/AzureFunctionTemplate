using Business;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration((builder, configurationBuilder) =>
    {
        configurationBuilder.SetBasePath(builder.HostingEnvironment.ContentRootPath);
        configurationBuilder.AddJsonFile("appsettings.json", 
            optional: true, 
            reloadOnChange: true);
        configurationBuilder.AddJsonFile($"appsettings.{builder.HostingEnvironment.EnvironmentName}.json",
            optional: true,
            reloadOnChange: true);
        configurationBuilder.AddEnvironmentVariables();
        configurationBuilder.Build();
    })
    .ConfigureServices((context, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        BusinessServiceRegister.Register(services, context.Configuration);
    })
    .Build();

host.Run();
