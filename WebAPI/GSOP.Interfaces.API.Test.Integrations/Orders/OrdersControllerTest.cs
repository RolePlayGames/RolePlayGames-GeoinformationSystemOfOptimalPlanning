using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.Orders.Exceptions;
using GSOP.Domain.Contracts.Orders.Models;
using GSOP.Interfaces.API.Test.Integrations.Core;
using GSOP.Interfaces.API.Test.Integrations.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace GSOP.Interfaces.API.Test.Integrations.Orders;

public class OrdersControllerTest : WebIntegrationTestBase
{
    private const string _url = "/api/orders";

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

    private static readonly OrderDTO _newOrder = new()
    {
        Number = "3030215",
        CustomerID = 2,
        FilmRecipeID = 2,
        Width = 400,
        QuantityInRunningMeter = 97920,
        FinishedGoods = 3108,
        Waste = 377,
        RollsCount = 96,
        PlannedDate = DateTime.Parse("2016.10.15T12:00:00"),
        PriceOverdue = 5.8,
    };

    private readonly Mock<IOrderRepository> _orderRepositoryMock;

    public OrdersControllerTest()
    {
        _orderRepositoryMock = new(MockBehavior.Strict);
    }

    [Fact]
    public async Task CreateOrder_NumberDoesNotExistAndCustomerExistsAndFilmRecipeExists_ReturnsStatusCodeOKAndOrderID()
    {
        // Arrange
        var request = _order;

        _orderRepositoryMock
            .Setup(x => x.IsNumberExists(It.Is<OrderNumber>(x => x == request.Number)))
            .ReturnsAsync(false)
            .Verifiable();

        _orderRepositoryMock
            .Setup(x => x.IsCustomerExists(It.Is<CustomerID>(x => x == request.CustomerID)))
            .ReturnsAsync(true)
            .Verifiable();

        _orderRepositoryMock
            .Setup(x => x.IsFilmRecipeExists(It.Is<FilmRecipeID>(x => x == request.FilmRecipeID)))
            .ReturnsAsync(true)
            .Verifiable();

        var orderID = Fixture.Create<long>();

        _orderRepositoryMock
            .Setup(x => x.Create(It.Is<IOrder>(x => x.Number == request.Number)))
            .ReturnsAsync(orderID)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(_url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadAsAsync<long>();

        result.Should().Be(orderID);

        _orderRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task CreateOrder_NumberExists_ReturnsStatusCodeUnprocessableEntity()
    {
        // Arrange
        var request = _order;

        _orderRepositoryMock
            .Setup(x => x.IsNumberExists(It.Is<OrderNumber>(x => x == request.Number)))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(_url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        var result = await response.Content.ReadAsStringAsync();

        result.Should().Be(nameof(OrderNumberAlreadyExistsException));

        _orderRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task CreateOrder_NumberDoesNotExistAndCustomerDoesNotExist_ReturnsStatusCodeUnprocessableEntity()
    {
        // Arrange
        var request = _order;

        _orderRepositoryMock
            .Setup(x => x.IsNumberExists(It.Is<OrderNumber>(x => x == request.Number)))
            .ReturnsAsync(false)
            .Verifiable();

        _orderRepositoryMock
            .Setup(x => x.IsCustomerExists(It.Is<CustomerID>(x => x == request.CustomerID)))
            .ReturnsAsync(false)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(_url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        var result = await response.Content.ReadAsStringAsync();

        result.Should().Be(nameof(CustomerDoesNotExistsException));

        _orderRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task CreateOrder_NumberDoesNotExistAndCustomerExistsAndFilmRecipeDoesNotExist_ReturnsStatusCodeUnprocessableEntity()
    {
        // Arrange
        var request = _order;

        _orderRepositoryMock
            .Setup(x => x.IsNumberExists(It.Is<OrderNumber>(x => x == request.Number)))
            .ReturnsAsync(false)
            .Verifiable();

        _orderRepositoryMock
            .Setup(x => x.IsCustomerExists(It.Is<CustomerID>(x => x == request.CustomerID)))
            .ReturnsAsync(true)
            .Verifiable();

        _orderRepositoryMock
            .Setup(x => x.IsFilmRecipeExists(It.Is<FilmRecipeID>(x => x == request.FilmRecipeID)))
            .ReturnsAsync(false)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(_url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        var result = await response.Content.ReadAsStringAsync();

        result.Should().Be(nameof(FilmRecipeDoesNotExistsException));

        _orderRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task DeleteOrder_OrderWasDeletedSuccessfully_ReturnsStatusCodeOK()
    {
        // Arrange
        var orderId = Fixture.Create<long>();
        var url = $"{_url}/{orderId}";

        _orderRepositoryMock
            .Setup(x => x.Delete(It.Is<ID>(x => x == orderId)))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var response = await Client.DeleteAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        _orderRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task DeleteOrder_OrderWasNotDeletedSuccessfully_ReturnsStatusCodeNotFound()
    {
        // Arrange
        var orderId = Fixture.Create<long>();
        var url = $"{_url}/{orderId}";

        _orderRepositoryMock
            .Setup(x => x.Delete(It.Is<ID>(x => x == orderId)))
            .ReturnsAsync(false)
            .Verifiable();

        // Act
        var response = await Client.DeleteAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        _orderRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task GetOrder_OrderExists_ReturnsStatusCodeOKAndCustomer()
    {
        // Arrange
        var orderId = Fixture.Create<long>();
        var url = $"{_url}/{orderId}";

        var order = Fixture.Create<OrderDTO>();

        _orderRepositoryMock
            .Setup(x => x.Get(It.Is<ID>(x => x == orderId)))
            .ReturnsAsync(order)
            .Verifiable();

        // Act
        var response = await Client.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadAsAsync<OrderDTO>();

        result.Should().Be(order);

        _orderRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task GetOrder_OrderDoesNotExist_ReturnsStatusCodeNotFound()
    {
        // Arrange
        var orderId = Fixture.Create<long>();
        var url = $"{_url}/{orderId}";

        _orderRepositoryMock
            .Setup(x => x.Get(It.Is<ID>(x => x == orderId)))
            .ReturnsAsync((OrderDTO?)null)
            .Verifiable();

        // Act
        var response = await Client.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        _orderRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task GetOrdersInfo_ReturnsStatusCodeOKAndCustomersInfo()
    {
        // Arrange
        var url = $"{_url}/info";
        var orders = Fixture.CreateMany<OrderInfo>().ToList();

        _orderRepositoryMock
            .Setup(x => x.GetInfos())
            .ReturnsAsync(orders)
            .Verifiable();

        // Act
        var response = await Client.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadAsAsync<IReadOnlyCollection<OrderInfo>>();

        result.Should().ContainInConsecutiveOrder(orders);

        _orderRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task UpdateOrder_OrderExistsAndNumberDoesNotExistAndCustomerExistsAndFilmRecipeExists_ReturnsStatusCodeOKAndOrderID()
    {
        // Arrange
        var orderId = Fixture.Create<long>();
        var url = $"{_url}/{orderId}";

        var request = _newOrder;
        var order = _order;

        _orderRepositoryMock
            .Setup(x => x.Get(It.Is<ID>(x => x == orderId)))
            .ReturnsAsync(order)
            .Verifiable();

        _orderRepositoryMock
            .Setup(x => x.IsNumberExists(It.Is<OrderNumber>(x => x == request.Number)))
            .ReturnsAsync(false)
            .Verifiable();

        _orderRepositoryMock
            .Setup(x => x.IsCustomerExists(It.Is<CustomerID>(x => x == request.CustomerID)))
            .ReturnsAsync(true)
            .Verifiable();

        _orderRepositoryMock
            .Setup(x => x.IsFilmRecipeExists(It.Is<FilmRecipeID>(x => x == request.FilmRecipeID)))
            .ReturnsAsync(true)
            .Verifiable();

        _orderRepositoryMock
            .Setup(x => x.Update(It.Is<ID>(x => x == orderId), It.Is<IOrder>(x => x.Number == request.Number)))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        _orderRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task UpdateOrder_OrderDoesNotExist_ReturnsStatusCodeNotFound()
    {
        // Arrange
        var orderId = Fixture.Create<long>();
        var url = $"{_url}/{orderId}";
        var request = _newOrder;

        _orderRepositoryMock
            .Setup(x => x.Get(It.Is<ID>(x => x == orderId)))
            .ReturnsAsync((OrderDTO?)null)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        _orderRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task UpdateOrder_OrderExistsAndNumberExists_ReturnsStatusCodeUnprocessableEntity()
    {
        // Arrange
        var orderId = Fixture.Create<long>();
        var url = $"{_url}/{orderId}";

        var request = _newOrder;
        var filmType = _order;

        _orderRepositoryMock
            .Setup(x => x.Get(It.Is<ID>(x => x == orderId)))
            .ReturnsAsync(filmType)
            .Verifiable();

        _orderRepositoryMock
            .Setup(x => x.IsNumberExists(It.Is<OrderNumber>(x => x == request.Number)))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        var result = await response.Content.ReadAsStringAsync();

        result.Should().Be(nameof(OrderNumberAlreadyExistsException));

        _orderRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task UpdateOrder_OrderExistsAndNumberDoesNotExistsAndCustomerDoesNotExist_ReturnsStatusCodeUnprocessableEntity()
    {
        // Arrange
        var orderId = Fixture.Create<long>();
        var url = $"{_url}/{orderId}";

        var request = _newOrder;
        var filmType = _order;

        _orderRepositoryMock
            .Setup(x => x.Get(It.Is<ID>(x => x == orderId)))
            .ReturnsAsync(filmType)
            .Verifiable();

        _orderRepositoryMock
            .Setup(x => x.IsNumberExists(It.Is<OrderNumber>(x => x == request.Number)))
            .ReturnsAsync(false)
            .Verifiable();

        _orderRepositoryMock
            .Setup(x => x.IsCustomerExists(It.Is<CustomerID>(x => x == request.CustomerID)))
            .ReturnsAsync(false)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        var result = await response.Content.ReadAsStringAsync();

        result.Should().Be(nameof(CustomerDoesNotExistsException));

        _orderRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task UpdateOrder_OrderExistsAndNumberDoesNotExistsAndCustomerExistsAndFilmRecipeDoesNotExist_ReturnsStatusCodeUnprocessableEntity()
    {
        // Arrange
        var orderId = Fixture.Create<long>();
        var url = $"{_url}/{orderId}";

        var request = _newOrder;
        var filmType = _order;

        _orderRepositoryMock
            .Setup(x => x.Get(It.Is<ID>(x => x == orderId)))
            .ReturnsAsync(filmType)
            .Verifiable();

        _orderRepositoryMock
            .Setup(x => x.IsNumberExists(It.Is<OrderNumber>(x => x == request.Number)))
            .ReturnsAsync(false)
            .Verifiable();

        _orderRepositoryMock
            .Setup(x => x.IsCustomerExists(It.Is<CustomerID>(x => x == request.CustomerID)))
            .ReturnsAsync(true)
            .Verifiable();

        _orderRepositoryMock
            .Setup(x => x.IsFilmRecipeExists(It.Is<FilmRecipeID>(x => x == request.FilmRecipeID)))
            .ReturnsAsync(false)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        var result = await response.Content.ReadAsStringAsync();

        result.Should().Be(nameof(FilmRecipeDoesNotExistsException));

        _orderRepositoryMock.VerifyStrongly();
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
        => services
            .AddScoped(_ => _orderRepositoryMock.Object)
            .AddCoreHelper();
}
