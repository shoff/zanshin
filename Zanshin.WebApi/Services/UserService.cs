#pragma warning disable 1591

namespace Zanshin.WebApi.Services
{
    using Zanshin.Domain.Entities.Identity;
    using Zanshin.Domain.Repositories.Interfaces;

    public class UserService : IUserService
    {
        private readonly IEntityRepository<User, int> userRepository;

        /// <summary>Initializes a new instance of the <see cref="UserService"/> class.</summary>
        /// <param name="userRepository">The user repository.</param>
        public UserService(IEntityRepository<User, int> userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>Updates the post count.</summary>
        /// <param name="userId">The user identifier.</param>
        public void UpdatePostCount(int userId)
        {
            var user = this.userRepository.GetById(userId);
            user.PostCount ++;
            this.userRepository.Context.Commit();
        }

        /// <summary>Updates the topic count.</summary>
        /// <param name="userId">The user identifier.</param>
        public void UpdateTopicCount(int userId)
        {
            var user = this.userRepository.GetById(userId);
            user.TopicCount++;
            this.userRepository.Context.Commit();
        }
    }
}
#pragma warning restore 1591
