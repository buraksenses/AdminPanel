using AdminPanel.BuildingConfiguration.Query.Application.Consumers;
using AdminPanel.BuildingConfiguration.Query.Application.Handlers;
using AdminPanel.BuildingConfiguration.Query.Domain.Repositories;
using AdminPanel.BuildingConfiguration.Query.Persistence.DataAccess;
using AdminPanel.BuildingConfiguration.Query.Persistence.Repositories;
using AdminPanel.Shared.Exceptions;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<BuildingCreatedEventConsumer>();
    x.AddConsumer<BuildingRemovedEventConsumer>();
    x.AddConsumer<BuildingUpdatedEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
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

builder.Services.AddScoped<BuildingCreatedEventConsumer>();
builder.Services.AddScoped<BuildingRemovedEventConsumer>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandler>();

app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();