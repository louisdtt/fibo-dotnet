using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/fibonacci", async () => await Fibonacci.Compute.ExecuteAsync(new[] {"44", "44", "44"}));

// app.MapGet("/", () => );

app.Run();
