using System.Collections.Generic;
using System.Linq;
using HearingBooks.Tests.Core;
using Xunit;

namespace HearingBooks.Epub.Tests;

public class EpubOperatorTests : TestBase
{
    [Fact]
    public void Title_Should_TitleOfTheBook()
    {
        var path = GetPathForTestFile("opowiesci-niesamowite.epub");

        var epubOperator = new EpubOperator(path);

        var result = epubOperator.Title();

        Assert.Equal("Opowieści Niesamowite", result);
    }
    
    [Fact]
    public void Author_Should_AuthorOfTheBook()
    {
        var path = GetPathForTestFile("opowiesci-niesamowite.epub");

        var epubOperator = new EpubOperator(path);

        var result = epubOperator.Author();

        Assert.Equal("Marcin Rusnak", result);
    }
    
    [Fact]
    public void AuthorList_Should_AuthorsOfTheBook()
    {
        var path = GetPathForTestFile("opowiesci-niesamowite.epub");

        var epubOperator = new EpubOperator(path);

        var result = epubOperator.AuthorList();
        
        Assert.NotEmpty(result);
        Assert.Equal("Marcin Rusnak", result.ElementAt(0));
    }

    [Fact]
    public void Navigation_Should_ReturnListOfNavigationItemsFromTheBook()
    {
        var path = GetPathForTestFile("opowiesci-niesamowite.epub");

        var navigationItemNamesFormTheBook = new List<string>
        {
            "Zaginięcie Benedicta Constantine’a",
            "Kołysanki dla umarłych",
            "Zły szeląg",
            "Pielgrzymka",
            "Pojedynek",
            "Bociani Zagon",
            "Opowieść, którą znam tylko ja",
            "Farid Tkacz i latający dywan",
            "Zabijając ptaki",
            "Koszmar z Yelland",
            "Białe piekło",
            "Pudełko pełne cudów",
            "Ostatni śnieg w roku",
        };

        var epubOperator = new EpubOperator(path);

        var result = epubOperator.Navigation();

        var navigationItemsNames = result.Select(x => x.Title).ToList();

        Assert.NotEmpty(result);
        Assert.Equal(navigationItemNamesFormTheBook.Count, navigationItemsNames.Count);

        for (var itemNumber = 0; itemNumber < navigationItemsNames.Count; itemNumber++)
        {
            Assert.Equal(
                navigationItemNamesFormTheBook.ElementAt(itemNumber),
                navigationItemsNames.ElementAt(itemNumber)
            );
        }
    }
    
    [Fact]
    public void ReadingOrder_Should_ReturnListOfAllReadableSections()
    {
        var path = GetPathForTestFile("opowiesci-niesamowite.epub");

        var epubOperator = new EpubOperator(path);

        var result = epubOperator.ReadingOrder();

        Assert.NotEmpty(result);
    }
}