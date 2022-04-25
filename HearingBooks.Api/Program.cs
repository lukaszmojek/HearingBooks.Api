global using FastEndpoints;
global using FastEndpoints.Security;

using FastEndpoints.Swagger;
using HearingBooks.Api.Auth;
using HearingBooks.Api.Configuration;
using HearingBooks.Api.Speech;
using HearingBooks.Api.Storage;
using HearingBooks.Api.Syntheses.TextSyntheses;
using HearingBooks.Infrastructure.Repositories;
using HearingBooks.Persistance;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<IApiConfiguration, ApiConfiguration>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddFastEndpoints();
builder.Services.AddAuthenticationJWTBearer(builder.Configuration.GetSection("Authorization")["Secret"]);

// builder.Services.AddAuthorization(o =>
//     o.AddPolicy("HearingBooks", b =>
//         b.RequireRole("HearingBooks")));

builder.Services.AddSwaggerDoc(settings =>
{
    settings.Title = "HearingBooks.Api";
    settings.Version = "v1";
});
// builder.Services.AddSwaggerGen(
//     c =>
//     {
//         c.SwaggerDoc("v1", new OpenApiInfo {Title = "HearingBooks.Api", Version = "v1"});
//                 
//         c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
//             In = ParameterLocation.Header, 
//             Description = "Please insert JWT token with Bearer into field",
//             Name = "Authorization",
//             Type = SecuritySchemeType.ApiKey 
//         });
//                 
//         c.AddSecurityRequirement(new OpenApiSecurityRequirement {
//             { 
//                 new OpenApiSecurityScheme 
//                 { 
//                     Reference = new OpenApiReference 
//                     { 
//                         Type = ReferenceType.SecurityScheme,
//                         Id = "Bearer" 
//                     } 
//                 },
//                 new string[] { } 
//             } 
//         });
//
//         // Set the comments path for the Swagger JSON and UI.
//         var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//         var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
//         c.IncludeXmlComments(xmlPath);
//     }
// );

builder.Services.AddDbContext<HearingBooksDbContext>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseUrl"))
            .EnableSensitiveDataLogging();
    });

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

app.Run();