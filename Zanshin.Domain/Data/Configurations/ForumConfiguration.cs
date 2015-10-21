namespace Zanshin.Domain.Data.Configurations
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration;
    using Zanshin.Domain.Entities.Forum;

    /// <summary>
    /// 
    /// </summary>
    public class ForumConfiguration : EntityTypeConfiguration<Forum>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForumConfiguration" /> class.
        /// </summary>
        public ForumConfiguration()
        {
            HasMany(t => t.Topics);
            
            Property(t => t.Name).HasColumnAnnotation(IndexAnnotation.AnnotationName,
                new IndexAnnotation(new IndexAttribute("IX_ForumName", 1) { IsUnique = true }));

            HasMany(p => p.Tags).WithMany(t => t.Forums).Map(mc =>
                {
                    mc.ToTable("ForumJoinTag");
                });
        }
    }
}