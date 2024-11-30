using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.ProductionLines;
using GSOP.Domain.Contracts.ProductionLines.Exceptions;
using GSOP.Domain.Contracts.ProductionLines.Models;
using GSOP.Domain.Contracts.ProductionLines.ProductionRules;
using GSOP.Interfaces.API.Test.Integrations.Core;
using GSOP.Interfaces.API.Test.Integrations.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace GSOP.Interfaces.API.Test.Integrations.ProductionLines;

public class ProductionLinesControllerTest : WebIntegrationTestBase
{
    private const string _url = "/api/production-lines";

    private static readonly ProductionLineDTO _productionLine = new()
    {
        Name = "MEX 08",
        HourCost = 155,
        MaxProductionSpeed = 120,
        WidthMin = 0,
        WidthMax = 500,
        ThicknessMin = 0,
        ThicknessMax = 80,
        ThicknessChangeTime = TimeSpan.FromMinutes(15),
        ThicknessChangeConsumption = 8,
        WidthChangeTime = TimeSpan.FromMinutes(15),
        WidthChangeConsumption = 8,
        SetupTime = TimeSpan.FromMinutes(30),
        NozzleChangeRules = [new NozzleChangeRuleDTO { NozzleTo = 5, ChangeTime = TimeSpan.FromMinutes(15), ChangeConsumption = 8 }],
        CalibratoinChangeRules = [new CalibratoinChangeRuleDTO { CalibrationTo = 25, ChangeTime = TimeSpan.FromMinutes(15), ChangeConsumption = 8 }],
        CoolingLipChangeRules = [new CoolingLipChangeRuleDTO { CoolingLipTo = 5, ChangeTime = TimeSpan.FromMinutes(15), ChangeConsumption = 8 }],
        FilmTypeChangeRules = [new FilmTypeChangeRuleDTO { FilmRecipeFromID = 1, FilmRecipeToID = 2, ChangeTime = TimeSpan.FromMinutes(15), ChangeConsumption = 8 }],
    };

    private static readonly ProductionLineDTO _newProductionLine = new()
    {
        Name = "MEX 09",
        HourCost = 185,
        MaxProductionSpeed = 140,
        WidthMin = 0,
        WidthMax = 800,
        ThicknessMin = 0,
        ThicknessMax = 120,
        ThicknessChangeTime = TimeSpan.FromMinutes(20),
        ThicknessChangeConsumption = 10,
        WidthChangeTime = TimeSpan.FromMinutes(20),
        WidthChangeConsumption = 10,
        SetupTime = TimeSpan.FromMinutes(40),
        NozzleChangeRules = [new NozzleChangeRuleDTO { NozzleTo = 5, ChangeTime = TimeSpan.FromMinutes(30), ChangeConsumption = 10 }],
        CalibratoinChangeRules = [new CalibratoinChangeRuleDTO { CalibrationTo = 25, ChangeTime = TimeSpan.FromMinutes(30), ChangeConsumption = 10 }],
        CoolingLipChangeRules = [new CoolingLipChangeRuleDTO { CoolingLipTo = 15, ChangeTime = TimeSpan.FromMinutes(30), ChangeConsumption = 10 }],
        FilmTypeChangeRules = [new FilmTypeChangeRuleDTO { FilmRecipeFromID = 1, FilmRecipeToID = 2, ChangeTime = TimeSpan.FromMinutes(30), ChangeConsumption = 10 }],
    };

    private readonly Mock<IProductionLineRepository> _productionLineRepositoryMock;

    public ProductionLinesControllerTest()
    {
        _productionLineRepositoryMock = new(MockBehavior.Strict);
    }

    [Fact]
    public async Task CreateProductionLine_NameDoesNotExist_ReturnsStatusCodeOKAndProductionLineID()
    {
        // Arrange
        var request = _productionLine;

        _productionLineRepositoryMock
            .Setup(x => x.IsNameExists(It.Is<ProductionLineName>(x => x == request.Name)))
            .ReturnsAsync(false)
            .Verifiable();

        var productionLineID = Fixture.Create<long>();

        _productionLineRepositoryMock
            .Setup(x => x.Create(It.Is<IProductionLine>(x => x.Name == request.Name)))
            .ReturnsAsync(productionLineID)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(_url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadAsAsync<long>();

        result.Should().Be(productionLineID);

        _productionLineRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task CreateProductionLine_NameExists_ReturnsStatusCodeUnprocessableEntity()
    {
        // Arrange
        var request = _productionLine;

        _productionLineRepositoryMock
            .Setup(x => x.IsNameExists(It.Is<ProductionLineName>(x => x == request.Name)))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(_url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        var result = await response.Content.ReadAsStringAsync();

        result.Should().Be(nameof(ProductionLineNameAlreadyExistsException));

        _productionLineRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task DeleteProductionLine_ProductionLineWasDeletedSuccessfully_ReturnsStatusCodeOK()
    {
        // Arrange
        var productionLineId = Fixture.Create<long>();
        var url = $"{_url}/{productionLineId}";

        _productionLineRepositoryMock
            .Setup(x => x.Delete(It.Is<ID>(x => x == productionLineId)))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var response = await Client.DeleteAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        _productionLineRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task DeleteProductionLine_ProductionLineWasNotDeletedSuccessfully_ReturnsStatusCodeNotFound()
    {
        // Arrange
        var productionLineId = Fixture.Create<long>();
        var url = $"{_url}/{productionLineId}";

        _productionLineRepositoryMock
            .Setup(x => x.Delete(It.Is<ID>(x => x == productionLineId)))
            .ReturnsAsync(false)
            .Verifiable();

        // Act
        var response = await Client.DeleteAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        _productionLineRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task GetProductionLine_ProductionLineExists_ReturnsStatusCodeOKAndCustomer()
    {
        // Arrange
        var productionLineId = Fixture.Create<long>();
        var url = $"{_url}/{productionLineId}";

        var productionLine = Fixture.Create<ProductionLineDTO>();

        _productionLineRepositoryMock
            .Setup(x => x.Get(It.Is<ID>(x => x == productionLineId)))
            .ReturnsAsync(productionLine)
            .Verifiable();

        // Act
        var response = await Client.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadAsAsync<ProductionLineDTO>();

        result.HourCost.Should().Be(productionLine.HourCost);
        result.MaxProductionSpeed.Should().Be(productionLine.MaxProductionSpeed);
        result.Name.Should().Be(productionLine.Name);
        result.SetupTime.Should().Be(productionLine.SetupTime);
        result.ThicknessChangeConsumption.Should().Be(productionLine.ThicknessChangeConsumption);
        result.ThicknessChangeTime.Should().Be(productionLine.ThicknessChangeTime);
        result.ThicknessMax.Should().Be(productionLine.ThicknessMax);
        result.ThicknessMin.Should().Be(productionLine.ThicknessMin);
        result.WidthChangeConsumption.Should().Be(productionLine.WidthChangeConsumption);
        result.WidthChangeTime.Should().Be(productionLine.WidthChangeTime);
        result.WidthMax.Should().Be(productionLine.WidthMax);
        result.WidthMin.Should().Be(productionLine.WidthMin);
        result.CalibratoinChangeRules.Should().BeEquivalentTo(productionLine.CalibratoinChangeRules);
        result.CoolingLipChangeRules.Should().BeEquivalentTo(productionLine.CoolingLipChangeRules);
        result.FilmTypeChangeRules.Should().BeEquivalentTo(productionLine.FilmTypeChangeRules);
        result.NozzleChangeRules.Should().BeEquivalentTo(productionLine.NozzleChangeRules);

        _productionLineRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task GetProductionLine_ProductionLineDoesNotExist_ReturnsStatusCodeNotFound()
    {
        // Arrange
        var productionLineId = Fixture.Create<long>();
        var url = $"{_url}/{productionLineId}";

        _productionLineRepositoryMock
            .Setup(x => x.Get(It.Is<ID>(x => x == productionLineId)))
            .ReturnsAsync((ProductionLineDTO?)null)
            .Verifiable();

        // Act
        var response = await Client.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        _productionLineRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task GetProductionLinesInfo_ReturnsStatusCodeOKAndCustomersInfo()
    {
        // Arrange
        var url = $"{_url}/info";
        var productionLines = Fixture.CreateMany<ProductionLineInfo>().ToList();

        _productionLineRepositoryMock
            .Setup(x => x.GetInfos())
            .ReturnsAsync(productionLines)
            .Verifiable();

        // Act
        var response = await Client.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadAsAsync<IReadOnlyCollection<ProductionLineInfo>>();

        result.Should().ContainInConsecutiveOrder(productionLines);

        _productionLineRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task UpdateProductionLine_ProductionLineExistsAndNameDoesNotExist_ReturnsStatusCodeOKAndProductionLineID()
    {
        // Arrange
        var productionLineId = Fixture.Create<long>();
        var url = $"{_url}/{productionLineId}";

        var request = _newProductionLine;
        var productionLine = _productionLine;

        _productionLineRepositoryMock
            .Setup(x => x.Get(It.Is<ID>(x => x == productionLineId)))
            .ReturnsAsync(productionLine)
            .Verifiable();

        _productionLineRepositoryMock
            .Setup(x => x.IsNameExists(It.Is<ProductionLineName>(x => x == request.Name)))
            .ReturnsAsync(false)
            .Verifiable();

        _productionLineRepositoryMock
            .Setup(x => x.Update(It.Is<ID>(x => x == productionLineId), It.Is<IProductionLine>(x => x.Name == request.Name)))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        _productionLineRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task UpdateProductionLine_ProductionLineDoesNotExist_ReturnsStatusCodeNotFound()
    {
        // Arrange
        var productionLineId = Fixture.Create<long>();
        var url = $"{_url}/{productionLineId}";
        var request = _newProductionLine;

        _productionLineRepositoryMock
            .Setup(x => x.Get(It.Is<ID>(x => x == productionLineId)))
            .ReturnsAsync((ProductionLineDTO?)null)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        _productionLineRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task UpdateProductionLine_ProductionLineExistsAndNumberExists_ReturnsStatusCodeUnprocessableEntity()
    {
        // Arrange
        var productionLineId = Fixture.Create<long>();
        var url = $"{_url}/{productionLineId}";

        var request = _newProductionLine;
        var filmType = _productionLine;

        _productionLineRepositoryMock
            .Setup(x => x.Get(It.Is<ID>(x => x == productionLineId)))
            .ReturnsAsync(filmType)
            .Verifiable();

        _productionLineRepositoryMock
            .Setup(x => x.IsNameExists(It.Is<ProductionLineName>(x => x == request.Name)))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        var result = await response.Content.ReadAsStringAsync();

        result.Should().Be(nameof(ProductionLineNameAlreadyExistsException));

        _productionLineRepositoryMock.VerifyStrongly();
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
        => services
            .AddScoped(_ => _productionLineRepositoryMock.Object)
            .AddCoreHelper();
}
