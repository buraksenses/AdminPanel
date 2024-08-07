using System.Text;
using AdminPanel.BuildingConfiguration.Command.Application.Handlers;
using AdminPanel.BuildingConfiguration.Command.Application.Validations;
using AdminPanel.BuildingConfiguration.Command.Domain.Aggregates;
using AdminPanel.BuildingConfiguration.Command.Persistence.Config;
using AdminPanel.BuildingConfiguration.Command.Persistence.Handlers;
using AdminPanel.BuildingConfiguration.Command.Persistence.Repositories;
using AdminPanel.BuildingConfiguration.Command.Persistence.Stores;
using AdminPanel.Shared.Exceptions;
using CQRS.Core.Domain;
using CQRS.Core.Domain.Enums;
using CQRS.Core.Events;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri("amqps://b-e8a3e19e-49a5-41aa-9170-7ffacadb2865.mq.eu-north-1.amazonaws.com:5671"), h =>
        {
            h.Username("admin");
            h.Password("Burack8888.!");
        });
    });
});

BsonClassMap.RegisterClassMap<BaseEvent>();
BsonClassMap.RegisterClassMap<BuildingCreatedEvent>(cm =>
{
    cm.AutoMap();
    cm.SetIgnoreExtraElements(true);
    cm.GetMemberMap(c => c.BuildingType).SetSerializer(new EnumSerializer<BuildingType>(BsonType.String));
});

BsonClassMap.RegisterClassMap<BuildingUpdatedEvent>(cm =>
{
    cm.AutoMap();
    cm.SetIgnoreExtraElements(true);
});

BsonClassMap.RegisterClassMap<BuildingRemovedEvent>(cm =>
{
    cm.AutoMap();
    cm.SetIgnoreExtraElements(true);
});

BsonSerializer.RegisterSerializer(typeof(BaseEvent), new EventDataSerializer());

//BsonSerializer.RegisterSerializer(typeof(BaseEvent), new EventDataSerializer());


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// if (builder.Environment.IsDevelopment())
// {
//     builder.Configuration.AddUserSecrets<Program>();
// }

var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    });

builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));

builder.Services.AddSingleton<IMongoClient, MongoClient>(
    sp => new MongoClient(builder.Configuration.GetValue<string>("MongoDbConfig:ConnectionString")));


builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
builder.Services.AddScoped<IEventStore, EventStore>();
builder.Services.AddScoped<IEventSourcingHandler<BuildingAggregate>, EventSourcingHandler>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddBuildingCommandHandler).Assembly));

builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<AddBuildingValidator>();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();