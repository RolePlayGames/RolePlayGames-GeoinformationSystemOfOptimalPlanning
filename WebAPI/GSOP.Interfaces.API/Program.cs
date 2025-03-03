using GSOP.Application.DI;
using GSOP.Domain.DI;
using GSOP.Infrastructure.DataAccess.DI;
using GSOP.Infrastructure.Excel.DI;
using GSOP.Interfaces.API.Extensions;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

#region Configure services

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDomainServices();
builder.Services.AddApplicationServices();
builder.Services.AddDataAccessComponents();
builder.Services.AddExcelComponents();
builder.Services.AddWebApiComponents();

#endregion

var app = builder.Build();

#region Configure app

await app.MigrateDatabase();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseUiStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToController("GetReactStaticFileHtml", "Home");

#endregion

app.Run();
