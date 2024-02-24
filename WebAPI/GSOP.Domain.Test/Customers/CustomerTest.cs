using GSOP.Domain.Contracts.Customers;
using GSOP.Domain.Contracts.Customers.Exceptions;
using GSOP.Domain.Contracts.Customers.Models;
using GSOP.Domain.Customers;

namespace GSOP.Domain.Test.Customers
{
    public class CustomerTest
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly CustomerName _customerName;
        private readonly Customer _customer;

        public CustomerTest()
        {
            _customerRepositoryMock = new(MockBehavior.Strict);
            _customerName = new CustomerName("James");
            _customer = new Customer(_customerName, _customerRepositoryMock.Object);
        }

        [Fact]
        public async Task SetName_NewNameDoesNotExist_UpdatesCustomerName()
        {
            // Arrange
            var newName = new CustomerName("Alexander");

            _customerRepositoryMock
                .Setup(x => x.IsCustomerNameExsits(newName))
                .ReturnsAsync(false)
                .Verifiable();

            // Act
            await _customer.SetName(newName);

            // Assert
            _customer.Name.Should().Be(newName);

            _customerRepositoryMock.VerifyStrongly();
        }

        [Fact]
        public async Task SetName_NewNameDoesNotExist_ThrowsCustomerNameAlreadyExistsException()
        {
            // Arrange
            var newName = new CustomerName("Alexander");

            _customerRepositoryMock
                .Setup(x => x.IsCustomerNameExsits(newName))
                .ReturnsAsync(true)
                .Verifiable();

            // Act & Assert
            var action = async () => await _customer.SetName(newName);

            await action.Should().ThrowAsync<CustomerNameAlreadyExistsException>();

            _customer.Name.Should().Be(_customerName);

            _customerRepositoryMock.VerifyStrongly();
        }
    }
}
