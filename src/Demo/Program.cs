using System;
using System.Diagnostics;
using System.IO;
using Fibonacci;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

Console.WriteLine(environmentName);

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddEnvironmentVariables()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
    .Build();
    
var applicationName = configuration.GetValue<string>("Application:Name");
var applicationMessage = configuration.GetValue<string>("Application:Message");

Console.WriteLine($"Application Name : {applicationName}");
Console.WriteLine($"Application Message : {applicationMessage}");

var applicationSection = configuration.GetSection("Application");
var applicationConfig = applicationSection.Get<ApplicationConfig>();

var loggerFactory = LoggerFactory.Create(builder => {
        builder.AddConsole();
    }
);

var services = new ServiceCollection();
services.AddTransient<Compute>();
services.AddLogging(configure => configure.AddConsole());
services.AddDbContext<FibonacciDataContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

using (var serviceProvider = services.BuildServiceProvider())
{
    var logger =serviceProvider.GetService<ILogger<Compute>>();
    logger.LogError($"Application Name : {applicationConfig.Name}");
    logger.LogInformation($"Application Message : {applicationConfig.Message}");


    Stopwatch stopwatch = new();
    stopwatch.Start();
    var compute = serviceProvider.GetService<Compute>();
    var tasks = await compute.ExecuteAsync(args);
    foreach (var task in tasks) Console.WriteLine($"Fibo result : {task}");
    stopwatch.Stop();
    Console.WriteLine($"{stopwatch.Elapsed.Seconds}s");
}

public class ApplicationConfig
{
    public string Name { get; set; }
    public string Message { get; set; }
}