namespace Zanshin.Domain.Data.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using Zanshin.Domain.Entities.Forum;

    /// <summary>
    /// 
    /// </summary>
    public class PostConfiguration : EntityTypeConfiguration<Post>  
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostConfiguration" /> class.
        /// </summary>
        public PostConfiguration()
        {
            HasMany(p => p.Tags).WithMany(t => t.Posts).Map(mc =>
            {
                mc.ToTable("PostJoinTag");
            });
        }
    }
}