using GSOP.Domain.Contracts.FilmRecipes;
using GSOP.Domain.FilmRecipes;
using GSOP.Domain.Contracts.FilmRecipes.Models;
using GSOP.Domain.Contracts.FilmRecipes.Exceptions;

namespace GSOP.Domain.Test.FilmRecipes;

public class FilmRecipeTest
{
    private readonly Mock<IFilmRecipeRepository> _filmRecipeRepositoryMock;

    private readonly FilmRecipeName _filmRecipeName;
    private readonly FilmTypeID _filmTypeID;
    private readonly FilmRecipeThickness _filmRecipeThickness;
    private readonly FilmRecipeProductionSpeed _filmRecipeProductionSpeed;
    private readonly FilmRecipeMaterialCost _filmRecipeMaterialCost;
    private readonly FilmRecipeNozzle _filmRecipeNozzle;
    private readonly FilmRecipeCalibration _filmRecipeCalibration;
    private readonly FilmRecipeCoolingLip _filmRecipeCoolingLip;

    private readonly FilmRecipe _filmRecipe;

    public FilmRecipeTest()
    {
        _filmRecipeRepositoryMock = new(MockBehavior.Strict);

        _filmRecipeName = new FilmRecipeName("NFS-01");
        _filmTypeID = new FilmTypeID(1);
        _filmRecipeThickness = new FilmRecipeThickness(1.01);
        _filmRecipeProductionSpeed = new FilmRecipeProductionSpeed(2.02);
        _filmRecipeMaterialCost = new FilmRecipeMaterialCost(3.03);
        _filmRecipeNozzle = new FilmRecipeNozzle(4.04);
        _filmRecipeCalibration = new FilmRecipeCalibration(5.05);
        _filmRecipeCoolingLip = new FilmRecipeCoolingLip(6.06);

        _filmRecipe = new(
            _filmRecipeName,
            _filmTypeID,
            _filmRecipeThickness,
            _filmRecipeProductionSpeed,
            _filmRecipeMaterialCost,
            _filmRecipeNozzle,
            _filmRecipeCalibration,
            _filmRecipeCoolingLip,
            _filmRecipeRepositoryMock.Object);
    }

    [Fact]
    public async Task SetName_NewNameDoesNotExist_UpdatesFilmRecipeName()
    {
        // Arrange
        var newName = new FilmRecipeName("NFS-02");

        _filmRecipeRepositoryMock
            .Setup(x => x.IsNameExsits(newName))
            .ReturnsAsync(false)
            .Verifiable();

        // Act
        await _filmRecipe.SetName(newName);

        // Assert
        _filmRecipe.Name.Should().Be(newName);

        _filmRecipeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task SetName_NewNameExists_ThrowsFilmRecipeNameAlreadyExistsException()
    {
        // Arrange
        var newName = new FilmRecipeName("NFS-02");

        _filmRecipeRepositoryMock
            .Setup(x => x.IsNameExsits(newName))
            .ReturnsAsync(true)
            .Verifiable();

        // Act & Assert
        var action = async () => await _filmRecipe.SetName(newName);

        await action.Should().ThrowAsync<FilmRecipeNameAlreadyExistsException>();

        _filmRecipe.Name.Should().Be(_filmRecipeName);

        _filmRecipeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task SetFilmTypeId_FilmTypeExists_UpdatesFilmRecipeFilmTypeID()
    {
        // Arrange
        var newFilmTypeId = new FilmTypeID(2);

        _filmRecipeRepositoryMock
            .Setup(x => x.IsFilmTypeExists(newFilmTypeId))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        await _filmRecipe.SetFilmTypeID(newFilmTypeId);

        // Assert
        _filmRecipe.FilmTypeID.Should().Be(newFilmTypeId);

        _filmRecipeRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task SetFilmTypeId_FilmTypeDoesNotExist_ThrowsFilmTypeDoesNotExistsException()
    {
        // Arrange
        var newFilmTypeId = new FilmTypeID(2);

        _filmRecipeRepositoryMock
            .Setup(x => x.IsFilmTypeExists(newFilmTypeId))
            .ReturnsAsync(false)
            .Verifiable();

        // Act & Assert
        var action = async () => await _filmRecipe.SetFilmTypeID(newFilmTypeId);

        await action.Should().ThrowAsync<FilmTypeDoesNotExistsException>();

        _filmRecipe.FilmTypeID.Should().Be(_filmTypeID);

        _filmRecipeRepositoryMock.VerifyStrongly();
    }
}
