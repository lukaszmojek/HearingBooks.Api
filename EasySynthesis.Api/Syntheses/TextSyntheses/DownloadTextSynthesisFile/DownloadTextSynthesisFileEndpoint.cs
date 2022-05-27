using EasySynthesis.Domain.Entities;
using EasySynthesis.Infrastructure;
using EasySynthesis.Infrastructure.Repositories;
using EasySynthesis.Services.Core.Storage;

namespace EasySynthesis.Api.Syntheses.TextSyntheses.DownloadTextSynthesisFile;

public class DownloadTextSynthesisFileEndpoint : Endpoint<DownloadTextSynthesisFileRequest>
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
		Get("text-syntheses/{SynthesisId}");
		Roles("HearingBooks", "Writer", "Subscriber", "PayAsYouGo");
	}

	public override async Task HandleAsync(DownloadTextSynthesisFileRequest request, CancellationToken cancellationToken)
	{
		var requestingUser = (User) HttpContext.Items["User"];
                
		var synthesis = await _textSynthesisRepository.GetById(request.SynthesisId);

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