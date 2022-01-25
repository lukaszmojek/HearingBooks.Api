using HearingBooks.Domain.Entities;
using HearingBooks.Domain.ValueObjects.User;
using HearingBooks.Persistance;
using Microsoft.AspNetCore.Mvc;

namespace HearingBooks.Api.Seed;

public static class SeedEndpointExtensions
{
    private static readonly string _baseEndpointGroupRoute = "seed";

    public static void MapSeedEndpoints(this WebApplication app)
    {
        app.MapGet(
            $"/{_baseEndpointGroupRoute}/users",
            async ([FromServices] HearingBooksDbContext context) =>
            {
                var users = new List<User>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Łukasz",
                        LastName = "Mojek",
                        UserName = "shaggy",
                        Email = "lukasz@hb.com",
                        Password = "zaq123", 
                        EmailNotificationsEnabled = true,
                        EmailIsUsername = false,
                        IsActive = true,
                        Type = UserType.HearingBooks,
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Łukasz",
                        LastName = "Mojek",
                        UserName = "user",
                        Email = "user@email.com",
                        Password = "zaq123", 
                        EmailNotificationsEnabled = true,
                        EmailIsUsername = true,
                        IsActive = true,
                        Type = UserType.PayAsYouGo,
                    }
                };

                var usersToDelete = context.Users
                    .AsEnumerable()
                    .Where(entity => users.Any(user => user.Email == entity.Email));
                
                context.Users.RemoveRange(usersToDelete);
                await context.SaveChangesAsync();
                
                await context.Users.AddRangeAsync(users);
                await context.SaveChangesAsync();
                
                return Results.Ok();
            });//.RequireAuthorization("hearing-books");
        
        app.MapGet(
            $"/{_baseEndpointGroupRoute}/languages-and-voices",
            async ([FromServices] HearingBooksDbContext context) =>
            {
                var enVoices = new[]
                {
                    new Voice
                    {
                        Id = Guid.NewGuid(),
                        Name = "pl-PL-AgnieszkaNeural",
                        DisplayName = "Amber",
                        Type = VoiceType.Female,
                        IsMultilingual = false
                    },
                    new Voice
                    {
                        Id = Guid.NewGuid(),
                        Name = "pl-PL-ZofiaNeural",
                        DisplayName = "Zofia",
                        Type = VoiceType.Female,
                        IsMultilingual = false
                    },
                    new Voice
                    {
                        Id = Guid.NewGuid(),
                        Name = "pl-PL-MarekNeural",
                        DisplayName = "Marek",
                        Type = VoiceType.Male,
                        IsMultilingual = false
                    },
                };

                var plVoices = new[]
                {
                    new Voice
                    {
                        Id = Guid.NewGuid(),
                        Name = "en-US-AmberNeural",
                        DisplayName = "Amber",
                        Type = VoiceType.Female,
                        IsMultilingual = false
                    },
                    new Voice
                    {
                        Id = Guid.NewGuid(),
                        Name = "en-US-AriaNeural",
                        DisplayName = "Aria",
                        Type = VoiceType.Female,
                        IsMultilingual = false
                    },
                    new Voice
                    {
                        Id = Guid.NewGuid(),
                        Name = "en-US-JennyMultilingualNeural",
                        DisplayName = "Jenny",
                        Type = VoiceType.Female,
                        IsMultilingual = true
                    },
                    new Voice
                    {
                        Id = Guid.NewGuid(),
                        Name = "en-US-AnaNeural",
                        DisplayName = "Ana",
                        Type = VoiceType.Kid,
                        IsMultilingual = false
                    },
                    new Voice
                    {
                        Id = Guid.NewGuid(),
                        Name = "en-US-BrandonNeural",
                        DisplayName = "Brandon",
                        Type = VoiceType.Male,
                        IsMultilingual = false
                    },
                    new Voice
                    {
                        Id = Guid.NewGuid(),
                        Name = "en-US-ChristopherNeural",
                        DisplayName = "Christopher",
                        Type = VoiceType.Male,
                        IsMultilingual = false
                    },
                    new Voice
                    {
                        Id = Guid.NewGuid(),
                        Name = "en-US-JacobNeural",
                        DisplayName = "Jacob",
                        Type = VoiceType.Male,
                        IsMultilingual = false
                    },
                };
                
                var languages = new List<Language>
                {
                    new() {
                        Id = Guid.NewGuid(),
                        Name = "Polish",
                        Symbol = "pl-PL",
                        Voices = enVoices
                    },
                    new() {
                        Id = Guid.NewGuid(),
                        Name = "English",
                        Symbol = "en-US",
                        Voices = plVoices
                    }
                };

                var languagesToDelete = context.Languages
                    .AsEnumerable()
                    .Where(
                        entity =>
                            languages.Any(
                                language =>
                                    language.Name == entity.Name &&
                                    language.Symbol == entity.Symbol
                            )
                    );
                
                var voicesToDelete = context.Voices
                    .AsEnumerable()
                    .Where(
                        entity =>
                            enVoices.Concat(plVoices)
                                .Any(
                                    voice =>
                                        voice.Name == entity.Name &&
                                        voice.Type == entity.Type
                                )
                    );

                context.Voices.RemoveRange(voicesToDelete);
                context.Languages.RemoveRange(languagesToDelete);
                await context.SaveChangesAsync();
                
                await context.Languages.AddRangeAsync(languages);
                await context.SaveChangesAsync();
                
                return Results.Ok();
            });//.RequireAuthorization("hearing-books");
    }
}