namespace Zanshin.Domain.Data.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using Zanshin.Domain.Entities.Identity;

    /// <summary>
    /// 
    /// </summary>
    public class UserLoginConfiguration : EntityTypeConfiguration<UserLogin>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserLoginConfiguration" /> class.
        /// </summary>
        public UserLoginConfiguration()
        {
            HasKey(l => new
                            {
                                l.LoginProvider,
                                l.ProviderKey,
                                l.UserId
                            }).HasRequired(u => u.User);
            ToTable("UserLogins");
        }
    }
}