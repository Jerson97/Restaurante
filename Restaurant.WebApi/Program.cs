using Restaurant.Application;
using Restaurant.Persistence;
using Restaurant.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

//Configuracion de Persistence
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

// Add services to the container.

builder.Services.AddControllers();
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
app.UseHandlerUsers();
app.MapControllers();

app.Run();
