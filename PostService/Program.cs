using MassTransit;
using Microsoft.EntityFrameworkCore;
using PostService.API.GRPC;
using PostService.Application.Services;
using PostService.Core.Abstractions;
using PostService.DataAccess;
using PostService.DataAccess.Repositories;
using PostService.RabbitMQ.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();

builder.Services.AddDbContext<PostServiceDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString(nameof(PostServiceDbContext)));
});

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<UsersConsumer>();
    busConfigurator.AddConsumer<ThreadsConsumer>();

    busConfigurator.UsingRabbitMq((ctx, configurator) =>
    {
        configurator.Host(new Uri(builder.Configuration["MessageBroker:Host"]), h =>
        {
            h.Username(builder.Configuration["MessageBroker:Username"]);
            h.Password(builder.Configuration["MessageBroker:Password"]);
        });

        configurator.ConfigureEndpoints(ctx);
    });
});

builder.Services.AddScoped<IMessagesService, MessagesService>();
builder.Services.AddScoped<IMessagesRepository, PostsRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UsersService>();
builder.Services.AddScoped<IThreadService, ThreadsService>();
builder.Services.AddScoped<IThreadRepository, ThreadRepository>();


// Настройка Kestrel для HTTP/2 для gRPC
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5001, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
    });

    options.ListenLocalhost(5002, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseGrpcWeb();

app.UseCors();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<GRPPostsController>();
});

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
