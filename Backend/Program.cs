using Backend.Models;
using Backend.Repository;
using Backend.Services;
using Backend.Services.Hub;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(
              options =>options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")),ServiceLifetime.Transient);
builder.Services.AddTransient<TimerRepository>();
builder.Services.AddTransient<TimerService>();
builder.Services.AddHostedService<TimerConsiumer>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddCors();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials());
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapHub<TimerHub>("timer");
app.MapControllers();

app.Run();
