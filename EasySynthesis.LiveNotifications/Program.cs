using EasySynthesis.LiveNotifications;
using EasySynthesis.LiveNotifications.Hubs;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    // elided...
    x.SetKebabCaseEndpointNameFormatter();
				
    x.UsingRabbitMq((context,cfg) =>
    {
        cfg.Host("localhost", "/", h => {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
			
    x.AddConsumers(typeof(Worker).Assembly);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
        .WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .SetIsOriginAllowed((x) => true));
});

builder.Services
    .AddSignalR()
    .AddAzureSignalR();

builder.Services.AddHostedService<Worker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("CorsPolicy");
app.UseFileServer();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<TextSynthesesHub>("/syntheses");
});

app.Run();
