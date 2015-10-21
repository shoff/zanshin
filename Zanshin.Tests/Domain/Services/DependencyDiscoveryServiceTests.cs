namespace Zanshin.Tests.Domain.Services
{
    using Moq;

    using NUnit.Framework;

    using Zanshin.Domain.Helpers.Interfaces;
    using Zanshin.Domain.Services;

    [TestFixture]
    public class DependencyDiscoveryServiceTests
    {
        private Mock<IConfigurationWrapper> configurationWrapperMock;
        private AssemblyDiscoveryService assemblyDiscoveryService;


        [SetUp]
        public void SetUp()
        {
            this.configurationWrapperMock = new Mock<IConfigurationWrapper>();
            this.assemblyDiscoveryService = new AssemblyDiscoveryService(this.configurationWrapperMock.Object);
        }

    }
}