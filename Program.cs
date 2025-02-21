using System.Collections.ObjectModel;
using BookManagementAPI.Data;
using BookManagementAPI.Infrastructure;
using BookManagementAPI.Interfaces;
using BookManagementAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NLog.Web;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ✅ Define MSSQL Server sink options
var sinkOptions = new MSSqlServerSinkOptions
{
    TableName = "Logs",
    AutoCreateSqlTable = true, // ✅ Ensure Logs table is created manually
    BatchPostingLimit = 50,
    SchemaName = "dbo"
};

// ✅ Define column options
var columnOptions = new ColumnOptions
{
    AdditionalColumns = new Collection<SqlColumn>
    {
        new SqlColumn("UserName", System.Data.SqlDbType.NVarChar, true, 100),
        new SqlColumn("RequestPath", System.Data.SqlDbType.NVarChar, true, 200)
    }
};

// ✅ Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
    .WriteTo.MSSqlServer(
        connectionString: connectionString,
        sinkOptions: sinkOptions,
        columnOptions: columnOptions,
        restrictedToMinimumLevel: LogEventLevel.Warning
    )
    .CreateLogger();

Log.Information("Application Starting...");
builder.Host.UseSerilog();
builder.Host.UseNLog(); // ✅ Use both Serilog & NLog

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// ✅ Register Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Book Management API",
        Description = "An ASP.NET Core Web API for managing books.",
    });
});


var app = builder.Build();

// ✅ Ensure Swagger is used only after `AddSwaggerGen()`
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // ✅ Ensures Swagger is available
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}


// ✅ Ensure database is created and migrations are applied
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();

    try
    {
        context.Database.Migrate(); // ✅ Apply migrations before inserting seed data
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        Log.Error($"Error initializing database: {ex.Message}");
    }
}

// ✅ Middleware setup in correct order
app.UseExceptionHandler(); // ✅ Must be before controllers
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.MapControllers();

Log.Information("Application Started Successfully!");

app.Lifetime.ApplicationStopping.Register(() =>
{
    Log.Information("Application is shutting down...");
    Log.CloseAndFlush();
});

app.Run();
