using Microsoft.EntityFrameworkCore;
using MorboLensAI;
using MorboLensAI.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();


//APIs
app.MapTodoEndpoints();


//Start the project
app.Run();