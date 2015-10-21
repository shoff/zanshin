namespace Zanshin.Tests.Domain.Providers
{
    using Moq;

    using NUnit.Framework;

    using Zanshin.Domain.Data.Interfaces;
    using Zanshin.Domain.Entities.Forum;
    using Zanshin.Domain.Entities.Identity;
    using Zanshin.Domain.Providers;

    [TestFixture]
    public class UserStoreProviderTests
    {
        private Mock<IDataContext> dataContextMock;
        private Mock<IEntityStore<User>> userStoreMock;
        private Mock<IEntityStore<Group>> groupStoreMock;
        private UserStoreProvider<User, int> userStoreProvider;

        public void SetUp()
        {
            //this.dataContextMock = new Mock<IDataContext>();
            //this.userStoreMock = new Mock<IEntityStore<User>>();
            //this.groupStoreMock = new Mock<IEntityStore<Group>>();
            //this.userStoreProvider = new UserStoreProvider<User, int>
            //    (this.dataContextMock.Object, this.userStoreMock.Object);
        }
    }
}