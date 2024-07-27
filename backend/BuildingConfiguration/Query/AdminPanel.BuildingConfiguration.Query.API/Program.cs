using AdminPanel.BuildingConfiguration.Query.Application.Consumers;
using AdminPanel.BuildingConfiguration.Query.Persistence.DataAccess;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BuildingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BuildingConnectionString")));
    
builder.Services.AddHostedService<ConsumerHostedService>();

builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig)));


var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();