namespace Zanshin.Domain.Data.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using Zanshin.Domain.Entities;

    /// <summary>
    /// 
    /// </summary>
    public class WebsiteConfiguration : EntityTypeConfiguration<Website>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebsiteConfiguration"/> class.
        /// </summary>
        public WebsiteConfiguration()
        {
            HasMany(p => p.Tags).WithMany(t => t.Websites).Map(mc =>
                {
                    mc.ToTable("WebsiteJoinTag");
                });    
        }
    }
}