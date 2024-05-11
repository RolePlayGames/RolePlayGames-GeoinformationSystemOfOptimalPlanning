using AutoFixture;
using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.Customers;
using GSOP.Domain.Contracts.Customers.Exceptions;
using GSOP.Domain.Contracts.Customers.Models;
using GSOP.Domain.Customers;

namespace GSOP.Domain.Test.Customers;

public class CustomerFactoryTest
{
    private static readonly Fixture _fixture = new();
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly CustomerFactory _customerFactory;

    public CustomerFactoryTest()
    {
        _customerRepositoryMock = new(MockBehavior.Strict);
        _customerFactory = new(_customerRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateCustomer_ById_CustomerExists_CreatesCutomerFromRepository()
    {
        // Arrange
        var id = _fixture.Create<ID>();
        var customerDTO = new CustomerDTO { Name = "Alexander" };
        var customerName = new CustomerName(customerDTO.Name);

        _customerRepositoryMock
            .Setup(x => x.GetCustomer(id))
            .ReturnsAsync(customerDTO)
            .Verifiable();

        // Act
        var customer = await _customerFactory.CreateCustomer(id);

        // Assert
        customer.Name.Should().Be(customerName);

        _customerRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task CreateCustomer_ById_CustomerDoesNotExist_ThrowsCustomerWasNotFoundException()
    {
        // Arrange
        var id = _fixture.Create<ID>();

        _customerRepositoryMock
            .Setup(x => x.GetCustomer(id))
            .ReturnsAsync((CustomerDTO?)null)
            .Verifiable();

        // Act & Assert
        var action = async () => await _customerFactory.CreateCustomer(id);

        await action.Should().ThrowAsync<CustomerWasNotFoundException>();

        _customerRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task CreateCustomer_ByDTO_CustomerNameDoesNotExist_CreatesNewCutomer()
    {
        // Arrange
        var customerDTO = new CustomerDTO { Name = "Alexander" };
        var customerName = new CustomerName(customerDTO.Name);

        _customerRepositoryMock
            .Setup(x => x.IsCustomerNameExsits(customerName))
            .ReturnsAsync(false)
            .Verifiable();

        // Act
        var customer = await _customerFactory.CreateCustomer(customerDTO);

        // Assert
        customer.Name.Should().Be(customerName);

        _customerRepositoryMock.VerifyStrongly();
    }

    [Fact]
    public async Task CreateCustomer_ByDTO_CustomerNameExists_ThrowsCustomerNameAlreadyExistsException()
    {
        // Arrange
        var customerDTO = new CustomerDTO { Name = "Alexander" };
        var customerName = new CustomerName(customerDTO.Name);

        _customerRepositoryMock
            .Setup(x => x.IsCustomerNameExsits(customerName))
            .ReturnsAsync(true)
            .Verifiable();

        // Act & Assert
        var action = async () => await _customerFactory.CreateCustomer(customerDTO);

        await action.Should().ThrowAsync<CustomerNameAlreadyExistsException>();

        _customerRepositoryMock.VerifyStrongly();
    }
}
