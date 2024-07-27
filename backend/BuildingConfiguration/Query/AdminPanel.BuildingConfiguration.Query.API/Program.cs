using AdminPanel.BuildingConfiguration.Query.Application.Consumers;
using AdminPanel.BuildingConfiguration.Query.Application.Handlers;
using AdminPanel.BuildingConfiguration.Query.Domain.Repositories;
using AdminPanel.BuildingConfiguration.Query.Persistence.DataAccess;
using AdminPanel.BuildingConfiguration.Query.Persistence.Repositories;
using AdminPanel.Shared.Exceptions;
using Confluent.Kafka;
using CQRS.Core.Consumers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BuildingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BuildingConnectionString")));
    

builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig)));
builder.Services.AddScoped<IEventConsumer, EventConsumer>();
builder.Services.AddScoped<IBuildingRepository, BuildingRepository>();

builder.Services.AddHostedService<ConsumerHostedService>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(BuildingRemovedNotificationHandler).Assembly));

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandler>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();