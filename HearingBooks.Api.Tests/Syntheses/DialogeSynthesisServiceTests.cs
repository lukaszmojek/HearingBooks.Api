using System;
using System.Linq;
using HearingBooks.Api.Speech;
using HearingBooks.Api.Syntheses.DialogueSyntheses;
using HearingBooks.Api.Syntheses.DialogueSyntheses.RequestDialogueSynthesis;
using HearingBooks.Domain.Entities;
using HearingBooks.Infrastructure.Repositories;
using HearingBooks.Persistance;
using Moq;
using Xunit;

namespace HearingBooks.Api.Tests.Syntheses;

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

	private readonly Mock<ISpeechService> _speechServiceMock;
	private readonly Mock<IDialogueSynthesisRepository> _dialogueSynthesisRepositoryMock;
	private readonly Mock<HearingBooksDbContext> _context;
	private readonly DialogueSynthesisService _dialogueSynthesisService;
	
	public DialogeSynthesisServiceTests()
	{
		_speechServiceMock = new Mock<ISpeechService>();
		_dialogueSynthesisRepositoryMock = new Mock<IDialogueSynthesisRepository>();
		_context = new Mock<HearingBooksDbContext>();
		_dialogueSynthesisService = new DialogueSynthesisService(
			TestsFixture.GetService<ISpeechService>(),
			TestsFixture.GetService<IDialogueSynthesisRepository>(),
			// TestsFixture.GetDbContext()
			TestsFixture.GetService<HearingBooksDbContext>()
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
			
		var requestId = await _dialogueSynthesisService.CreateRequest(dialogueSynthesisRequest, new User{Id = Guid.Parse("b8a1afdc-fd52-4d87-9886-6e4fd9a5fdaa")});
		
		Assert.Equal(requestId.ToString(), "1");
	}
}