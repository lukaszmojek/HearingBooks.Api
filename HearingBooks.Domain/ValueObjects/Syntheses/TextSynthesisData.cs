using System.ComponentModel.DataAnnotations.Schema;
using HearingBooks.Domain.DDD;
using Microsoft.EntityFrameworkCore;

namespace HearingBooks.Domain.ValueObjects.TextSynthesis;

[Owned]
// [ComplexType]
public class TextSynthesisData : ValueObject<TextSynthesisData>
{
    public string Title { get; }
    public string BlobContainerName { get; }
    public string BlobName { get; }

    public TextSynthesisData()
    {
        
    }
    
    public TextSynthesisData(string title, string blobContainerName, string blobName)
    {
        if (string.IsNullOrWhiteSpace(title)) 
            throw new ArgumentException($"{nameof(title)} cannot be empty");
        
        if (string.IsNullOrWhiteSpace(blobContainerName)) 
            throw new ArgumentException($"{nameof(blobContainerName)} cannot be empty");
        
        if (string.IsNullOrWhiteSpace(blobName)) 
            throw new ArgumentException($"{nameof(blobName)} cannot be empty");
            
        Title = title;
        BlobContainerName = blobContainerName;
        BlobName = blobName;
    }
    
    protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
    {
        yield return Title;
        yield return BlobContainerName;
        yield return BlobName;
    }
}