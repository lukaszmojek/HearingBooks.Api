using EasySynthesis.Api.Seed;
using EasySynthesis.Contracts.DialogueSynthesis;
using EasySynthesis.Domain.Exceptions;
using EasySynthesis.Infrastructure;
using EasySynthesis.Infrastructure.Repositories;
using EasySynthesis.Persistance;
using EasySynthesis.Services;
using EasySynthesis.Services.Speech;
using EasySynthesis.Tests.Core;

namespace EasySynthesis.SynthesisProcessor.Tests;

public class DialogeSynthesisServiceTests
{
	private readonly string _person1Line1 = "Siema siema, pierwsza osoba mówi";
	private readonly string _person2Line1 = "No heloł heloł, co tam słychować?";
	private readonly string _person1Line2 = "A no w sumie to nic ciekawego, dzięki za troskę";
	
	private string DialogueText () => $@"
		{_person1Line1}
		{DialogueSynthesisService.LineSeparator}
		{_person2Line1}
		{DialogueSynthesisService.LineSeparator}
		{_person1Line2}
	";

	private readonly DialogueSynthesisService _dialogueSynthesisService;
	private readonly IUserRepository _userRepository;
	
	public DialogeSynthesisServiceTests()
	{
		_userRepository = TestsFixture.GetService<IUserRepository>();
		_dialogueSynthesisService = new DialogueSynthesisService(
			TestsFixture.GetService<ISpeechService>(),
			TestsFixture.GetService<IDialogueSynthesisRepository>(),
			TestsFixture.GetService<HearingBooksDbContext>(),
			TestsFixture.GetService<ISynthesisPricingService>(),
			TestsFixture.GetService<IUserRepository>(),
			TestsFixture.GetService<ILanguageRepository>(),
			TestsFixture.GetService<IVoiceRepository>()
		);
	}
	
	[Fact]
	public void SplitDialogueIntoLines_Should_PerformCorrectSplit()
	{
		var lines = _dialogueSynthesisService.SplitDialogueIntoLines(DialogueText());
		
		Assert.Equal(3, lines.Count());
		Assert.Equal(_person1Line1, lines.ElementAt(0).Item1);
		Assert.Equal(0, lines.ElementAt(0).Item2);
		Assert.Equal(_person2Line1, lines.ElementAt(1).Item1);
		Assert.Equal(1, lines.ElementAt(1).Item2);
		Assert.Equal(_person1Line2, lines.ElementAt(2).Item1);
		Assert.Equal(2, lines.ElementAt(2).Item2);
	}
	
	[Fact]
	public async void CreateRequest_Should_CreateDialogueSynthesis()
	{
		var dialogueSynthesisRequest = new DialogueSynthesisData()
		{
			DialogueText = DialogueText(),
			FirstSpeakerVoice = "pl-PL-AgnieszkaNeural",
			SecondSpeakerVoice = "pl-PL-MarekNeural",
			Language = "pl-PL",
			Title = "Test Dialogue Synthesis"
		};

		var requestId = Guid.NewGuid();
			
		var request = await _dialogueSynthesisService.CreateRequest(dialogueSynthesisRequest, SeedConfig.TestUserId, requestId);
		
		Assert.Equal(requestId, request.Id);
		Assert.Equal(dialogueSynthesisRequest.Title, request.Title);
		Assert.Equal(dialogueSynthesisRequest.DialogueText, request.DialogueText);
		Assert.Equal(dialogueSynthesisRequest.FirstSpeakerVoice, request.FirstSpeakerVoice.Name);
		Assert.Equal(dialogueSynthesisRequest.SecondSpeakerVoice, request.SecondSpeakerVoice.Name);
		Assert.Equal(dialogueSynthesisRequest.Language, request.Language.Symbol);
	}
	
	[Fact]
	public async void CreateRequest_Should_Throw_EasySynthesisUserCannotCreateSynthesisException_When_User_Is_Of_Type_EasySynthesis()
	{
		var dialogueSynthesisRequest = new DialogueSynthesisData()
		{
			DialogueText = DialogueText(),
			FirstSpeakerVoice = "pl-PL-AgnieszkaNeural",
			SecondSpeakerVoice = "pl-PL-MarekNeural",
			Language = "pl-PL",
			Title = "Test Dialogue Synthesis"
		};

		var requestId = Guid.NewGuid();
			
		await Assert.ThrowsAsync<EasySynthesisUserCannotCreateSynthesisException>(async () => await _dialogueSynthesisService.CreateRequest(dialogueSynthesisRequest, SeedConfig.TestEasySynthesisId, requestId));
	}
	
	[Fact]
	public async void CreateRequest_Should_Throw_UserDoesNotHaveBalanceToCreateSynthesisException_When_UserDoes_Not_Have_Balance_To_Create_Synthesis()
	{
		var dialogueSynthesisRequest = new DialogueSynthesisData()
		{
			DialogueText = new string('c', 1_000_000_000),
			FirstSpeakerVoice = "pl-PL-AgnieszkaNeural",
			SecondSpeakerVoice = "pl-PL-MarekNeural",
			Language = "pl-PL",
			Title = "Test Dialogue Synthesis"
		};

		var requestId = Guid.NewGuid();
			
		await Assert.ThrowsAsync<UserDoesNotHaveBalanceToCreateSynthesisException>(async () => await _dialogueSynthesisService.CreateRequest(dialogueSynthesisRequest, SeedConfig.TestUserId, requestId));
	}
}