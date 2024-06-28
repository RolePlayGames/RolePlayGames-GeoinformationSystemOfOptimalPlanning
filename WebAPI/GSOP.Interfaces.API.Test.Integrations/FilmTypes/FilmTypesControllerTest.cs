using GSOP.Domain.Contracts;
using GSOP.Interfaces.API.Test.Integrations.Core.Extensions;
using GSOP.Interfaces.API.Test.Integrations.Core;
using Microsoft.Extensions.DependencyInjection;
using GSOP.Domain.Contracts.FilmTypes;
using GSOP.Domain.Contracts.FilmTypes.Models;
using GSOP.Domain.Contracts.FilmTypes.Exceptions;

namespace GSOP.Interfaces.API.Test.Integrations.FilmTypes;

public class FilmTypesControllerTest : WebIntegrationTestBase
{
    private const string _url = "/api/film-types";

    private readonly Mock<IFilmTypeRepository> _filmTypeRepositoryMock;

    public FilmTypesControllerTest()
    {
        _filmTypeRepositoryMock = new(MockBehavior.Strict);
    }

    [Fact]
    public async Task CreateFilmType_ArticleDoesNotExist_ReturnsStatusCodeOKAndFilmTypeId()
    {
        // Arrange        
        var request = new FilmTypeDTO { Article = "NFS" };

        _filmTypeRepositoryMock
            .Setup(x => x.IsArticleExsits(It.Is<FilmTypeArticle>(x => x == request.Article)))
            .ReturnsAsync(false)
            .Verifiable();

        var customerId = Fixture.Create<long>();

        _filmTypeRepositoryMock
            .Setup(x => x.Create(It.Is<IFilmType>(x => x.Article == request.Article)))
            .ReturnsAsync(customerId)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(_url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadAsAsync<long>();

        result.Should().Be(customerId);

        _filmTypeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task CreateFilmType_ArticleExists_ReturnsStatusCodeUnprocessableEntity()
    {
        // Arrange        
        var request = new FilmTypeDTO { Article = "NFS" };

        _filmTypeRepositoryMock
            .Setup(x => x.IsArticleExsits(It.Is<FilmTypeArticle>(x => x == request.Article)))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(_url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        var result = await response.Content.ReadAsStringAsync();

        result.Should().Be(nameof(FilmTypeArticleAlreadyExistsException));

        _filmTypeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task DeleteFilmType_FilmTypeWasDeletedSuccessfully_ReturnsStatusCodeOK()
    {
        // Arrange
        var filmTypeId = Fixture.Create<long>();
        var url = $"{_url}/{filmTypeId}";

        _filmTypeRepositoryMock
            .Setup(x => x.Delete(It.Is<ID>(x => x == filmTypeId)))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var response = await Client.DeleteAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        _filmTypeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task DeleteFilmType_FilmTypeWasNotDeletedSuccessfully_ReturnsStatusCodeNotFound()
    {
        // Arrange
        var filmTypeId = Fixture.Create<long>();
        var url = $"{_url}/{filmTypeId}";

        _filmTypeRepositoryMock
            .Setup(x => x.Delete(It.Is<ID>(x => x == filmTypeId)))
            .ReturnsAsync(false)
            .Verifiable();

        // Act
        var response = await Client.DeleteAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        _filmTypeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task GetFilmType_FilmTypeExists_ReturnsStatusCodeOKAndCustomer()
    {
        // Arrange
        var filmTypeId = Fixture.Create<long>();
        var url = $"{_url}/{filmTypeId}";

        var customer = Fixture.Create<FilmTypeDTO>();

        _filmTypeRepositoryMock
            .Setup(x => x.Get(It.Is<ID>(x => x == filmTypeId)))
            .ReturnsAsync(customer)
            .Verifiable();

        // Act
        var response = await Client.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadAsAsync<FilmTypeDTO>();

        result.Should().Be(customer);

        _filmTypeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task GetFilmType_FilmTypeDoesNotExist_ReturnsStatusCodeNotFound()
    {
        // Arrange
        var filmTypeId = Fixture.Create<long>();
        var url = $"{_url}/{filmTypeId}";

        _filmTypeRepositoryMock
            .Setup(x => x.Get(It.Is<ID>(x => x == filmTypeId)))
            .ReturnsAsync((FilmTypeDTO?)null)
            .Verifiable();

        // Act
        var response = await Client.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        _filmTypeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task GetFilmTypesInfo_ReturnsStatusCodeOKAndCustomersInfo()
    {
        // Arrange
        var url = $"{_url}/info";
        var filmTypes = Fixture.CreateMany<FilmTypeInfo>().ToList();

        _filmTypeRepositoryMock
            .Setup(x => x.GetInfos())
            .ReturnsAsync(filmTypes)
            .Verifiable();

        // Act
        var response = await Client.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadAsAsync<IReadOnlyCollection<FilmTypeInfo>>();

        result.Should().ContainInConsecutiveOrder(filmTypes);

        _filmTypeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task UpdateFilmType_FilmTypeExistsAndArticleDoesNotExist_ReturnsStatusCodeOKAndFilmTypeId()
    {
        // Arrange
        var filmTypeId = Fixture.Create<long>();
        var url = $"{_url}/{filmTypeId}";
        var request = new FilmTypeDTO { Article = "NFS" };

        var filmType = new FilmTypeDTO { Article = "NFS2" };

        _filmTypeRepositoryMock
            .Setup(x => x.Get(It.Is<ID>(x => x == filmTypeId)))
            .ReturnsAsync(filmType)
            .Verifiable();

        _filmTypeRepositoryMock
            .Setup(x => x.IsArticleExsits(It.Is<FilmTypeArticle>(x => x == request.Article)))
            .ReturnsAsync(false)
            .Verifiable();

        _filmTypeRepositoryMock
            .Setup(x => x.Update(It.Is<ID>(x => x == filmTypeId), It.Is<IFilmType>(x => x.Article == request.Article)))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        _filmTypeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task UpdateFilmType_FilmTypeDoseNotExist_ReturnsStatusCodeNotFound()
    {
        // Arrange
        var filmTypeId = Fixture.Create<long>();
        var url = $"{_url}/{filmTypeId}";
        var request = new FilmTypeDTO { Article = "NFS" };

        _filmTypeRepositoryMock
            .Setup(x => x.Get(It.Is<ID>(x => x == filmTypeId)))
            .ReturnsAsync((FilmTypeDTO?)null)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        _filmTypeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task UpdateFilmType_FilmTypeExistsAndArticleExists_ReturnsStatusCodeUnprocessableEntity()
    {
        // Arrange
        var filmTypeId = Fixture.Create<long>();
        var url = $"{_url}/{filmTypeId}";
        var request = new FilmTypeDTO { Article = "NFS" };

        var filmType = new FilmTypeDTO { Article = "NFS2" };

        _filmTypeRepositoryMock
            .Setup(x => x.Get(It.Is<ID>(x => x == filmTypeId)))
            .ReturnsAsync(filmType)
            .Verifiable();

        _filmTypeRepositoryMock
            .Setup(x => x.IsArticleExsits(It.Is<FilmTypeArticle>(x => x == request.Article)))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        var result = await response.Content.ReadAsStringAsync();

        result.Should().Be(nameof(FilmTypeArticleAlreadyExistsException));

        _filmTypeRepositoryMock.VerifyStrongly();
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
        => services
            .AddScoped(_ => _filmTypeRepositoryMock.Object)
            .AddCoreHelper();
}
