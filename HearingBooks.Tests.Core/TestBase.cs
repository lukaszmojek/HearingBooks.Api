namespace HearingBooks.Tests.Core;

public class TestBase
{
    protected string GetPathForTestFile(string fileName) => $"./../../../../HearingBooks.Tests.Core/data/{fileName}";
}