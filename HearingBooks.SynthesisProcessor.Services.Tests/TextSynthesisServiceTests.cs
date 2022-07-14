using EasySynthesis.Api.Seed;
using EasySynthesis.Contracts.DialogueSynthesis;
using EasySynthesis.Contracts.TextSynthesis;
using EasySynthesis.Domain.Exceptions;
using EasySynthesis.Infrastructure;
using EasySynthesis.Infrastructure.Repositories;
using EasySynthesis.Persistance;
using EasySynthesis.Services;
using EasySynthesis.Services.Speech;
using EasySynthesis.Tests.Core;

namespace EasySynthesis.SynthesisProcessor.Tests;

public class TextSynthesisServiceTests
{
	private readonly string _textToSynthesize = "Siema siema, pierwsza osoba mówi";

	private readonly TextSynthesisService _textSynthesisService;
	private readonly IUserRepository _userRepository;
	
	public TextSynthesisServiceTests()
	{
		_userRepository = TestsFixture.GetService<IUserRepository>();
		_textSynthesisService = new TextSynthesisService(
			TestsFixture.GetService<ISpeechService>(),
			TestsFixture.GetService<ITextSynthesisRepository>(),
			TestsFixture.GetService<HearingBooksDbContext>(),
			TestsFixture.GetService<ISynthesisPricingService>(),
			TestsFixture.GetService<IVoiceRepository>(),
			TestsFixture.GetService<ILanguageRepository>(),
			TestsFixture.GetService<IUserRepository>()
		);
	}
	

	[Fact]
	public async void CreateRequest_Should_CreateTextSynthesis()
	{
		var textSynthesisData = new TextSynthesisData()
		{
			TextToSynthesize = _textToSynthesize,
			Voice = "pl-PL-MarekNeural",
			Language = "pl-PL",
			Title = "Test Text Synthesis"
		};

		var requestId = Guid.NewGuid();
			
		var request = await _textSynthesisService.CreateRequest
			(textSynthesisData, SeedConfig.TestUserId, requestId);
		
		Assert.Equal(requestId, request.Id);
		Assert.Equal(textSynthesisData.Title, request.Title);
		Assert.Equal(textSynthesisData.TextToSynthesize, request.SynthesisText);
		Assert.Equal(textSynthesisData.Voice, request.Voice.Name);
		Assert.Equal(textSynthesisData.Language, request.Language.Symbol);
	}
	
	[Fact]
	public async void CreateRequest_Should_Throw_EasySynthesisUserCannotCreateSynthesisException_When_User_Is_Of_Type_EasySynthesis()
	{
		var textSynthesisData = new TextSynthesisData()
		{
			TextToSynthesize = _textToSynthesize,
			Voice = "pl-PL-MarekNeural",
			Language = "pl-PL",
			Title = "Test Text Synthesis"
		};

		var requestId = Guid.NewGuid();
			
		await Assert.ThrowsAsync<EasySynthesisUserCannotCreateSynthesisException>(async () => await _textSynthesisService.CreateRequest(textSynthesisData, SeedConfig.TestEasySynthesisId, requestId));
	}
	
	[Fact]
	public async void CreateRequest_Should_Throw_UserDoesNotHaveBalanceToCreateSynthesisException_When_UserDoes_Not_Have_Balance_To_Create_Synthesis()
	{
		var textSynthesisData = new TextSynthesisData()
		{
			TextToSynthesize = new string('c', 10_000_000),
			Voice = "pl-PL-MarekNeural",
			Language = "pl-PL",
			Title = "Test Text Synthesis"
		};

		var requestId = Guid.NewGuid();
			
		await Assert.ThrowsAsync<UserDoesNotHaveBalanceToCreateSynthesisException>(async () => await _textSynthesisService.CreateRequest(textSynthesisData, SeedConfig.TestUserId, requestId));
	}
}