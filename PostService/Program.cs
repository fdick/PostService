using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PostService.API.GRPC;
using PostService.Application.Services;
using PostService.Core.Abstractions;
using PostService.DataAccess;
using PostService.DataAccess.Repositories;
using PostService.RabbitMQ.Consumers;
using System.Security.Claims;
using System.Text.Json;

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

builder.Services.AddScoped<IPostsService, PostsService>();
builder.Services.AddScoped<IPostsRepository, PostsRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UsersService>();
builder.Services.AddScoped<IThreadService, ThreadsService>();
builder.Services.AddScoped<IThreadRepository, ThreadRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("all", policy =>
    {
        policy.AllowAnyOrigin()  // Разрешить любые источники
              .AllowAnyMethod()   // Разрешить любые HTTP методы (GET, POST, PUT, DELETE и т.д.)
              .AllowAnyHeader();   // Разрешить любые заголовки
    });
});


var authServerUrl = "http://localhost:8080";
var realm = "main";
var clientId = "post-service-client";

var realmUrl = $"{authServerUrl}/realms/{realm}";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = realmUrl;
        options.Audience = clientId;
        options.RequireHttpsMetadata = false; // Только для разработки!

        // ВАЖНО: Не нужен симметричный ключ!
        // Библиотека автоматически получит публичные ключи от Keycloak

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = realmUrl,
            ValidateAudience = true,
            ValidAudience = clientId,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(2)
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                // ПОЛУЧАЕМ resource_access
                var resourceAccess = context.Principal?.FindFirst("resource_access")?.Value;
                if (string.IsNullOrEmpty(resourceAccess))
                    return Task.CompletedTask;

                try
                {
                    using var doc = JsonDocument.Parse(resourceAccess);
                    var identity = context.Principal.Identity as ClaimsIdentity;

                    // Проходим по ВСЕМ клиентам в resource_access
                    foreach (var client in doc.RootElement.EnumerateObject())
                    {
                        var clientName = client.Name;

                        // Получаем роли для этого клиента
                        if (client.Value.TryGetProperty("roles", out var roles))
                        {
                            foreach (var role in roles.EnumerateArray())
                            {
                                var roleValue = role.GetString();
                                if (!string.IsNullOrEmpty(roleValue))
                                {
                                    // Добавляем роль в формате: "client_name.role_name"
                                    identity?.AddClaim(new Claim(ClaimTypes.Role, $"{clientName}.{roleValue}"));

                                    // Также добавляем просто название роли (если нужно)
                                    identity?.AddClaim(new Claim(ClaimTypes.Role, roleValue));

                                    Console.WriteLine($"Added role: {clientName}.{roleValue}");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing roles: {ex.Message}");
                }

                return Task.CompletedTask;
            }
        };
    });

// Настройка Kestrel для HTTP/2 для gRPC
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5001, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
        //listenOptions.UseHttps();
    });

    options.ListenLocalhost(5002, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
        //listenOptions.UseHttps();
    });
});

//builder.WebHost.UseUrls("http://*:5001", "http://*:5002");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PostServiceDbContext>();

    // Это создаст базу данных и таблицы, если их нет
    //dbContext.Database.EnsureCreated();
    // Или используйте миграции (рекомендуется)
    dbContext.Database.Migrate();
}

app.UseRouting();

app.UseGrpcWeb();

app.UseCors("all");

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<GRPPostsController>();
});

//app.UseHttpsRedirection();



app.MapControllers();

app.Run();
