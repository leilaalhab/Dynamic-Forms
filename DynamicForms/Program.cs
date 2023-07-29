global using DynamicForms.Models;
global using DynamicForms.Dtos.Choice;
global using DynamicForms.Dtos.Step;
global using DynamicForms.Dtos.Input;
global using DynamicForms.Dtos.Form;
global using DynamicForms.Data;

global using AutoMapper;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;


using DynamicForms.Services.ChoiceService;
using DynamicForms.Services.FormService;
using DynamicForms.Services.InputService;
using DynamicForms.Services.StepService;

using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpContextAccessor();


builder.Services.AddScoped<IChoiceService, ChoiceService>();
builder.Services.AddScoped<IInputService, InputService>();
builder.Services.AddScoped<IFormService, FormService>();
builder.Services.AddScoped<IStepService, StepService>();

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

app.Run();

