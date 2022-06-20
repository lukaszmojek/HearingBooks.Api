using VersOne.Epub;

namespace EasySynthesis.Epub;

public class EpubOperator
{
    private readonly EpubBook _book;
    
    public EpubOperator(string path)
    {
        _book = EpubReader.ReadBook(path);
    }
    
    public string Title() => _book.Title;
    public string Author() => _book.Author;
    public IList<string> AuthorList() => _book.AuthorList;
    public IList<EpubNavigationItem> Navigation() => _book.Navigation;
    public IList<EpubTextContentFile> ReadingOrder() => _book.ReadingOrder;
}