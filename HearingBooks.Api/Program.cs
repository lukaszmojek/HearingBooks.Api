using FastEndpoints.Swagger;
using HearingBooks.Api.Core.Auth;
using HearingBooks.Api.Core.Configuration;
using HearingBooks.Common.Mapper;
using HearingBooks.Infrastructure;
using HearingBooks.MassTransit;
using HearingBooks.Persistance;
using HearingBooks.Services.Core.Storage;
using Marten;
using Microsoft.EntityFrameworkCore;
using Weasel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<IApiConfiguration, ApiConfiguration>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddFastEndpoints();
builder.Services.AddJWTBearerAuth(builder.Configuration.GetSection("Authorization")["Secret"]);

builder.Services.AddSwaggerDoc(settings =>
{
    settings.Title = "HearingBooks.Api";
    settings.Version = "v1";
});

builder.Services.AddHearingBooksMassTransit();

builder.Services.AddDbContext<HearingBooksDbContext>(
    options =>
    {
        var databaseUrl = builder.Configuration.GetConnectionString("DatabaseUrl");
        options.UseNpgsql(databaseUrl);

        if (builder.Environment.IsDevelopment())
        {
            options.EnableSensitiveDataLogging();
        }
    });

// This is the absolute, simplest way to integrate Marten into your
// .Net Core application with Marten's default configuration
builder.Services.AddMarten(options =>
{
    // Establish the connection string to your Marten database
    var martenUrl = builder.Configuration.GetConnectionString("MartenUrl");
    options.Connection(martenUrl);

    // If we're running in development mode, let Marten just take care
    // of all necessary schema building and patching behind the scenes
    if (builder.Environment.IsDevelopment())
    {
        options.AutoCreateSchemaObjects = AutoCreate.All;
    }
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<ISynthesisPricingService, SynthesisPricingService>();

builder.Services.RegisterRepositories();

builder.Services.AddAutoMapper(typeof(TextSynthesisProfile));

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<JwtMiddleware>();

//TODO: Configure CORS properly
app.UseCors(x => 
    x.AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(origin => true)
        .AllowCredentials()
);

app.UseFastEndpoints();
app.UseOpenApi();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Test"))
{
    app.UseDeveloperExceptionPage();
    app.UseSwaggerUi3(c => c.ConfigureDefaults()); 
}

app.Run();