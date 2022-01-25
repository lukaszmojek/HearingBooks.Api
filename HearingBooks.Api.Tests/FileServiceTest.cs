using System.IO;
using System.Threading.Tasks;
using HearingBooks.Api.Storage;
using Xunit;

namespace HearingBooks.Api.Tests;

public class FileServiceTest
{
    private readonly string _fileName = "test_file_name";
    private string _filePath => $"./{_fileName}.txt";

    [Fact]
    public void When_Calls_CreateTextFile_Should_Create_Txt_File()
    {
        var fileService = CreateFileService();

        var _ = fileService.CreateTextFile(_fileName);
        
        Assert.True(File.Exists(_filePath));
    }
    
    [Fact]
    public async Task When_Calls_WriteToTextFileAsync_Should_Write_Content_To_Txt_File()
    {
        const string content = "some example content as plain text";
        var fileService = CreateFileService(); 
        (var writer, _) = fileService.CreateTextFile(_fileName);
        
        await fileService.WriteToTextFileAsync(writer, content);
        var fileContent = await File.ReadAllTextAsync(_filePath);
        
        Assert.Equal(content, fileContent);
    }

    private static IFileService CreateFileService()
    {
        return new FileService();
    }
}