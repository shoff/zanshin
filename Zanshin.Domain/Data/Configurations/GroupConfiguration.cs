namespace Zanshin.Domain.Data.Configurations
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration;
    using Zanshin.Domain.Entities.Forum;
    using Zanshin.Domain.Entities.Identity;

    /// <summary>
    /// </summary>
    public class GroupConfiguration : EntityTypeConfiguration<Group>  
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupConfiguration"/> class.
        /// </summary>
        public GroupConfiguration()
        {

            Property(r => r.GroupName).HasColumnAnnotation("Index",
                new IndexAnnotation(new IndexAttribute("NameIndex")
                {
                    IsUnique = true
                }));


           // HasMany(r => r.Administrators).WithMany(g => g.Groups);
            //    .Map(m => 
            //{
            //    m.MapLeftKey("UserId");
            //    m.MapRightKey("GroupId");
            //    m.ToTable("GroupAdmins");
            //});

            //HasMany(r => r.Members).WithMany(g => g.Groups);
            //    .Map(m =>
            //{
            //    m.MapLeftKey("UserId");
            //    m.MapRightKey("GroupId");
            //    m.ToTable("GroupMembers");
            //});
            
            ToTable("Groups");
        }
    }
}