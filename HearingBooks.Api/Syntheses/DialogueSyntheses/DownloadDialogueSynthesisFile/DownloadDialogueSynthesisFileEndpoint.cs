using HearingBooks.Domain.Entities;
using HearingBooks.Infrastructure;
using HearingBooks.Infrastructure.Repositories;
using HearingBooks.Services.Core.Storage;

namespace HearingBooks.Api.Syntheses.DialogueSyntheses.DownloadDialogueSynthesisFile;

public class DownloadDialogueSynthesisFileEndpoint : Endpoint<DowloadDialogueSynthesisFileRequest>
{
	private IDialogueSynthesisRepository _dialogueSynthesisRepository;
	private IStorageService _storageService;

	public DownloadDialogueSynthesisFileEndpoint(IDialogueSynthesisRepository dialogueSynthesisRepository, IStorageService storageService)
	{
		_dialogueSynthesisRepository = dialogueSynthesisRepository;
		_storageService = storageService;
	}

	public override void Configure()
	{
		Get("dialogue-syntheses/{SynthesisId}");
		Roles("HearingBooks", "Writer", "Subscriber", "PayAsYouGo");
	}

	public override async Task HandleAsync(DowloadDialogueSynthesisFileRequest request, CancellationToken cancellationToken)
	{
		var requestingUser = (User) HttpContext.Items["User"];
                
		var synthesis = await _dialogueSynthesisRepository.GetById(request.SynthesisId);

		if (synthesis.User.Id != requestingUser.Id)
		{
		    await SendForbiddenAsync(cancellationToken);
		}

		var containerClient = await _storageService.GetBlobContainerClientAsync(synthesis.User.Id.ToString());
		var blobClient = containerClient.GetBlobClient(synthesis.BlobName);

		var blob = blobClient.DownloadContent();

		var blobBytes = blob.Value.Content.ToArray();
		
		var blobDataStream = new MemoryStream(blobBytes);

		await SendStreamAsync(blobDataStream, $"{synthesis.BlobName.CleanFromNonAsciiCharacters()}.wav", blobDataStream.Length);
	}
}