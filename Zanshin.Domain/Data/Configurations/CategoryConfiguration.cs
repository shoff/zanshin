namespace Zanshin.Domain.Data.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using Zanshin.Domain.Entities.Forum;

    /// <summary>
    /// 
    /// </summary>
    public class CategoryConfiguration : EntityTypeConfiguration<Category>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryConfiguration" /> class.
        /// </summary>
        public CategoryConfiguration()
        {
            HasMany(p => p.Tags).WithMany(t => t.Categories).Map(mc =>
                {
                    mc.ToTable("CategoryJoinTag");
                });
            HasMany(f => f.Forums);

        }
    }
}