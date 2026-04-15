using Microsoft.EntityFrameworkCore;
using MorboLensAI.Repository;
using MorboLensAI.Models;
using MorboLensAI.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();


//APIs
app.MapTodoEndpoints();


//Start the project
app.Run();