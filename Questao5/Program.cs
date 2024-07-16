using Dapper;
using MediatR;
using Microsoft.Data.Sqlite;
using Questao5.Application.Services;
using Questao5.Configurations;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces;
using Questao5.Domain.Language;
using Questao5.Infrastructure.Database.Repository;
using Questao5.Infrastructure.Sqlite;
using System.Data;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

// sqlite
builder.Services.AddSingleton(new DatabaseConfig { Name = builder.Configuration.GetValue<string>("DatabaseName", "Data Source=database.sqlite") });
builder.Services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();
builder.Services.AddTransient<IContaCorrenteService, ContaCorrenteService>();
builder.Configuration.AddJsonFile("mensagenserro.json", optional: true, reloadOnChange: true);

var errorMessagesConfig = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("mensagenserro.json").Build();


builder.Services.AddSingleton<Dictionary<string, string>>(errorMessagesConfig.GetChildren().ToDictionary(x => x.Key, x => x.Value));
builder.Services.AddScoped<IContaCorrenteRepository, ContaCorrenteRepository>();
builder.Services.AddScoped<IMovimentoRepository, MovimentoRepository>();
builder.Services.AddScoped<IIdempotenciaRepository, IdempotenciaRepository>();

// AutoMapper Settings
builder.Services.AddAutoMapperConfiguration();

//Dapper
builder.Services.AddSingleton<IDbConnection>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    var connection = new SqliteConnection(connectionString);
    connection.Open();
    return connection;
});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// sqlite
#pragma warning disable CS8602 // Dereference of a possibly null reference.
app.Services.GetService<IDatabaseBootstrap>().Setup();
#pragma warning restore CS8602 // Dereference of a possibly null reference.


app.Run();

// Informações úteis:
// Tipos do Sqlite - https://www.sqlite.org/datatype3.html