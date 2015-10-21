namespace Zanshin.Domain.Data.Interfaces
{
    using System.Data.Entity;

    using Zanshin.Domain.Entities;
    using Zanshin.Domain.Entities.Forum;
    using Zanshin.Domain.Entities.Identity;

    /// <summary>
    /// </summary>
    public interface IDataContext : IQueryableDataContext
    {
        /// <summary>
        /// Tracks the changes.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        void TrackChanges(bool value);

        /// <summary>
        /// Gets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        IDbSet<Tag>Tags { get; }
        
        /// <summary>
        /// Gets the forums.
        /// </summary>
        /// <value>
        /// The forums.
        /// </value>
        IDbSet<Forum> Forums { get; }

        /// <summary>
        /// Gets the topics.
        /// </summary>
        /// <value>
        /// The topics.
        /// </value>
        IDbSet<Topic> Topics { get; }

        /// <summary>
        /// Gets the posts.
        /// </summary>
        /// <value>
        /// The posts.
        /// </value>
        IDbSet<Post> Posts { get; }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        IDbSet<User> Users { get; }

        /// <summary>
        /// Gets the avatars.
        /// </summary>
        /// <value>
        /// The avatars.
        /// </value>
        IDbSet<Avatar> Avatars { get; }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        IDbSet<Group> Groups { get; }

        /// <summary>
        /// Gets the categories.
        /// </summary>
        /// <value>
        /// The categories.
        /// </value>
        IDbSet<Category> Categories { get; }

        /// <summary>
        /// Gets the ip addresses.
        /// </summary>
        /// <value>
        /// The ip addresses.
        /// </value>
        IDbSet<GeoLocation> GeoLocations { get; }

        /// <summary>
        /// Gets the location counts.
        /// </summary>
        /// <value>
        /// The location counts.
        /// </value>
        IDbSet<LocationCount> LocationCounts { get; }

        /// <summary>
        /// Gets the private messages.
        /// </summary>
        /// <value>
        /// The private messages.
        /// </value>
        IDbSet<PrivateMessage> PrivateMessages { get; }
    }
}