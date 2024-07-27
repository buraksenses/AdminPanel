using AdminPanel.BuildingConfiguration.Command.Application.Handlers;
using AdminPanel.BuildingConfiguration.Command.Application.Validations;
using AdminPanel.BuildingConfiguration.Command.Domain.Aggregates;
using AdminPanel.BuildingConfiguration.Command.Persistence.Handlers;
using AdminPanel.BuildingConfiguration.Command.Persistence.Producers;
using AdminPanel.BuildingConfiguration.Command.Persistence.Repositories;
using AdminPanel.BuildingConfiguration.Command.Persistence.Stores;
using AdminPanel.Shared.Exceptions;
using Confluent.Kafka;
using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using EcoVerse.StockManagement.Command.Infrastructure.Config;
using FluentValidation;
using FluentValidation.AspNetCore;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

BsonClassMap.RegisterClassMap<BaseEvent>();
BsonClassMap.RegisterClassMap<BuildingCreatedEvent>();
BsonClassMap.RegisterClassMap<BuildingRemovedEvent>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));
builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)));

builder.Services.AddSingleton<IMongoClient, MongoClient>(
    sp => new MongoClient(builder.Configuration.GetValue<string>("MongoDbSettings:ConnectionString")));


builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
builder.Services.AddScoped<IEventProducer, EventProducer>();
builder.Services.AddScoped<IEventStore, EventStore>();
builder.Services.AddScoped<IEventSourcingHandler<BuildingAggregate>, EventSourcingHandler>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddBuildingCommandHandler).Assembly));

builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<AddBuildingValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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