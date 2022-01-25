namespace HearingBooks.Api.Storage;

public interface IFileService
{
    (StreamWriter, string) CreateTextFile(string fileName);
    FileStream CreateFile(string fileName);
    Task WriteToTextFileAsync(StreamWriter writer, string content);
    Task WriteToFileAsync(FileStream file);
}