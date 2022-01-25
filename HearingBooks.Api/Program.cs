using System.Reflection;
using HearingBooks.Api.Auth;
using HearingBooks.Api.Configuration;
using HearingBooks.Api.Languages;
using HearingBooks.Api.Seed;
using HearingBooks.Api.Speech;
using HearingBooks.Api.Storage;
using HearingBooks.Api.Syntheses;
using HearingBooks.Infrastructure.Repositories;
using HearingBooks.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<IApiConfiguration, ApiConfiguration>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo {Title = "HearingBooks.Api", Version = "v1"});
                
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
            In = ParameterLocation.Header, 
            Description = "Please insert JWT token with Bearer into field",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey 
        });
                
        c.AddSecurityRequirement(new OpenApiSecurityRequirement {
            { 
                new OpenApiSecurityScheme 
                { 
                    Reference = new OpenApiReference 
                    { 
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer" 
                    } 
                },
                new string[] { } 
            } 
        });

        // Set the comments path for the Swagger JSON and UI.
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    }
);

#if DEBUG
builder.Services.AddDbContext<HearingBooksDbContext>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseUrl"))
            .EnableSensitiveDataLogging();
    });
#endif

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.AddScoped<ISpeechService, SpeechService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<TextSynthesisService, TextSynthesisService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILanguageRepository, LanguageRepository>();
builder.Services.AddScoped<ITextSynthesisRepository, TextSynthesisRepository>();

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json",
        $"{builder.Environment.ApplicationName} v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(x => 
    x.AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials()
);

app.UseMiddleware<JwtMiddleware>();

//TODO: Move that to middleware
// app.Use(async (ctx, next) =>
// {
//     try
//     {
//         await next();
//     }
//     catch(BadHttpRequestException ex)
//     {
//         ctx.Response.StatusCode = ex.StatusCode;
//         await ctx.Response.WriteAsync(ex.Message);
//     }
// });

app.MapAuthEndpoints();
app.MapSynthesesEndpoints();
app.MapLanguagesEndpoints();
app.MapSeedEndpoints();

app.Run();