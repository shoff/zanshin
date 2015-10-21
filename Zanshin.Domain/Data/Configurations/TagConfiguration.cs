namespace Zanshin.Domain.Data.Configurations
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration;
    using Zanshin.Domain.Entities;

    /// <summary>
    /// 
    /// </summary>
    public class TagConfiguration : EntityTypeConfiguration<Tag>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagConfiguration"/> class.
        /// </summary>
        public TagConfiguration()
        {
            HasMany(t => t.Topics);
            HasMany(p => p.Posts);
            HasMany(u => u.Users);
            HasMany(f => f.Forums);
            HasMany(c => c.Categories);
            HasMany(a => a.Avatars);
            HasMany(w => w.Websites);

           
            Property(t => t.Text).IsRequired().HasColumnAnnotation(
                IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_TAG_TEXT", 2) { IsUnique = true }));
        }
    }
}