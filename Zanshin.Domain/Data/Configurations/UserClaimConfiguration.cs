namespace Zanshin.Domain.Data.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using Zanshin.Domain.Entities.Identity;

    /// <summary>
    /// 
    /// </summary>
    public class UserClaimConfiguration : EntityTypeConfiguration<UserClaim>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserClaimConfiguration" /> class.
        /// </summary>
        public UserClaimConfiguration()
        {
            HasRequired(u => u.User);
            ToTable("UserClaims");
        }
    }
}