namespace Zanshin.Domain.Data.Configurations
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration;
    using Zanshin.Domain.Entities.Identity;

    /// <summary>
    /// 
    /// </summary>
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserConfiguration" /> class.
        /// </summary>
        public UserConfiguration()
        {
            HasMany(p => p.Tags).WithMany(t => t.Users)
                .Map(mc =>
                {
                    mc.ToTable("UserJoinTag");
                    mc.MapLeftKey("Id");
                    mc.MapRightKey("TagId");
                });


            // users
            HasMany(pm => pm.Messages).WithRequired(u=>u.FromUser);

            // HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.RoleId);
            HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);

            Property(u => u.UserName).IsRequired().HasMaxLength(256).HasColumnAnnotation("Index",
               new IndexAnnotation(new IndexAttribute("UserNameIndex")
               {
                   IsUnique = true
               }));

            Property(u => u.Email).HasMaxLength(256);            
            ToTable("Users");

        }
    }
}