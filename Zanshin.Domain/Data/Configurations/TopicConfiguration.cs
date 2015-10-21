namespace Zanshin.Domain.Data.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using Zanshin.Domain.Entities;
    using Zanshin.Domain.Entities.Forum;

    /// <summary>
    /// 
    /// </summary>
    public class TopicConfiguration : EntityTypeConfiguration<Topic>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TopicConfiguration" /> class.
        /// </summary>
        public TopicConfiguration()
        {
            HasMany(p => p.Posts);

            HasMany(p => p.Tags).WithMany(t => t.Topics).Map(mc =>
                {
                    mc.ToTable("TopicJoinTag");
                });
        }
    }
}