using Microsoft.EntityFrameworkCore;
using PostService.Application.Services;
using PostService.Core.Abstractions;
using PostService.DataAccess;
using PostService.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PostServiceDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString(nameof(PostServiceDbContext)));
});

builder.Services.AddScoped<IMessagesService, MessagesService>();
builder.Services.AddScoped<IMessagesRepository, MessagesRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.MapGet("/", () => "Hello World!");
//app.MapGet("/up", () => "it's custom page");

app.Run();
