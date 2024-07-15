using AutoFixture;
using GSOP.Domain.Contracts.Orders.Exceptions;
using GSOP.Domain.Contracts.Orders.Models;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts;
using GSOP.Domain.Orders;
using GSOP.Domain.Contracts.FilmRecipes.Models;
using GSOP.Domain.FilmTypes;

namespace GSOP.Domain.Test.Orders;

public class OrderFactoryTest
{
    private static readonly Fixture _fixture = new();
    private static readonly OrderDTO _order = new()
    {
        Number = "3030214",
        CustomerID = 1,
        FilmRecipeID = 1,
        Width = 300,
        QuantityInRunningMeter = 11990,
        FinishedGoods = 636,
        Waste = 148,
        RollsCount = 12,
        PlannedDate = DateTime.Parse("2016.10.07T21:30:00"),
        PriceOverdue = 5.5,
    };

    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly OrderFactory _orderFactory;

    public OrderFactoryTest()
    {
        _orderRepositoryMock = new(MockBehavior.Strict);
        _orderFactory = new(_orderRepositoryMock.Object);
    }

    [Fact]
    public async Task Create_ById_OrderExists_CreatesOrderFromRepository()
    {
        // Arrange
        var id = _fixture.Create<ID>();
        var orderDTO = _order;

        var number = new OrderNumber(orderDTO.Number);
        var customerId = new CustomerID(orderDTO.CustomerID);
        var filmRecipeId = new FilmRecipeID(orderDTO.FilmRecipeID);
        var width = new OrderWidth(orderDTO.Width);
        var quantityInRunningMeter = new OrderQuantityInRunningMeter(orderDTO.QuantityInRunningMeter);
        var finishedGoods = new OrderFinishedGoods(orderDTO.FinishedGoods);
        var waste = new OrderWaste(orderDTO.Waste);
        var rollsCount = new OrderRollsCount(orderDTO.RollsCount);
        var plannedDate = new OrderPlannedDate(orderDTO.PlannedDate);
        var priceOverdue = new OrderPriceOverdue(orderDTO.PriceOverdue);

        _orderRepositoryMock
            .Setup(x => x.Get(id))
            .ReturnsAsync(orderDTO)
            .Verifiable();

        // Act
        var order = await _orderFactory.Create(id);

        // Assert
        order.Number.Should().Be(number);
        order.CustomerID.Should().Be(customerId);
        order.FilmRecipeID.Should().Be(filmRecipeId);
        order.Width.Should().Be(width);
        order.QuantityInRunningMeter.Should().Be(quantityInRunningMeter);
        order.FinishedGoods.Should().Be(finishedGoods);
        order.Waste.Should().Be(waste);
        order.RollsCount.Should().Be(rollsCount);
        order.PlannedDate.Should().Be(plannedDate);
        order.PriceOverdue.Should().Be(priceOverdue);

        _orderRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task Create_ById_OrderDoesNotExist_ThrowsOrderWasNotFoundException()
    {
        // Arrange
        var id = _fixture.Create<ID>();

        _orderRepositoryMock
            .Setup(x => x.Get(id))
            .ReturnsAsync((OrderDTO?)null)
            .Verifiable();

        // Act & Assert
        var action = async () => await _orderFactory.Create(id);

        await action.Should().ThrowAsync<OrderWasNotFoundException>();

        _orderRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task Create_ByDTOOrderNumberDoesNotExistAndCustomerExistsAndFilmRecipeExists_CreatesNewOrder()
    {
        // Arrange
        var orderDTO = _order;

        var number = new OrderNumber(orderDTO.Number);

        _orderRepositoryMock
            .Setup(x => x.IsNumberExists(number))
            .ReturnsAsync(false)
            .Verifiable();

        var customerId = new CustomerID(orderDTO.CustomerID);

        _orderRepositoryMock
            .Setup(x => x.IsCustomerExists(customerId))
            .ReturnsAsync(true)
            .Verifiable();

        var filmRecipeId = new FilmRecipeID(orderDTO.FilmRecipeID);

        _orderRepositoryMock
            .Setup(x => x.IsFilmRecipeExists(filmRecipeId))
            .ReturnsAsync(true)
            .Verifiable();

        var width = new OrderWidth(orderDTO.Width);
        var quantityInRunningMeter = new OrderQuantityInRunningMeter(orderDTO.QuantityInRunningMeter);
        var finishedGoods = new OrderFinishedGoods(orderDTO.FinishedGoods);
        var waste = new OrderWaste(orderDTO.Waste);
        var rollsCount = new OrderRollsCount(orderDTO.RollsCount);
        var plannedDate = new OrderPlannedDate(orderDTO.PlannedDate);
        var priceOverdue = new OrderPriceOverdue(orderDTO.PriceOverdue);

        // Act
        var order = await _orderFactory.Create(orderDTO);

        // Assert
        order.Number.Should().Be(number);
        order.CustomerID.Should().Be(customerId);
        order.FilmRecipeID.Should().Be(filmRecipeId);
        order.Width.Should().Be(width);
        order.QuantityInRunningMeter.Should().Be(quantityInRunningMeter);
        order.FinishedGoods.Should().Be(finishedGoods);
        order.Waste.Should().Be(waste);
        order.RollsCount.Should().Be(rollsCount);
        order.PlannedDate.Should().Be(plannedDate);
        order.PriceOverdue.Should().Be(priceOverdue);

        _orderRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task Create_ByDTO_OrderNumberExists_ThrowsOrderNumberAlreadyExistsException()
    {
        // Arrange
        var orderDTO = _order;
        var number = new OrderNumber(orderDTO.Number);

        _orderRepositoryMock
            .Setup(x => x.IsNumberExists(number))
            .ReturnsAsync(true)
            .Verifiable();

        // Act & Assert
        var action = async () => await _orderFactory.Create(orderDTO);

        await action.Should().ThrowAsync<OrderNumberAlreadyExistsException>();

        _orderRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task Create_ByDTO_OrderNumberDoesNotExistsAndCustomerDoesNotExist_ThrowsCustomerDoesNotExistsException()
    {
        // Arrange
        var orderDTO = _order;

        var number = new OrderNumber(orderDTO.Number);

        _orderRepositoryMock
            .Setup(x => x.IsNumberExists(number))
            .ReturnsAsync(false)
            .Verifiable();

        var customerId = new CustomerID(orderDTO.CustomerID);

        _orderRepositoryMock
            .Setup(x => x.IsCustomerExists(customerId))
            .ReturnsAsync(false)
            .Verifiable();

        // Act & Assert
        var action = async () => await _orderFactory.Create(orderDTO);

        await action.Should().ThrowAsync<CustomerDoesNotExistsException>();

        _orderRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task Create_ByDTO_OrderNumberDoesNotExistsAndCustomerExistsAndFilmRecipeDoesNotExist_ThrowsFilmRecipeDoesNotExistsException()
    {
        // Arrange
        var orderDTO = _order;

        var number = new OrderNumber(orderDTO.Number);

        _orderRepositoryMock
            .Setup(x => x.IsNumberExists(number))
            .ReturnsAsync(false)
            .Verifiable();

        var customerId = new CustomerID(orderDTO.CustomerID);

        _orderRepositoryMock
            .Setup(x => x.IsCustomerExists(customerId))
            .ReturnsAsync(true)
            .Verifiable();

        var filmRecipeId = new FilmRecipeID(orderDTO.FilmRecipeID);

        _orderRepositoryMock
            .Setup(x => x.IsFilmRecipeExists(filmRecipeId))
            .ReturnsAsync(false)
            .Verifiable();

        // Act & Assert
        var action = async () => await _orderFactory.Create(orderDTO);

        await action.Should().ThrowAsync<FilmRecipeDoesNotExistsException>();

        _orderRepositoryMock.VerifyStrongly();
    }
}
