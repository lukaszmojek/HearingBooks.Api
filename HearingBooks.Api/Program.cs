using FastEndpoints.Swagger;
using HearingBooks.Api.Core.Auth;
using HearingBooks.Api.Core.Configuration;
using HearingBooks.Common.Mapper;
using HearingBooks.Infrastructure;
using HearingBooks.MassTransit;
using HearingBooks.Persistance;
using HearingBooks.Services.Core.Storage;
using Microsoft.EntityFrameworkCore;

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
        options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseUrl"))
            .EnableSensitiveDataLogging();
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