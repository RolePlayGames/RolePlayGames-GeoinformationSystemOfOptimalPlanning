using GSOP.Infrastructure.DataAccess.DI;
using GSOP.Interfaces.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

#region Configurate services

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDataAccessComponents();

#endregion

var app = builder.Build();

#region Configurate app

await app.MigrateDatabase();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();
