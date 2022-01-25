using AutoMapper;
using Azure.Storage.Blobs.Models;
using HearingBooks.Api.Storage;
using HearingBooks.Domain.Entities;
using HearingBooks.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HearingBooks.Api.Syntheses;

public static class TextSynthesesEndpointExtensions
{
    private static readonly string _baseEndpointGroupRoute = "text-syntheses";

    public static void MapSynthesesEndpoints(this WebApplication app)
    {
        app.MapGet(
            $"/{_baseEndpointGroupRoute}",
            async (HttpContext context, ITextSynthesisRepository textSynthesisRepository, IMapper mapper) =>
            {
                var requestingUser = (User) context.Items["User"];
                
                var syntheses = await textSynthesisRepository.GetAllForUser(requestingUser.Id);
                var synthesesDto = mapper.Map<IEnumerable<TextSynthesisDto>>(syntheses);
                
                return Results.Ok(synthesesDto);
            });
        
        //TODO: How to still have route here?
        //TODO: Consider switching to FastEndpoints
        app.MapGet(
            "/text-syntheses/{textSynthesisId}",
            async ([FromRoute] Guid textSynthesisId, HttpContext context, HttpResponse httpResponse, ITextSynthesisRepository textSynthesisRepository,
                IStorageService storageService) =>
            {
                var requestingUser = (User) context.Items["User"];
                
                var synthesis = await textSynthesisRepository.GetById(textSynthesisId);

                // if (synthesis.RequestingUserId != requestingUser.Id)
                // {
                //     return Results.Forbid();
                // }

                var containerClient = await storageService.GetBlobContainerClientAsync(synthesis.RequestingUserId.ToString());
                var blobClient = containerClient.GetBlobClient(synthesis.BlobName);

                var blob = blobClient.DownloadContent();

                var blobBytes = blob.Value.Content.ToArray();
                    
                // var blobResponse = new BlobResponse
                // {
                //     Details = blob.Value.Details,
                //     Content = blobBytes
                // };
                //
                // var blobDataStream = new MemoryStream(blobBytes);
                
                httpResponse.Headers.ContentDisposition = "attachment";
                httpResponse.Headers.ContentType = "application/octet-stream";
                
                await httpResponse.BodyWriter.WriteAsync(blobBytes);
                await httpResponse.BodyWriter.FlushAsync();
                await httpResponse.BodyWriter.CompleteAsync();
            });
        
        app.MapPost(
            $"/{_baseEndpointGroupRoute}", 
            async ([FromServices] TextSynthesisService textSynthesisService, [FromBody] TextSyntehsisRequest request, HttpContext context) =>
            {
                // var result = await speech.SynthesizeAudioAsync();
                var requestingUser = (User) context.Items["User"];
                request.RequestingUserId = requestingUser.Id;
                
                var requestId = await textSynthesisService.CreateRequest(request);

                return Results.Created($"{requestId.ToString()}", null);
            });
    }
}

public class BlobResponse
{
    public BlobDownloadDetails Details { get; set; }
    public byte[] Content { get; set; }
}