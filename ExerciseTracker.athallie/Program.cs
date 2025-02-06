using System.Diagnostics;
using ExerciseTracker.athallie.Model;
using ExerciseTracker.athallie.Models;
using ExerciseTracker.athallie.Repositories;
using ExerciseTracker.athallie.Services;
using ExerciseTracker.athallie.UI;
using ExerciseTracker.athallie.Utils;
using Microsoft.EntityFrameworkCore;

 /*API Setup*/
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ExerciseTrackerContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);
builder.Services.AddScoped<IRepository<Exercise>, Repository<Exercise>>();
builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging
    .AddDebug()
    .AddEventSourceLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

/*UI Setup*/
HttpClient httpClient = new HttpClient();
HttpUtils httpUtils = new HttpUtils(httpClient);

Parallel.Invoke(
    () => app.Run(),
    () =>
    {
        while(app.Urls.Count <= 0)
        {
            Thread.Sleep(1000);
        }
        Console.WriteLine("\n");
        httpUtils.ApiEndpoint = $"{app.Urls.First()}/api/Exercises";
        IConsoleUI ui = new ConsoleUI(httpUtils, new UserInput());
        ui.Run();
    }
);