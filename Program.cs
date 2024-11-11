using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using MoviesAPi.EndPoints;
using MoviesAPi.Entities;
using MoviesAPi.Repository;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
// );


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(configuration =>
    {
        configuration.WithOrigins(builder.Configuration["AllowedOrigins"]!)
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
    options.AddPolicy("free", configuration =>
    {
        configuration.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddOutputCache();
builder.Services.AddScoped<IGenresRepository, GenresRepository>();

var app = builder.Build();


app.UseCors();
app.UseOutputCache();
app.UseSwagger();
app.UseSwaggerUI();


app.MapGet("/", () => "سلام فرزندم");

app.MapGroup("/genres").MapGenres();



app.Run();



