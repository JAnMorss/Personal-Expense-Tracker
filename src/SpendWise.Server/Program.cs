using Serilog;
using SpendWise.Application;
using SpendWise.Infrastructure;
using SpendWise.Infrastructure.Seeding;
using SpendWise.Server.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

//using (var scope = app.Services.CreateScope())
//{
//    await DataSeeder.SeedAsync(scope.ServiceProvider);
//}

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseRequestContextLogging();

app.UseSerilogRequestLogging();

app.UseCustomExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
