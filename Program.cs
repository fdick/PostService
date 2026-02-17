

using Microsoft.EntityFrameworkCore;
using PostService.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PostServiceDbContext>(opt => 
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString(nameof(PostServiceDbContext));
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/up", () => "it's custom page");

app.Run();
