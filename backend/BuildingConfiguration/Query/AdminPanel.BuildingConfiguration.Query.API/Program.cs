using AdminPanel.BuildingConfiguration.Query.Application.Consumers;
using AdminPanel.BuildingConfiguration.Query.Application.Handlers;
using AdminPanel.BuildingConfiguration.Query.Domain.Entities;
using AdminPanel.BuildingConfiguration.Query.Domain.Repositories;
using AdminPanel.BuildingConfiguration.Query.Persistence.DataAccess;
using AdminPanel.BuildingConfiguration.Query.Persistence.Repositories;
using AdminPanel.Shared.Exceptions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<BuildingCreatedEventConsumer>();
    x.AddConsumer<BuildingRemovedEventConsumer>();
    x.AddConsumer<BuildingUpdatedEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri("amqps://b-e8a3e19e-49a5-41aa-9170-7ffacadb2865.mq.eu-north-1.amazonaws.com:5671"), h =>
        {
            h.Username("admin");
            h.Password("Burack8888.!");
        });

        cfg.ReceiveEndpoint("building-create-queue", e =>
        {
            e.ConfigureConsumer<BuildingCreatedEventConsumer>(context);
        });
        
        cfg.ReceiveEndpoint("building-remove-queue", e =>
        {
            e.ConfigureConsumer<BuildingRemovedEventConsumer>(context);
        });
        
        cfg.ReceiveEndpoint("building-update-queue", e =>
        {
            e.ConfigureConsumer<BuildingUpdatedEventConsumer>(context);
        });
    });
});
BsonClassMap.RegisterClassMap<Building>();

builder.Services.AddScoped<BuildingCreatedEventConsumer>();
builder.Services.AddScoped<BuildingRemovedEventConsumer>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));

builder.Services.AddSingleton<IMongoClient, MongoClient>(
    sp => new MongoClient(builder.Configuration.GetValue<string>("MongoDbConfig:ConnectionString")));

builder.Services.AddDbContext<BuildingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BuildingConnectionString")));

builder.Services.AddScoped<IBuildingRepository, BuildingRepository>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllBuildingsQueryHandler).Assembly));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<GlobalExceptionHandler>();

app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();