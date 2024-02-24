using AutoFixture;
using GSOP.Application.Customers;
using GSOP.Domain.Contracts.Customers;
using Moq;

namespace GSOP.Application.Test.Customers
{
    public class CustomerServiceTest
    {
        private static readonly Fixture _fixture = new();

        private readonly Mock<ICustomerFactory> _customerFactoryMock;
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;

        private readonly CustomerService _customerService;

        public CustomerServiceTest()
        {
            _customerFactoryMock = new(MockBehavior.Strict);
            _customerRepositoryMock = new(MockBehavior.Strict);

            _customerService = new(_customerFactoryMock.Object, _customerRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateCustomer()
        {
            // Arrange

            // Act
            _customerService.CreateCustomer

            // Assert
        }
    }
}
