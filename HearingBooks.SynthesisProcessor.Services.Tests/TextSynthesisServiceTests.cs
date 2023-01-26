using HearingBooks.Api.Seed;
using HearingBooks.Contracts.TextSynthesis;
using HearingBooks.Domain.Exceptions;
using HearingBooks.Infrastructure;
using HearingBooks.Infrastructure.Repositories;
using HearingBooks.Persistance;
using HearingBooks.SynthesisProcessor.Services;
using HearingBooks.SynthesisProcessor.Services.Speech;
using HearingBooks.Tests.Core;

namespace HearingBooks.SynthesisProcessor.Tests;

public class TextSynthesisServiceTests
{
	private readonly string _textToSynthesize = "Siema siema, pierwsza osoba mówi";

	private readonly TextSynthesisService _textSynthesisService;
	private readonly IUserRepository _userRepository;
	
	public TextSynthesisServiceTests()
	{
		_userRepository = TestsFixture.GetService<IUserRepository>(); //TODO: Refactor tests and move that to either docker or IWebFixture or sth like that
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
	public async void CreateRequest_Should_Throw_HearingBooksUserCannotCreateSynthesisException_When_User_Is_Of_Type_HearingBooks()
	{
		var textSynthesisData = new TextSynthesisData()
		{
			TextToSynthesize = _textToSynthesize,
			Voice = "pl-PL-MarekNeural",
			Language = "pl-PL",
			Title = "Test Text Synthesis"
		};

		var requestId = Guid.NewGuid();
			
		await Assert.ThrowsAsync<HearingBooksUserCannotCreateSynthesisException>(async () => await _textSynthesisService.CreateRequest(textSynthesisData, SeedConfig.TestHearingBooksId, requestId));
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