﻿global using DynamicForms.Models;
global using DynamicForms.Dtos.Choice;
global using DynamicForms.Dtos.Step;
global using DynamicForms.Dtos.Input;
global using DynamicForms.Dtos.Form;
global using DynamicForms.Dtos.Formula;
global using DynamicForms.Dtos.Answer;
global using DynamicForms.Data;

global using AutoMapper;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;

using DynamicForms.Hubs;
using DynamicForms.Services.ChoiceService;
using DynamicForms.Services.FormService;
using DynamicForms.Services.InputService;
using DynamicForms.Services.StepService;
using DynamicForms.Services.FormulaService;
using DynamicForms.Services.ConditionService;
using DynamicForms.Services.HandleFormService;
using DynamicForms.Services.HandleFormulaService;
using DynamicForms.Services.AnswerService;
using DynamicForms.Services.ProgressService;

var builder = WebApplication.CreateBuilder(args);
// builder.WebHost.ConfigureKestrel(options =>
// {
//     // Setup a HTTP/2 endpoint without TLS.
//     options.ListenLocalhost(5256, o => o.Protocols =
//         HttpProtocols.Http2);
// });

//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
// builder.Services.AddHttpContextAccessor();

builder.Services.Configure<DynamicFormsDatabaseSettings>(
    builder.Configuration.GetSection("DynamicFormsMDB"));
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IFormulaService, FormulaService>();
builder.Services.AddScoped<IChoiceService, ChoiceService>();
builder.Services.AddScoped<IInputService, InputService>();
builder.Services.AddScoped<IFormService, FormService>();
builder.Services.AddScoped<IStepService, StepService>();
builder.Services.AddScoped<IConditionService, ConditionService>();
builder.Services.AddScoped<IAnswerService, AnswerService>();
builder.Services.AddScoped<IProgressService, ProgressService>();

builder.Services.AddScoped<IHandleFormService, HandleFormService>();
builder.Services.AddScoped<IHandleFormulaService, HandleFormulaService>();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();
//app.UseAuthorization();
//app.MapControllers();

builder.Services.AddCors();
builder.Services.AddSignalR();

var app = builder.Build();
app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true,
    DefaultContentType = "text/plain"
});

app.UseCors(builder => builder
.WithOrigins("null")
.AllowAnyHeader()
.AllowAnyMethod()
.AllowCredentials());
app.MapHub<FormHub>("/FillForm");

app.MapGet("/", () => "Hello World!");
app.Run();