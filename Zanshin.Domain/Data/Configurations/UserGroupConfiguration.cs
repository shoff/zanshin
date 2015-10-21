namespace Zanshin.Domain.Data.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using Zanshin.Domain.Entities.Identity;

    /// <summary>
    /// 
    /// </summary>
    public class UserGroupConfiguration : EntityTypeConfiguration<UserRole>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="UserGroupConfiguration"/> class.
        /// </summary>
        public UserGroupConfiguration()
        {
            HasKey(r => new
                            {
                                r.UserId,
                                r.RoleId
                            }).ToTable("UserGroups");
        }
    }
}