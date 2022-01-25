namespace HearingBooks.Api.Storage;

public class FileService : IFileService
{
    public (StreamWriter, string) CreateTextFile(string fileName)
    {
        var path = CreateTextFilePath(fileName);
        
        return (File.CreateText(path), path);
    }

    public FileStream CreateFile(string fileName)
    {
        var path = CreateFilePath(fileName);
        
        return File.Create(path);
    }

    public async Task WriteToTextFileAsync(StreamWriter writer, string content)
    {
        await writer.WriteAsync(content);
        await writer.FlushAsync();
    }

    public async Task WriteToFileAsync(FileStream file)
    {
        await file.WriteAsync(new byte[] {});
    }
    
    private string CreateFilePath(string fileName) => $"./{fileName}";
    private string CreateTextFilePath(string fileName) => $"{CreateFilePath(fileName)}.txt";
}