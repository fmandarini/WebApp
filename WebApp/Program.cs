using Database;
using Microsoft.EntityFrameworkCore;
using Models.Interfaces;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IConcert, DbConcertsImpl>();
builder.Services.AddScoped<IArtist, DbArtistsImpl>();

builder.Services.AddDbContext<MusicDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MusicDB"),
        optionsBuilder => optionsBuilder.MigrationsAssembly("Database")));

// var numberElementsPerPage = builder.Configuration["NumberElementsPerPage"] != null
//     ? Convert.ToInt32(builder.Configuration["NumberElementsPerPage"])
//     : 2;

var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp =>
    exceptionHandlerApp.Run(async context =>
        await Results.Problem().ExecuteAsync(context)));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();