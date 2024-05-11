using GSOP.Domain.FilmTypes;
using GSOP.Domain.Contracts.FilmTypes;
using GSOP.Domain.Contracts.FilmTypes.Exceptions;
using GSOP.Domain.Contracts.FilmTypes.Models;

namespace GSOP.Domain.Test.FilmTypes;

public class FilmTypeTest
{
    private readonly Mock<IFilmTypeRepository> _filmTypeRepositoryMock;
    private readonly FilmTypeArticle _filmTypeArticle;
    private readonly FilmType _filmType;

    public FilmTypeTest()
    {
        _filmTypeRepositoryMock = new(MockBehavior.Strict);
        _filmTypeArticle = new FilmTypeArticle("NFS");
        _filmType = new FilmType(_filmTypeArticle, _filmTypeRepositoryMock.Object);
    }

    [Fact]
    public async Task SetArticle_NewArticleDoesNotExist_UpdatesFilmTypeArticle()
    {
        // Arrange
        var newArticle = new FilmTypeArticle("NFS2");

        _filmTypeRepositoryMock
            .Setup(x => x.IsArticleExsits(newArticle))
            .ReturnsAsync(false)
            .Verifiable();

        // Act
        await _filmType.SetArticle(newArticle);

        // Assert
        _filmType.Article.Should().Be(newArticle);

        _filmTypeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task SetArticle_NewArticleDoesNotExist_ThrowsFilmTypeArticleAlreadyExistsException()
    {
        // Arrange
        var newArticle = new FilmTypeArticle("NFS2");

        _filmTypeRepositoryMock
            .Setup(x => x.IsArticleExsits(newArticle))
            .ReturnsAsync(true)
            .Verifiable();

        // Act & Assert
        var action = async () => await _filmType.SetArticle(newArticle);

        await action.Should().ThrowAsync<FilmTypeArticleAlreadyExistsException>();

        _filmType.Article.Should().Be(_filmTypeArticle);

        _filmTypeRepositoryMock.VerifyStrongly();
    }
}
