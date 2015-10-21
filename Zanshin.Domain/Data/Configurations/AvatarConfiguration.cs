namespace Zanshin.Domain.Data.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using Zanshin.Domain.Entities;

    /// <summary>
    /// 
    /// </summary>
    public class AvatarConfiguration : EntityTypeConfiguration<Avatar>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarConfiguration"/> class.
        /// </summary>
        public AvatarConfiguration()
        {
            HasMany(p => p.Tags).WithMany(t => t.Avatars).Map(mc =>
                {
                    mc.ToTable("AvatarJoinTag");
                });
        }
    }
}