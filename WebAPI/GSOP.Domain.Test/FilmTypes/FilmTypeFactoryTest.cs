using AutoFixture;
using GSOP.Domain.Contracts;
using GSOP.Domain.FilmTypes;
using GSOP.Domain.Contracts.FilmTypes;
using GSOP.Domain.Contracts.FilmTypes.Models;
using GSOP.Domain.Contracts.FilmTypes.Exceptions;

namespace GSOP.Domain.Test.FilmTypes;

public class FilmTypeFactoryTest
{
    private static readonly Fixture _fixture = new();
    private readonly Mock<IFilmTypeRepository> _filmTypeRepositoryMock;
    private readonly FilmTypeFactory _filmTypeFactory;

    public FilmTypeFactoryTest()
    {
        _filmTypeRepositoryMock = new(MockBehavior.Strict);
        _filmTypeFactory = new(_filmTypeRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateFilmType_ById_FilmTypeExists_CreatesFilmTypeFromRepository()
    {
        // Arrange
        var id = _fixture.Create<ID>();
        var filmTypeDTO = new FilmTypeDTO { Article = "NFS" };
        var filmTypeArticle = new FilmTypeArticle(filmTypeDTO.Article);

        _filmTypeRepositoryMock
            .Setup(x => x.Get(id))
            .ReturnsAsync(filmTypeDTO)
            .Verifiable();

        // Act
        var filmType = await _filmTypeFactory.CreateFilmType(id);

        // Assert
        filmType.Article.Should().Be(filmTypeArticle);

        _filmTypeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task CreateFilmType_ById_FilmTypeDoesNotExist_ThrowsFilmTypeWasNotFoundException()
    {
        // Arrange
        var id = _fixture.Create<ID>();

        _filmTypeRepositoryMock
            .Setup(x => x.Get(id))
            .ReturnsAsync((FilmTypeDTO?)null)
            .Verifiable();

        // Act & Assert
        var action = async () => await _filmTypeFactory.CreateFilmType(id);

        await action.Should().ThrowAsync<FilmTypeWasNotFoundException>();

        _filmTypeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task CreateFilmType_ByDTOFilmTypeNameDoesNotExist_CreatesNewFilmType()
    {
        // Arrange
        var filmTypeDTO = new FilmTypeDTO { Article = "NFS" };
        var filmTypeArticle = new FilmTypeArticle(filmTypeDTO.Article);

        _filmTypeRepositoryMock
            .Setup(x => x.IsArticleExsits(filmTypeArticle))
            .ReturnsAsync(false)
            .Verifiable();

        // Act
        var filmType = await _filmTypeFactory.CreateFilmType(filmTypeDTO);

        // Assert
        filmType.Article.Should().Be(filmTypeArticle);

        _filmTypeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task CreateFilmType_ByDTO_FilmTypeNameExists_ThrowsFilmTypeArticleAlreadyExistsException()
    {
        // Arrange
        var filmTypeDTO = new FilmTypeDTO { Article = "NFS" };
        var filmTypeArticle = new FilmTypeArticle(filmTypeDTO.Article);

        _filmTypeRepositoryMock
            .Setup(x => x.IsArticleExsits(filmTypeArticle))
            .ReturnsAsync(true)
            .Verifiable();

        // Act & Assert
        var action = async () => await _filmTypeFactory.CreateFilmType(filmTypeDTO);

        await action.Should().ThrowAsync<FilmTypeArticleAlreadyExistsException>();

        _filmTypeRepositoryMock.VerifyStrongly();
    }
}
