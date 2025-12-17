using Restaurant.Application;
using Restaurant.Persistence;
using Restaurant.WebApi.Extensions;
using Restaurant.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Inyección de dependencias por capa
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddSecurity(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseHandlerUsers();

app.MapControllers();

app.Run();
