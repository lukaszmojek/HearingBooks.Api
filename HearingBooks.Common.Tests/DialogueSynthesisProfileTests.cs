using AutoMapper;
using EasySynthesis.Common.Mapper;
using EasySynthesis.Contracts;
using EasySynthesis.Domain.Entities;
using EasySynthesis.Domain.ValueObjects.Syntheses;

namespace EasySynthesis.Common.Tests;

public class DialogueSynthesisProfileTests
{
    private IMapper _mapper;
    private DialogueSynthesis _dialogueSynthesis;
    
    public DialogueSynthesisProfileTests()
    {
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<DialogueSynthesisProfile>());
        _mapper = mapperConfig.CreateMapper();

        var userId = Guid.NewGuid();
        var synthesisId = Guid.NewGuid();
        var dialogue = new string('s', 200);
        
        _dialogueSynthesis = new DialogueSynthesis()
        {
            Id = synthesisId,
            BlobContainerName = userId.ToString(),
            BlobName = "sample.wav",
            CharacterCount = dialogue.Length,
            DurationInSeconds = 10,
            Language = new Language
            {
                Name = "Polski",
            },
            PriceInUsd = 10,
            Status = DialogueSynthesisStatus.Completed,
            DialogueText = dialogue,
            Title = "some title",
            User = new User
            {
                Id = userId
            },
            FirstSpeakerVoice = new Voice
            {
                Name = "Krzysztof"
            },
            SecondSpeakerVoice = new Voice
            {
                Name = "Zofia"
            }
        };
    }
    
    [Fact]
    public void Should_Correctly_Map_All_Basic_Properties()
    {
        var synthesisDto = _mapper.Map<DialogueSynthesisDto>(_dialogueSynthesis);
        
        Assert.Equal(_dialogueSynthesis.Id, synthesisDto.Id);
        Assert.Equal(_dialogueSynthesis.BlobContainerName, synthesisDto.BlobContainerName);
        Assert.Equal(_dialogueSynthesis.BlobName, synthesisDto.BlobName);
        Assert.Equal(_dialogueSynthesis.CharacterCount, synthesisDto.CharacterCount);
        Assert.Equal(_dialogueSynthesis.DurationInSeconds, synthesisDto.DurationInSeconds);
        Assert.Equal(_dialogueSynthesis.PriceInUsd, synthesisDto.PriceInUsd);
        Assert.Equal(_dialogueSynthesis.Status, synthesisDto.Status);
        Assert.Equal(_dialogueSynthesis.DialogueText, synthesisDto.DialogueText);
        Assert.Equal(_dialogueSynthesis.Title, synthesisDto.Title);
    }
    
    [Fact]
    public void Should_Correctly_Map_Language_Name()
    {
        var synthesisDto = _mapper.Map<DialogueSynthesisDto>(_dialogueSynthesis);
        
        Assert.Equal(_dialogueSynthesis.Language.Name, synthesisDto.Language);
    }
    
    [Fact]
    public void Should_Correctly_Map_Speaker_Voice_Names()
    {
        var synthesisDto = _mapper.Map<DialogueSynthesisDto>(_dialogueSynthesis);
        
        Assert.Equal(_dialogueSynthesis.FirstSpeakerVoice.Name, synthesisDto.FirstSpeakerVoice);
        Assert.Equal(_dialogueSynthesis.SecondSpeakerVoice.Name, synthesisDto.SecondSpeakerVoice);
    }
    
    [Fact]
    public void Should_Correctly_Map_User_Id()
    {
        var synthesisDto = _mapper.Map<DialogueSynthesisDto>(_dialogueSynthesis);
        
        Assert.Equal(_dialogueSynthesis.User.Id, synthesisDto.RequestingUserId);
    }
}