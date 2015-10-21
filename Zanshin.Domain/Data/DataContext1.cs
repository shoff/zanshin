

namespace Zanshin.Domain.Data
{
    using System.Data.Entity;

    using Zanshin.Domain.Entities;
    using Zanshin.Domain.Entities.Forum;
    using Zanshin.Domain.Entities.Identity;

    public sealed partial class DataContext
    {
        private IDbSet<Forum> forums;
        private IDbSet<Topic> topics;
        private IDbSet<Post> posts;
        private IDbSet<User> users;
        private IDbSet<Group> groups;
        private IDbSet<GroupMessage> groupMessages;
        private IDbSet<Category> categories;
        private IDbSet<Avatar> avatars;
        private IDbSet<LocationCount> locationCounts;
        private IDbSet<GeoLocation> geoLocations;
        private IDbSet<PrivateMessage> privateMessages;
        private IDbSet<Tag> tags;
        private IDbSet<UserProfile> userProfiles;
        private IDbSet<UserLogin> userLogins;
        private IDbSet<UserRole> userGroups;
        private IDbSet<UserClaim> userClaims;
        private IDbSet<Rank> ranks;
        private IDbSet<MessageReadByUser> messageReadByUser;
        private IDbSet<Website> websites;
        private IDbSet<Log> logs;

        /// <summary>
        /// Gets the logs.
        /// </summary>
        /// <value>
        /// The logs.
        /// </value>
        public IDbSet<Log> Logs
        {
            get { return this.logs ?? (this.logs = this.SetEntity<Log>()); }

        }

        /// <summary>
        /// Gets the message read by users.
        /// </summary>
        /// <value>
        /// The message read by users.
        /// </value>
        public IDbSet<MessageReadByUser> MessageReadByUsers
        {
            get { return this.messageReadByUser ?? (this.messageReadByUser = this.SetEntity<MessageReadByUser>()); }

        }

        /// <summary>
        /// Gets the ranks.
        /// </summary>
        /// <value>
        /// The ranks.
        /// </value>
        public IDbSet<Rank> Ranks
        {
            get { return this.ranks ?? (this.ranks = this.SetEntity<Rank>()); }
        }
        
        /// <summary>
        /// Gets the websites.
        /// </summary>
        /// <value>
        /// The websites.
        /// </value>
        public IDbSet<Website> Websites
        {
            get { return this.websites ?? (this.websites = this.SetEntity<Website>()); }
        }

        /// <summary>
        /// Gets the group messages.
        /// </summary>
        /// <value>
        /// The group messages.
        /// </value>
        public IDbSet<GroupMessage> GroupMessages
        {
            get { return this.groupMessages ?? (this.groupMessages = this.SetEntity<GroupMessage>()); }
        }

        /// <summary>
        /// Gets the user claims.
        /// </summary>
        /// <value>
        /// The user claims.
        /// </value>
        public IDbSet<UserClaim> UserClaims
        {
            get { return this.userClaims ?? (this.userClaims = this.SetEntity<UserClaim>()); }
        }

        /// <summary>
        /// Gets the user groups.
        /// </summary>
        /// <value>
        /// The user groups.
        /// </value>
        public IDbSet<UserRole> UserGroups
        {
            get { return this.userGroups ?? (this.userGroups = this.SetEntity<UserRole>()); }
        }

        /// <summary>
        /// Gets the user logins.
        /// </summary>
        /// <value>
        /// The user logins.
        /// </value>
        public IDbSet<UserLogin> UserLogins
        {
            get { return this.userLogins ?? (this.userLogins = this.SetEntity<UserLogin>()); }
        }

        /// <summary>
        /// Gets the user profiles.
        /// </summary>
        /// <value>
        /// The user profiles.
        /// </value>
        public IDbSet<UserProfile> UserProfiles
        {
            get { return this.userProfiles ?? (this.userProfiles = this.SetEntity<UserProfile>()); }
        }

        /// <summary>
        /// Gets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public IDbSet<Tag> Tags
        {
            get { return this.tags ?? (this.tags = this.SetEntity<Tag>()); }
        }

        /// <summary>
        /// Gets the forums.
        /// </summary>
        /// <value>
        /// The forums.
        /// </value>
        public IDbSet<Forum> Forums
        {
            get { return this.forums ?? (this.forums = this.SetEntity<Forum>()); }
        }

        /// <summary>
        /// Gets the topics.
        /// </summary>
        /// <value>
        /// The topics.
        /// </value>
        public IDbSet<Topic> Topics
        {
            get { return this.topics ?? (this.topics = this.SetEntity<Topic>()); }
        }

        /// <summary>
        /// Gets the posts.
        /// </summary>
        /// <value>
        /// The posts.
        /// </value>
        public IDbSet<Post> Posts
        {
            get { return this.posts ?? (this.posts = this.SetEntity<Post>()); }
        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        public IDbSet<User> Users
        {
            get { return this.users ?? (this.users = this.SetEntity<User>()); }
        }

        /// <summary>
        /// Gets the avatars.
        /// </summary>
        /// <value>
        /// The avatars.
        /// </value>
        public IDbSet<Avatar> Avatars
        {
            get { return this.avatars ?? (this.avatars = this.SetEntity<Avatar>()); }
        }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public IDbSet<Group> Groups
        {
            get { return this.groups ?? (this.groups = this.SetEntity<Group>()); }
        }

        /// <summary>
        /// Gets the categories.
        /// </summary>
        /// <value>
        /// The categories.
        /// </value>
        public IDbSet<Category> Categories
        {
            get { return this.categories ?? (this.categories = this.SetEntity<Category>()); }
        }
        /// <summary>
        /// Gets the ip addresses.
        /// </summary>
        /// <value>
        /// The ip addresses.
        /// </value>
        public IDbSet<GeoLocation> GeoLocations
        {
            get { return this.geoLocations ?? (this.geoLocations = this.SetEntity<GeoLocation>()); }
        }

        /// <summary>
        /// Gets the location counts.
        /// </summary>
        /// <value>
        /// The location counts.
        /// </value>
        public IDbSet<LocationCount> LocationCounts
        {
            get { return this.locationCounts ?? (this.locationCounts = this.SetEntity<LocationCount>()); }
        }

        /// <summary>
        /// Gets the private messages.
        /// </summary>
        /// <value>
        /// The private messages.
        /// </value>
        public IDbSet<PrivateMessage> PrivateMessages
        {
            get { return this.privateMessages ?? (this.privateMessages = this.SetEntity<PrivateMessage>()); }
        }
    }
}