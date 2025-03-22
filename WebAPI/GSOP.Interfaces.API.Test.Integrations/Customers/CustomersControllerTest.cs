using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.Customers;
using GSOP.Domain.Contracts.Customers.Exceptions;
using GSOP.Domain.Contracts.Customers.Models;
using GSOP.Interfaces.API.Test.Integrations.Core;
using GSOP.Interfaces.API.Test.Integrations.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace GSOP.Interfaces.API.Test.Integrations.Customers;

public class CustomersControllerTest : WebIntegrationTestBase
{
    private const string _url = "/api/customers";

    private readonly Mock<ICustomerRepository> _customerRepositoryMock;

    public CustomersControllerTest()
    {
        _customerRepositoryMock = new(MockBehavior.Strict);
    }

    [Fact]
    public async Task CreateCustomer_NameDoesNotExist_ReturnsStatusCodeOKAndCustomerId()
    {
        // Arrange
        var request = new CustomerDTO { Name = "Klockner Pentaplast Internal", Coordinates = null };

        _customerRepositoryMock
            .Setup(x => x.IsCustomerNameExsits(It.Is<CustomerName>(x => x == request.Name)))
            .ReturnsAsync(false)
            .Verifiable();

        var customerId = Fixture.Create<long>();

        _customerRepositoryMock
            .Setup(x => x.CreateCustomer(It.Is<ICustomer>(x => x.Name == request.Name)))
            .ReturnsAsync(customerId)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(_url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadAsAsync<long>();

        result.Should().Be(customerId);

        _customerRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task CreateCustomer_NameExists_ReturnsStatusCodeUnprocessableEntity()
    {
        // Arrange
        var request = new CustomerDTO { Name = "Klockner Pentaplast Internal", Coordinates = null };

        _customerRepositoryMock
            .Setup(x => x.IsCustomerNameExsits(It.Is<CustomerName>(x => x == request.Name)))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(_url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        var result = await response.Content.ReadAsStringAsync();

        result.Should().Be(nameof(CustomerNameAlreadyExistsException));

        _customerRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task DeleteCustomer_CustomerWasDeletedSuccessfully_ReturnsStatusCodeOK()
    {
        // Arrange
        var customerId = Fixture.Create<long>();
        var url = $"{_url}/{customerId}";

        _customerRepositoryMock
            .Setup(x => x.DeleteCustomer(It.Is<ID>(x => x == customerId)))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var response = await Client.DeleteAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        _customerRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task DeleteCustomer_CustomerWasNotDeletedSuccessfully_ReturnsStatusCodeNotFound()
    {
        // Arrange
        var customerId = Fixture.Create<long>();
        var url = $"{_url}/{customerId}";

        _customerRepositoryMock
            .Setup(x => x.DeleteCustomer(It.Is<ID>(x => x == customerId)))
            .ReturnsAsync(false)
            .Verifiable();

        // Act
        var response = await Client.DeleteAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        _customerRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task GetCustomer_CustomerExists_ReturnsStatusCodeOKAndCustomer()
    {
        // Arrange
        var customerId = Fixture.Create<long>();
        var url = $"{_url}/{customerId}";

        var customer = Fixture.Create<CustomerDTO>();

        _customerRepositoryMock
            .Setup(x => x.GetCustomer(It.Is<ID>(x => x == customerId)))
            .ReturnsAsync(customer)
            .Verifiable();

        // Act
        var response = await Client.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadAsAsync<CustomerDTO>();

        result.Should().Be(customer);

        _customerRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task GetCustomer_CustomerDoesNotExist_ReturnsStatusCodeNotFound()
    {
        // Arrange
        var customerId = Fixture.Create<long>();
        var url = $"{_url}/{customerId}";

        _customerRepositoryMock
            .Setup(x => x.GetCustomer(It.Is<ID>(x => x == customerId)))
            .ReturnsAsync((CustomerDTO?)null)
            .Verifiable();

        // Act
        var response = await Client.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        _customerRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task GetCustomersInfo_ReturnsStatusCodeOKAndCustomersInfo()
    {
        // Arrange
        var url = $"{_url}/info";
        var customers = Fixture.CreateMany<CustomerInfo>().ToList();

        _customerRepositoryMock
            .Setup(x => x.GetCustomersInfo())
            .ReturnsAsync(customers)
            .Verifiable();

        // Act
        var response = await Client.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadAsAsync<IReadOnlyCollection<CustomerInfo>>();

        result.Should().ContainInConsecutiveOrder(customers);

        _customerRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task UpdateCustomer_CustomerExistsAndNameDoesNotExist_ReturnsStatusCodeOKAndCustomerId()
    {
        // Arrange
        var customerId = Fixture.Create<long>();
        var url = $"{_url}/{customerId}";
        var request = new CustomerDTO { Name = "Klockner Pentaplast Internal", Coordinates = null };

        var customer = Fixture.Create<CustomerDTO>();

        _customerRepositoryMock
            .Setup(x => x.GetCustomer(It.Is<ID>(x => x == customerId)))
            .ReturnsAsync(customer)
            .Verifiable();

        _customerRepositoryMock
            .Setup(x => x.IsCustomerNameExsits(It.Is<CustomerName>(x => x == request.Name)))
            .ReturnsAsync(false)
            .Verifiable();

        _customerRepositoryMock
            .Setup(x => x.UpdateCustomer(It.Is<ID>(x => x == customerId), It.Is<ICustomer>(x => x.Name == request.Name)))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        _customerRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task UpdateCustomer_CustomerDoseNotExist_ReturnsStatusCodeNotFound()
    {
        // Arrange
        var customerId = Fixture.Create<long>();
        var url = $"{_url}/{customerId}";
        var request = Fixture.Create<CustomerDTO>();

        _customerRepositoryMock
            .Setup(x => x.GetCustomer(It.Is<ID>(x => x == customerId)))
            .ReturnsAsync((CustomerDTO?)null)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        _customerRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task UpdateCustomer_CustomerExistsAndNameExists_ReturnsStatusCodeUnprocessableEntity()
    {
        // Arrange
        var customerId = Fixture.Create<long>();
        var url = $"{_url}/{customerId}";
        var request = new CustomerDTO { Name = "Klockner Pentaplast Internal", Coordinates = null };

        var customer = Fixture.Create<CustomerDTO>();

        _customerRepositoryMock
            .Setup(x => x.GetCustomer(It.Is<ID>(x => x == customerId)))
            .ReturnsAsync(customer)
            .Verifiable();

        _customerRepositoryMock
            .Setup(x => x.IsCustomerNameExsits(It.Is<CustomerName>(x => x == request.Name)))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var response = await Client.PostAsJsonAsync(url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        var result = await response.Content.ReadAsStringAsync();

        result.Should().Be(nameof(CustomerNameAlreadyExistsException));

        _customerRepositoryMock.VerifyStrongly();
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
        => services
            .AddScoped(_ => _customerRepositoryMock.Object)
            .AddCoreHelper();
}
