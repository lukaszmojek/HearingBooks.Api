using HearingBooks.Api.Storage;
using HearingBooks.Domain.Entities;
using HearingBooks.Infrastructure.Repositories;

namespace HearingBooks.Api.Syntheses.TextSyntheses.DownloadTextSynthesisFile;

public class DownloadTextSynthesisFileEndpoint : Endpoint<DowloadTextSynthesisFileRequest>
{
	private ITextSynthesisRepository _textSynthesisRepository;
	private IStorageService _storageService;

	public DownloadTextSynthesisFileEndpoint(ITextSynthesisRepository textSynthesisRepository, IStorageService storageService)
	{
		_textSynthesisRepository = textSynthesisRepository;
		_storageService = storageService;
	}

	public override void Configure()
	{
		Get("text-syntheses/{TextSynthesisId}");
		Roles("HearingBooks", "Writer", "Subscriber", "PayAsYouGo");
	}

	public override async Task HandleAsync(DowloadTextSynthesisFileRequest request, CancellationToken cancellationToken)
	{
		var requestingUser = (User) HttpContext.Items["User"];
                
		var synthesis = await _textSynthesisRepository.GetById(request.TextSynthesisId);

		if (synthesis.RequestingUserId != requestingUser.Id)
		{
		    await SendForbiddenAsync(cancellationToken);
		}

		var containerClient = await _storageService.GetBlobContainerClientAsync(synthesis.RequestingUserId.ToString());
		var blobClient = containerClient.GetBlobClient(synthesis.BlobName);

		var blob = blobClient.DownloadContent();

		var blobBytes = blob.Value.Content.ToArray();
		
		var blobDataStream = new MemoryStream(blobBytes);

		await SendStreamAsync(blobDataStream, $"{request.TextSynthesisId}.wav", blobDataStream.Length);
	}
}