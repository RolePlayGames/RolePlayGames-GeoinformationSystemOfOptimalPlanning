using GSOP.Infrastructure.DataAccess.Contracts.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace GSOP.Interfaces.API.Test.Integrations.Core.Helpers
{
    /// <summary>
    /// Helps with core API services
    /// </summary>
    internal class CoreHelper : IWebHelper
    {
        private readonly Mock<IMigrator> _migratorMock;

        public CoreHelper()
        {
            _migratorMock = new();

            _migratorMock
                .Setup(x => x.Migrate())
                .Returns(Task.CompletedTask);
        }

        public void AddServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient(x => _migratorMock.Object);
        }
    }
}
