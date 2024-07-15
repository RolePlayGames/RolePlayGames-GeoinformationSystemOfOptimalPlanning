using GSOP.Domain.Contracts.Orders.Exceptions;
using GSOP.Domain.Contracts.Orders.Models;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Orders;

namespace GSOP.Domain.Test.Orders;

public class OrderTest
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;

    private readonly OrderNumber _orderNumber;
    private readonly CustomerID _customerID;
    private readonly FilmRecipeID _filmRecipeID;
    private readonly OrderWidth _orderWidth;
    private readonly OrderQuantityInRunningMeter _orderQuantityInRunning;
    private readonly OrderFinishedGoods _orderFinishedGoods;
    private readonly OrderWaste _orderWaste;
    private readonly OrderRollsCount _orderRollsCount;
    private readonly OrderPlannedDate _orderPlannedDate;
    private readonly OrderPriceOverdue _orderPriceOverdue;

    private readonly Order _order;

    public OrderTest()
    {
        _orderRepositoryMock = new(MockBehavior.Strict);

        _orderNumber = new OrderNumber("3030214");
        _customerID = new CustomerID(1);
        _filmRecipeID = new FilmRecipeID(1);
        _orderWidth = new OrderWidth(300);
        _orderQuantityInRunning = new OrderQuantityInRunningMeter(11990);
        _orderFinishedGoods = new OrderFinishedGoods(636);
        _orderWaste = new OrderWaste(148);
        _orderRollsCount = new OrderRollsCount(12);
        _orderPlannedDate = new OrderPlannedDate(DateTime.Parse("2016.10.07T21:30:00"));
        _orderPriceOverdue = new OrderPriceOverdue(5.5);

        _order = new(
            _orderNumber,
            _customerID,
            _filmRecipeID,
            _orderWidth,
            _orderQuantityInRunning,
            _orderFinishedGoods,
            _orderWaste,
            _orderRollsCount,
            _orderPlannedDate,
            _orderPriceOverdue,
            _orderRepositoryMock.Object);
    }

    [Fact]
    public async Task SetNumber_NewNumberDoesNotExist_UpdatesOrderNumber()
    {
        // Arrange
        var newNumber = new OrderNumber("3030215");

        _orderRepositoryMock
            .Setup(x => x.IsNumberExists(newNumber))
            .ReturnsAsync(false)
            .Verifiable();

        // Act
        await _order.SetNumber(newNumber);

        // Assert
        _order.Number.Should().Be(newNumber);

        _orderRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task SetNumber_NewNumberExists_ThrowsOrderNumberAlreadyExistsException()
    {
        // Arrange
        var newNumber = new OrderNumber("3030215");

        _orderRepositoryMock
            .Setup(x => x.IsNumberExists(newNumber))
            .ReturnsAsync(true)
            .Verifiable();

        // Act & Assert
        var action = async () => await _order.SetNumber(newNumber);

        await action.Should().ThrowAsync<OrderNumberAlreadyExistsException>();

        _order.Number.Should().Be(_orderNumber);

        _orderRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task SetCustomerId_CustomerExists_UpdatesOrderCustomerID()
    {
        // Arrange
        var newCustomerId = new CustomerID(2);

        _orderRepositoryMock
            .Setup(x => x.IsCustomerExists(newCustomerId))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        await _order.SetCustomerID(newCustomerId);

        // Assert
        _order.CustomerID.Should().Be(newCustomerId);

        _orderRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task SetCustomerId_CustomerDoesNotExist_ThrowsCustomerDoesNotExistsException()
    {
        // Arrange
        var newFilmTypeId = new CustomerID(2);

        _orderRepositoryMock
            .Setup(x => x.IsCustomerExists(newFilmTypeId))
            .ReturnsAsync(false)
            .Verifiable();

        // Act & Assert
        var action = async () => await _order.SetCustomerID(newFilmTypeId);

        await action.Should().ThrowAsync<CustomerDoesNotExistsException>();

        _order.CustomerID.Should().Be(_customerID);

        _orderRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task SetFilmRecipeId_FilmRecipeExists_UpdatesOrderFilmRecipeID()
    {
        // Arrange
        var newFilmRecipeId = new FilmRecipeID(2);

        _orderRepositoryMock
            .Setup(x => x.IsFilmRecipeExists(newFilmRecipeId))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        await _order.SetFilmRecipeID(newFilmRecipeId);

        // Assert
        _order.FilmRecipeID.Should().Be(newFilmRecipeId);

        _orderRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task SetFilmRecipeId_FilmRecipeDoesNotExist_ThrowsFilmRecipeDoesNotExistsException()
    {
        // Arrange
        var newFilmRecipeId = new FilmRecipeID(2);

        _orderRepositoryMock
            .Setup(x => x.IsFilmRecipeExists(newFilmRecipeId))
            .ReturnsAsync(false)
            .Verifiable();

        // Act & Assert
        var action = async () => await _order.SetFilmRecipeID(newFilmRecipeId);

        await action.Should().ThrowAsync<FilmRecipeDoesNotExistsException>();

        _order.CustomerID.Should().Be(_customerID);

        _orderRepositoryMock.VerifyStrongly();
    }
}
