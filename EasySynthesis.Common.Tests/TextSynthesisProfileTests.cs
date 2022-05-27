using AutoMapper;
using EasySynthesis.Common.Mapper;
using EasySynthesis.Contracts;
using EasySynthesis.Domain.Entities;
using EasySynthesis.Domain.ValueObjects.Syntheses;

namespace EasySynthesis.Common.Tests;

public class TextSynthesisProfileTests
{
    private IMapper _mapper;
    private TextSynthesis _textSynthesis;
    
    public TextSynthesisProfileTests()
    {
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<TextSynthesisProfile>());
        _mapper = mapperConfig.CreateMapper();

        var userId = Guid.NewGuid();
        var synthesisId = Guid.NewGuid();
        var textToSynthesize = new string('s', 200);
        
        _textSynthesis = new TextSynthesis
        {
            Id = synthesisId,
            BlobContainerName = userId.ToString(),
            BlobName = "sample.wav",
            CharacterCount = textToSynthesize.Length,
            DurationInSeconds = 10,
            Language = new Language
            {
                Name = "Polski",
            },
            PriceInUsd = 10,
            Status = TextSynthesisStatus.Completed,
            SynthesisText = textToSynthesize,
            Title = "some title",
            User = new User
            {
                Id = userId
            },
            Voice = new Voice
            {
                Name = "Krzysztof"
            }
        };
    }
    
    [Fact]
    public void Should_Correctly_Map_All_Basic_Properties()
    {
        var textSynthesisDto = _mapper.Map<TextSynthesisDto>(_textSynthesis);
        
        Assert.Equal(_textSynthesis.Id, textSynthesisDto.Id);
        Assert.Equal(_textSynthesis.BlobContainerName, textSynthesisDto.BlobContainerName);
        Assert.Equal(_textSynthesis.BlobName, textSynthesisDto.BlobName);
        Assert.Equal(_textSynthesis.CharacterCount, textSynthesisDto.CharacterCount);
        Assert.Equal(_textSynthesis.DurationInSeconds, textSynthesisDto.DurationInSeconds);
        Assert.Equal(_textSynthesis.PriceInUsd, textSynthesisDto.PriceInUsd);
        Assert.Equal(_textSynthesis.Status, textSynthesisDto.Status);
        Assert.Equal(_textSynthesis.SynthesisText, textSynthesisDto.SynthesisText);
        Assert.Equal(_textSynthesis.Title, textSynthesisDto.Title);
    }
    
    [Fact]
    public void Should_Correctly_Map_Language_Name()
    {
        var textSynthesisDto = _mapper.Map<TextSynthesisDto>(_textSynthesis);
        
        Assert.Equal(_textSynthesis.Language.Name, textSynthesisDto.Language);
    }
    
    [Fact]
    public void Should_Correctly_Map_Voice_Name()
    {
        var textSynthesisDto = _mapper.Map<TextSynthesisDto>(_textSynthesis);
        
        Assert.Equal(_textSynthesis.Voice.Name, textSynthesisDto.Voice);
    }
    
    [Fact]
    public void Should_Correctly_Map_User_Id()
    {
        var textSynthesisDto = _mapper.Map<TextSynthesisDto>(_textSynthesis);
        
        Assert.Equal(_textSynthesis.User.Id, textSynthesisDto.RequestingUserId);
    }
}