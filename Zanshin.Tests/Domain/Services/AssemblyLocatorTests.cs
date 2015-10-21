namespace Zanshin.Tests.Domain.Services
{
    using NUnit.Framework;

    using Zanshin.Domain.Services;

    [TestFixture]
    public class AssemblyLocatorTests
    {

        private AssemblyLocator assemblyLocator;

        [SetUp]
        public void SetUp()
        {
            this.assemblyLocator = new AssemblyLocator();
        }


        [Test]
        public void LoadAssemblies_Should_Contain_Relevant_Entries()
        {
            this.assemblyLocator.LoadAssemblies();
            var collection = this.assemblyLocator.AllAssemblies;
        }
    }
}