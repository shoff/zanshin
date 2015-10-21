namespace Zanshin.WebApi.Services
{
    using System;
    using System.Threading.Tasks;
    using Zanshin.Domain.Entities.Forum;
    using Zanshin.Domain.Repositories.Interfaces;

    /// <summary>
    /// </summary>
    public class ForumService : IForumService
    {
        private readonly IEntityRepository<Forum, int> forumRepository;
        private readonly IEntityRepository<Topic, int> topicRepository;
        private readonly IEntityRepository<Post, int> postRepository;

        /// <summary>Initializes a new instance of the <see cref="ForumService" /> class.</summary>
        /// <param name="forumRepository">The forum repository.</param>
        /// <param name="topicRepository">The topic repository.</param>
        /// <param name="postRepository">The post repository.</param>
        public ForumService(IEntityRepository<Forum, int> forumRepository, 
            IEntityRepository<Topic, int> topicRepository, IEntityRepository<Post, int> postRepository)
        {
            this.forumRepository = forumRepository;
            this.topicRepository = topicRepository;
            this.postRepository = postRepository;
        }

        /// <summary>
        /// Determines if the requested name is in available and valid
        /// </summary>
        /// <param name="forumName">Name of the forum.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">forumName</exception>
        public async Task<bool> NameValid(string forumName)
        {
            if (string.IsNullOrWhiteSpace(forumName))
            {
                throw new ArgumentNullException("forumName");
            }
            var found = await this.forumRepository.FindAsync(x => x.Name == forumName);
            return found != null;
        }


    }
}