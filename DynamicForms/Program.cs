global using DynamicForms.Models;
global using DynamicForms.Dtos.Choice;
global using DynamicForms.Dtos.Step;
global using DynamicForms.Dtos.Input;
global using DynamicForms.Dtos.Form;
global using DynamicForms.Dtos.Formula;
global using DynamicForms.Data;

global using AutoMapper;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;

using DynamicForms.Services;
using DynamicForms.Services.ChoiceService;
using DynamicForms.Services.FormService;
using DynamicForms.Services.InputService;
using DynamicForms.Services.StepService;
using DynamicForms.Services.FormulaService;

using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    // Setup a HTTP/2 endpoint without TLS.
    options.ListenLocalhost(5256, o => o.Protocols =
        HttpProtocols.Http2);
});

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.Configure<DynamicFormsDatabaseSettings>(
    builder.Configuration.GetSection("DynamicFormsMDB"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IFormulaService, FormulaService>();
builder.Services.AddScoped<IChoiceService, ChoiceService>();
builder.Services.AddScoped<IInputService, InputService>();
builder.Services.AddScoped<IFormService, FormService>();
builder.Services.AddScoped<IStepService, StepService>();
// put a consolewrite in the constructor
// see when the grpc is being created

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.MapGrpcService<HandleFormService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

//app.UseHttpsRedirection();
//app.UseAuthorization();

app.MapControllers();

app.Run();

