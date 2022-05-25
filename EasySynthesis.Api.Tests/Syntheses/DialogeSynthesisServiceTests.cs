using System;
using EasySynthesis.Api.Seed;
using EasySynthesis.Api.Syntheses.DialogueSyntheses.RequestDialogueSynthesis;
using EasySynthesis.Infrastructure;
using EasySynthesis.Infrastructure.Repositories;
using EasySynthesis.Persistance;
using Xunit;

namespace EasySynthesis.Api.Tests.Syntheses;

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
			TestsFixture.GetService<ISynthesisPricingService>()
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
		var dialogueSynthesisRequest = new DialogueSyntehsisRequest
		{
			DialogueText = DialogueText(),
			FirstSpeakerVoice = "pl-PL-AgnieszkaNeural",
			SecondSpeakerVoice = "pl-PL-MarekNeural",
			Language = "pl-PL",
			Title = "Test Dialogue Synthesis"
		};

		var requestingUser = await _userRepository.GetUserByIdAsync(SeedConfig.TestUserId);
			
		var requestId = await _dialogueSynthesisService.CreateRequest(dialogueSynthesisRequest, requestingUser);
		
		Assert.NotEqual(requestId, Guid.Empty);
	}
}