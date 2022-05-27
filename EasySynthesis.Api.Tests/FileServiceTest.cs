using System;
using System.IO;
using System.Threading.Tasks;
using EasySynthesis.Services.Core.Storage;
using Xunit;

namespace EasySynthesis.Api.Tests;

public class FileServiceTest
{
    private string _filePath(string filename) => $"./{filename}.txt";

    [Fact]
    public void When_Calls_CreateTextFile_Should_Create_Txt_File()
    {
        var fileName = Guid.NewGuid().ToString();
        var fileService = CreateFileService();

        var _ = fileService.CreateTextFile(fileName);
        
        Assert.True(File.Exists(_filePath(fileName)));
    }
    
    [Fact]
    public async Task When_Calls_WriteToTextFileAsync_Should_Write_Content_To_Txt_File()
    {
        var fileName = Guid.NewGuid().ToString();
        const string content = "some example content as plain text";
        var fileService = CreateFileService(); 
        (var writer, _) = fileService.CreateTextFile(fileName);
        
        await fileService.WriteToTextFileAsync(writer, content);
        writer.Close();
        
        var fileContent = await File.ReadAllTextAsync(_filePath(fileName));
        
        Assert.Equal(content, fileContent);
    }

    private static IFileService CreateFileService()
    {
        return new FileService();
    }
}