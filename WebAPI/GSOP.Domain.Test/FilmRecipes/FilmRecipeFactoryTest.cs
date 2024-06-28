using AutoFixture;
using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.FilmRecipes;
using GSOP.Domain.FilmRecipes;
using GSOP.Domain.Contracts.FilmRecipes.Models;
using GSOP.Domain.Contracts.FilmRecipes.Exceptions;

namespace GSOP.Domain.Test.FilmRecipes;

public class FilmRecipeFactoryTest
{
    private static readonly Fixture _fixture = new();
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

    private readonly Mock<IFilmRecipeRepository> _filmRecipeRepositoryMock;
    private readonly FilmRecipeFactory _filmRecipeFactory;

    public FilmRecipeFactoryTest()
    {
        _filmRecipeRepositoryMock = new(MockBehavior.Strict);
        _filmRecipeFactory = new(_filmRecipeRepositoryMock.Object);
    }

    [Fact]
    public async Task Create_ById_FilmRecipeExists_CreatesFilmRecipeFromRepository()
    {
        // Arrange
        var id = _fixture.Create<ID>();
        var filmRecipeDTO = _filmRecipe;

        var filmRecipeName = new FilmRecipeName(filmRecipeDTO.Name);
        var filmTypeId = new FilmTypeID(filmRecipeDTO.FilmTypeID);
        var thickness = new FilmRecipeThickness(filmRecipeDTO.Thickness);
        var productionSpeed = new FilmRecipeProductionSpeed(filmRecipeDTO.ProductionSpeed);
        var materialCost = new FilmRecipeMaterialCost(filmRecipeDTO.MaterialCost);
        var nozzle = new FilmRecipeNozzle(filmRecipeDTO.Nozzle);
        var calibration = new FilmRecipeCalibration(filmRecipeDTO.Calibration);
        var coolingLip = new FilmRecipeCoolingLip(filmRecipeDTO.CoolingLip);

        _filmRecipeRepositoryMock
            .Setup(x => x.Get(id))
            .ReturnsAsync(filmRecipeDTO)
            .Verifiable();

        // Act
        var filmType = await _filmRecipeFactory.Create(id);

        // Assert
        filmType.Name.Should().Be(filmRecipeName);
        filmType.FilmTypeID.Should().Be(filmTypeId);
        filmType.Thickness.Should().Be(thickness);
        filmType.ProductionSpeed.Should().Be(productionSpeed);
        filmType.MaterialCost.Should().Be(materialCost);
        filmType.Nozzle.Should().Be(nozzle);
        filmType.Calibration.Should().Be(calibration);
        filmType.CoolingLip.Should().Be(coolingLip);

        _filmRecipeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task Create_ById_FilmRecipeDoesNotExist_ThrowsFilmRecipeWasNotFoundException()
    {
        // Arrange
        var id = _fixture.Create<ID>();

        _filmRecipeRepositoryMock
            .Setup(x => x.Get(id))
            .ReturnsAsync((FilmRecipeDTO?)null)
            .Verifiable();

        // Act & Assert
        var action = async () => await _filmRecipeFactory.Create(id);

        await action.Should().ThrowAsync<FilmRecipeWasNotFoundException>();

        _filmRecipeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task Create_ByDTOFilmRecipeNameDoesNotExistAndFilmTypeExists_CreatesNewFilmRecipe()
    {
        // Arrange
        var filmRecipeDTO = _filmRecipe;

        var filmRecipeName = new FilmRecipeName(filmRecipeDTO.Name);

        _filmRecipeRepositoryMock
            .Setup(x => x.IsNameExsits(filmRecipeName))
            .ReturnsAsync(false)
            .Verifiable();

        var filmTypeId = new FilmTypeID(filmRecipeDTO.FilmTypeID);

        _filmRecipeRepositoryMock
            .Setup(x => x.IsFilmTypeExists(filmTypeId))
            .ReturnsAsync(true)
            .Verifiable();

        var thickness = new FilmRecipeThickness(filmRecipeDTO.Thickness);
        var productionSpeed = new FilmRecipeProductionSpeed(filmRecipeDTO.ProductionSpeed);
        var materialCost = new FilmRecipeMaterialCost(filmRecipeDTO.MaterialCost);
        var nozzle = new FilmRecipeNozzle(filmRecipeDTO.Nozzle);
        var calibration = new FilmRecipeCalibration(filmRecipeDTO.Calibration);
        var coolingLip = new FilmRecipeCoolingLip(filmRecipeDTO.CoolingLip);

        // Act
        var filmType = await _filmRecipeFactory.Create(filmRecipeDTO);

        // Assert
        filmType.Name.Should().Be(filmRecipeName);
        filmType.FilmTypeID.Should().Be(filmTypeId);
        filmType.Thickness.Should().Be(thickness);
        filmType.ProductionSpeed.Should().Be(productionSpeed);
        filmType.MaterialCost.Should().Be(materialCost);
        filmType.Nozzle.Should().Be(nozzle);
        filmType.Calibration.Should().Be(calibration);
        filmType.CoolingLip.Should().Be(coolingLip);

        _filmRecipeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task Create_ByDTO_FilmRecipeNameExists_ThrowsFilmRecipeNameAlreadyExistsException()
    {
        // Arrange
        var filmRecipeDTO = _filmRecipe;
        var filmRecipeName = new FilmRecipeName(filmRecipeDTO.Name);

        _filmRecipeRepositoryMock
            .Setup(x => x.IsNameExsits(filmRecipeName))
            .ReturnsAsync(true)
            .Verifiable();

        // Act & Assert
        var action = async () => await _filmRecipeFactory.Create(filmRecipeDTO);

        await action.Should().ThrowAsync<FilmRecipeNameAlreadyExistsException>();

        _filmRecipeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task Create_ByDTO_FilmRecipeNameDoesNotExistsAndFilmTypeDoesNotExist_ThrowsFilmTypeDoesNotExistsException()
    {
        // Arrange
        var filmRecipeDTO = _filmRecipe;

        var filmRecipeName = new FilmRecipeName(filmRecipeDTO.Name);

        _filmRecipeRepositoryMock
            .Setup(x => x.IsNameExsits(filmRecipeName))
            .ReturnsAsync(false)
            .Verifiable();

        var filmTypeId = new FilmTypeID(filmRecipeDTO.FilmTypeID);

        _filmRecipeRepositoryMock
            .Setup(x => x.IsFilmTypeExists(filmTypeId))
            .ReturnsAsync(false)
            .Verifiable();

        // Act & Assert
        var action = async () => await _filmRecipeFactory.Create(filmRecipeDTO);

        await action.Should().ThrowAsync<FilmTypeDoesNotExistsException>();

        _filmRecipeRepositoryMock.VerifyStrongly();
    }
}
