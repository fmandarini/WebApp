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

// Look Dependency Injection
builder.Services.AddScoped<IConcert, DbConcertsImpl>();
builder.Services.AddScoped<IArtist, DbArtistsImpl>();


// Look migrationsAssembly
builder.Services.AddDbContext<MusicDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("MusicDB"),
        optionsBuilder => optionsBuilder.MigrationsAssembly("Database")));

// Look configuration of dependency injection
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-8.0
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

// Add default data to database
using (var scope = app.Services.CreateScope())
{
    await using var dbContext = scope.ServiceProvider.GetRequiredService<MusicDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

app.Run();