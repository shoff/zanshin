namespace Zanshin.Domain.Helpers
{
    using System;
    using System.Collections.Generic;

    using Zanshin.Domain.Entities;
    using Zanshin.Domain.Entities.Forum;
    using Zanshin.Domain.Entities.Identity;

    /// <summary>
    /// </summary>
    public sealed class FakeForumData
    {
        private static readonly Random random = new Random((int) DateTime.Now.Ticks);
        private readonly List<User> users;
        private readonly List<Forum> forums;
        private readonly List<Topic> topics;
        private readonly List<Post> posts;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeForumData"/> class.
        /// </summary>
        /// <param name="forumCount">The forum count.</param>
        public FakeForumData(int forumCount = 10)
        {
            this.users = new List<User>();
            this.posts = new List<Post>();
            this.topics = new List<Topic>();
            this.forums = new List<Forum>();

            for (int u = 0; u < 200; u++)
            {
                this.users.Add(DataMaker.Instance.CreateAndFill<User>());
            }

            for (int i = 0; i < forumCount; i++)
            {
                var forum = DataMaker.Instance.CreateAndFill<Forum>();
                forum.ForumId = i;
                this.forums.Add(forum);
            }
            
            int lastTopicId = 0;

            for (int f = 0; f < 10; f++)
            {
                int topicCount = random.Next(10, 1500);

                for (int i = 0; i < topicCount; i++)
                {
                    var topic = DataMaker.Instance.CreateAndFill<Topic>();
                    topic.TopicId = ++lastTopicId;
                    topic.ForumId = f;

                    int randomPostCount = random.Next(1, 20);
                    for (int u = 0; u < randomPostCount; u++)
                    {
                        var post = DataMaker.Instance.CreateAndFill<Post>();
                        post.TopicId = topic.TopicId;
                        //post.ParentPost = u > 0 ? u - 1 : (int?) null;
                        post.UserId = this.users[random.Next(0, 199)].Id;
                        this.posts.Add(post);
                    }
                    
                    this.topics.Add(topic);
                }
            }

        }

        /// <summary>
        /// Gets the forums.
        /// </summary>
        /// <value>
        /// The forums.
        /// </value>
        public IEnumerable<Forum> Forums 
        { 
            get { return this.forums; }
        }

        /// <summary>
        /// Gets the topics.
        /// </summary>
        /// <value>
        /// The topics.
        /// </value>
        public IEnumerable<Topic> Topics
        {
            get { return this.topics; }
        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        public IEnumerable<User> Users
        {
            get { return this.users; }
        }

        /// <summary>
        /// Gets the posts.
        /// </summary>
        /// <value>
        /// The posts.
        /// </value>
        public IEnumerable<Post> Posts
        {
            get { return this.posts; }
        }

        public static User CreateUser()
        {
            User user = DataMaker.Instance.CreateAndFill<User>();
            user.Id = 0;
            user.AvatarId = 1;
            user.Profile = DataMaker.Instance.CreateAndFill<UserProfile>();

            //User user = new User

            //{
            //    AvatarId = avatar.AvatarId,
            //    UserProfileId = profile.UserProfileId,
            //    UserName = (randomUser.FirstName + "_" + randomUser.LastName),
            //    DisplayName = randomUser.DisplayName,
            //    Email = (string)randomUser.Item["user"]["email"],
            //    LastLogin = DateTime.Now.AddDays(-random.Next(10, 120)),
            //    Password = (string)randomUser.Item["user"]["sha256"],
            //    PasswordLastChangedDate = DateTime.Now.AddDays(-random.Next(10, 120)),
            //    UserId = 0,
            //    MaximumDaysBetweenPasswordChange = 180,
            //    Karma = random.Next(0, 5608),
            //};
            return user;
        }
    }
}