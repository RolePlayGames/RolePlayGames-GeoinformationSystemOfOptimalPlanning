using GSOP.Domain.Contracts;
using GSOP.Interfaces.API.Test.Integrations.Core.Extensions;
using GSOP.Interfaces.API.Test.Integrations.Core;
using Microsoft.Extensions.DependencyInjection;
using GSOP.Domain.Contracts.FilmRecipes;
using GSOP.Domain.Contracts.FilmRecipes.Models;
using GSOP.Domain.Contracts.FilmRecipes.Exceptions;

namespace GSOP.Interfaces.API.Test.Integrations.FilmRecipes;

public class FilmRecipesControllerTest : WebIntegrationTestBase
{
    private const string _url = "/api/film-recipes";

    private static readonly FilmRecipeDTO _filmRecipe = new()
    {
        Name = "NFS-01",
        FilmTypeID = 1,
        Thickness = 1.01,
        ProductionSpeed = 2.02,
        MaterialCost = 3.03,
        Nozzle = 4.04,
        Calibration = 5.05,
        CoolingLip = 6.06,
    };

    private static readonly FilmRecipeDTO _newFilmRecipe = new FilmRecipeDTO
    {
        Name = "NFS-02",
        FilmTypeID = 2,
        Thickness = 1.11,
        ProductionSpeed = 2.22,
        MaterialCost = 3.33,
        Nozzle = 4.44,
        Calibration = 5.55,
        CoolingLip = 6.66,
    };

    private readonly Mock<IFilmRecipeRepository> _filmRecipeRepositoryMock;

    public FilmRecipesControllerTest()
    {
        _filmRecipeRepositoryMock = new(MockBehavior.Strict);
    }

    [Fact]
    public async Task CreateFilmRecipe_NameDoesNotExistAndFilmTypeExists_ReturnsStatusCodeOKAndFilmRecipeID()
    {
        // Arrange
        var request = _filmRecipe;

        _filmRecipeRepositoryMock
            .Setup(x => x.IsNameExsits(It.Is<FilmRecipeName>(x => x == request.Name)))
            .ReturnsAsync(false)
            .Verifiable();

        _filmRecipeRepositoryMock
            .Setup(x => x.IsFilmTypeExists(It.Is<FilmTypeID>(x => x == request.FilmTypeID)))
            .ReturnsAsync(true)
            .Verifiable();

        var filmRecipeID = Fixture.Create<long>();

        _filmRecipeRepositoryMock
            .Setup(x => x.Create(It.Is<IFilmRecipe>(x => x.Name == request.Name)))
            .ReturnsAsync(filmRecipeID)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(_url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadAsAsync<long>();

        result.Should().Be(filmRecipeID);

        _filmRecipeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task CreateFilmRecipe_NameExists_ReturnsStatusCodeUnprocessableEntity()
    {
        // Arrange
        var request = _filmRecipe;

        _filmRecipeRepositoryMock
            .Setup(x => x.IsNameExsits(It.Is<FilmRecipeName>(x => x == request.Name)))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(_url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        var result = await response.Content.ReadAsStringAsync();

        result.Should().Be(nameof(FilmRecipeNameAlreadyExistsException));

        _filmRecipeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task CreateFilmRecipe_NameDoesNotExistAndFilmTypeDoesNotExists_ReturnsStatusCodeUnprocessableEntity()
    {
        // Arrange
        var request = _filmRecipe;

        _filmRecipeRepositoryMock
            .Setup(x => x.IsNameExsits(It.Is<FilmRecipeName>(x => x == request.Name)))
            .ReturnsAsync(false)
            .Verifiable();

        _filmRecipeRepositoryMock
            .Setup(x => x.IsFilmTypeExists(It.Is<FilmTypeID>(x => x == request.FilmTypeID)))
            .ReturnsAsync(false)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(_url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        var result = await response.Content.ReadAsStringAsync();

        result.Should().Be(nameof(FilmTypeDoesNotExistsException));

        _filmRecipeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task DeleteFilmRecipe_FilmRecipeWasDeletedSuccessfully_ReturnsStatusCodeOK()
    {
        // Arrange
        var filmRecipeId = Fixture.Create<long>();
        var url = $"{_url}/{filmRecipeId}";

        _filmRecipeRepositoryMock
            .Setup(x => x.Delete(It.Is<ID>(x => x == filmRecipeId)))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var response = await Client.DeleteAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        _filmRecipeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task DeleteFilmRecipe_FilmRecipeWasNotDeletedSuccessfully_ReturnsStatusCodeNotFound()
    {
        // Arrange
        var filmRecipeId = Fixture.Create<long>();
        var url = $"{_url}/{filmRecipeId}";

        _filmRecipeRepositoryMock
            .Setup(x => x.Delete(It.Is<ID>(x => x == filmRecipeId)))
            .ReturnsAsync(false)
            .Verifiable();

        // Act
        var response = await Client.DeleteAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        _filmRecipeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task GetAvaliableFilmTypes_ReturnsStatusCodeOKAndCustomersInfo()
    {
        // Arrange
        var url = $"{_url}/avaliable-film-types";
        var filmTypes = Fixture.CreateMany<AvaliableFilmType>().ToList();

        _filmRecipeRepositoryMock
            .Setup(x => x.GetAvaliableFilmTypes())
            .ReturnsAsync(filmTypes)
            .Verifiable();

        // Act
        var response = await Client.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadAsAsync<IReadOnlyCollection<AvaliableFilmType>>();

        result.Should().ContainInConsecutiveOrder(filmTypes);

        _filmRecipeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task GetFilmRecipe_FilmRecipeExists_ReturnsStatusCodeOKAndCustomer()
    {
        // Arrange
        var filmRecipeId = Fixture.Create<long>();
        var url = $"{_url}/{filmRecipeId}";

        var filmRecipe = Fixture.Create<FilmRecipeDTO>();

        _filmRecipeRepositoryMock
            .Setup(x => x.Get(It.Is<ID>(x => x == filmRecipeId)))
            .ReturnsAsync(filmRecipe)
            .Verifiable();

        // Act
        var response = await Client.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadAsAsync<FilmRecipeDTO>();

        result.Should().Be(filmRecipe);

        _filmRecipeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task GetFilmRecipe_FilmRecipeDoesNotExist_ReturnsStatusCodeNotFound()
    {
        // Arrange
        var filmRecipeId = Fixture.Create<long>();
        var url = $"{_url}/{filmRecipeId}";

        _filmRecipeRepositoryMock
            .Setup(x => x.Get(It.Is<ID>(x => x == filmRecipeId)))
            .ReturnsAsync((FilmRecipeDTO?)null)
            .Verifiable();

        // Act
        var response = await Client.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        _filmRecipeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task GetFilmRecipesInfo_ReturnsStatusCodeOKAndCustomersInfo()
    {
        // Arrange
        var url = $"{_url}/info";
        var filmRecipes = Fixture.CreateMany<FilmRecipeInfo>().ToList();

        _filmRecipeRepositoryMock
            .Setup(x => x.GetInfos())
            .ReturnsAsync(filmRecipes)
            .Verifiable();

        // Act
        var response = await Client.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadAsAsync<IReadOnlyCollection<FilmRecipeInfo>>();

        result.Should().ContainInConsecutiveOrder(filmRecipes);

        _filmRecipeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task UpdateFilmRecipe_FilmRecipeExistsAndNameDoesNotExistAndFilmTypeExists_ReturnsStatusCodeOKAndFilmRecipeID()
    {
        // Arrange
        var filmRecipeId = Fixture.Create<long>();
        var url = $"{_url}/{filmRecipeId}";

        var request = _newFilmRecipe;
        var filmRecipe = _filmRecipe;

        _filmRecipeRepositoryMock
            .Setup(x => x.Get(It.Is<ID>(x => x == filmRecipeId)))
            .ReturnsAsync(filmRecipe)
            .Verifiable();

        _filmRecipeRepositoryMock
            .Setup(x => x.IsNameExsits(It.Is<FilmRecipeName>(x => x == request.Name)))
            .ReturnsAsync(false)
            .Verifiable();

        _filmRecipeRepositoryMock
            .Setup(x => x.IsFilmTypeExists(It.Is<FilmTypeID>(x => x == request.FilmTypeID)))
            .ReturnsAsync(true)
            .Verifiable();

        _filmRecipeRepositoryMock
            .Setup(x => x.Update(It.Is<ID>(x => x == filmRecipeId), It.Is<IFilmRecipe>(x => x.Name == request.Name)))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        _filmRecipeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task UpdateFilmRecipe_FilmRecipeDoseNotExist_ReturnsStatusCodeNotFound()
    {
        // Arrange
        var filmRecipeId = Fixture.Create<long>();
        var url = $"{_url}/{filmRecipeId}";
        var request = _newFilmRecipe;

        _filmRecipeRepositoryMock
            .Setup(x => x.Get(It.Is<ID>(x => x == filmRecipeId)))
            .ReturnsAsync((FilmRecipeDTO?)null)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        _filmRecipeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task UpdateFilmRecipe_FilmRecipeExistsAndNameExists_ReturnsStatusCodeUnprocessableEntity()
    {
        // Arrange
        var filmRecipeId = Fixture.Create<long>();
        var url = $"{_url}/{filmRecipeId}";

        var request = _newFilmRecipe;
        var filmType = _filmRecipe;

        _filmRecipeRepositoryMock
            .Setup(x => x.Get(It.Is<ID>(x => x == filmRecipeId)))
            .ReturnsAsync(filmType)
            .Verifiable();

        _filmRecipeRepositoryMock
            .Setup(x => x.IsNameExsits(It.Is<FilmRecipeName>(x => x == request.Name)))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        var result = await response.Content.ReadAsStringAsync();

        result.Should().Be(nameof(FilmRecipeNameAlreadyExistsException));

        _filmRecipeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task UpdateFilmRecipe_FilmRecipeExistsAndNameDoesNotExistsAndFilmTypeDoesNotExist_ReturnsStatusCodeUnprocessableEntity()
    {
        // Arrange
        var filmRecipeId = Fixture.Create<long>();
        var url = $"{_url}/{filmRecipeId}";

        var request = _newFilmRecipe;
        var filmType = _filmRecipe;

        _filmRecipeRepositoryMock
            .Setup(x => x.Get(It.Is<ID>(x => x == filmRecipeId)))
            .ReturnsAsync(filmType)
            .Verifiable();

        _filmRecipeRepositoryMock
            .Setup(x => x.IsNameExsits(It.Is<FilmRecipeName>(x => x == request.Name)))
            .ReturnsAsync(false)
            .Verifiable();

        _filmRecipeRepositoryMock
            .Setup(x => x.IsFilmTypeExists(It.Is<FilmTypeID>(x => x == request.FilmTypeID)))
            .ReturnsAsync(false)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        var result = await response.Content.ReadAsStringAsync();

        result.Should().Be(nameof(FilmTypeDoesNotExistsException));

        _filmRecipeRepositoryMock.VerifyStrongly();
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
        => services
            .AddScoped(_ => _filmRecipeRepositoryMock.Object)
            .AddCoreHelper();
}
