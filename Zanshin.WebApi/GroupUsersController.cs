
namespace Zanshin.WebApi
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using Zanshin.Domain.Entities.Forum;
    using Zanshin.Domain.Entities.Identity;
    using Zanshin.Domain.Repositories.Interfaces;

    /// <summary>
    /// </summary>
    [RoutePrefix("api/v1/groups/{groupid}")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class GroupUsersController : ApiController
    {

        private readonly IEntityRepository<Group, int> groupRepository;
        private readonly IEntityRepository<User, int> userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupUsersController" /> class.
        /// </summary>
        /// <param name="groupRepository">The group repository.</param>
        /// <param name="userRepository">The user repository.</param>
        public GroupUsersController(IEntityRepository<Group, int> groupRepository, IEntityRepository<User, int> userRepository)
        {
            this.groupRepository = groupRepository;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Gets the specified group identifier.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns></returns>
        [Route("")]
        public async Task<IHttpActionResult> Get(int groupId)
        {
            Group group = this.groupRepository.GetById(groupId);

            if (group == null)
            {
                return this.NotFound();
            }

            return this.Ok(this.groupRepository.Get(u => u.GroupId == groupId, includeProperties: "Administrators,Members"));
        }


        /// <summary>
        /// Gets the specified group identifier.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns></returns>
        [Route("members")]
        public async Task<IHttpActionResult> GetMembers(int groupId)
        {
            Group group = this.groupRepository.GetById(groupId);

            if (group == null)
            {
                return this.NotFound();
            }

            return this.Ok(this.groupRepository.Get(u => u.GroupId == groupId, includeProperties: "Members"));
        }

        [Route("admins")]
        public async Task<IHttpActionResult> GetAdmins(int groupId)
        {
            Group group = this.groupRepository.GetById(groupId);

            if (group == null)
            {
                return this.NotFound();
            }

            return this.Ok(this.groupRepository.Get(u => u.GroupId == groupId, includeProperties: "Administrators"));
        }

        [Route("all")]
        public async Task<IHttpActionResult> GetUsers(int groupId)
        {
            Group group = this.groupRepository.GetById(groupId);

            if (group == null)
            {
                return this.NotFound();
            }

            return this.Ok(this.groupRepository.Get(u => u.GroupId == groupId, includeProperties: "Administrators,Members"));
        }

        /// <summary>
        /// Posts the admin.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        [Route("admins")]
        public async Task<IHttpActionResult> PostAdmin(int groupId, int userId)
        {
            if (userId == 0)
            {
                return StatusCode(System.Net.HttpStatusCode.NotAcceptable);
            }

            Group group = this.groupRepository.Get(g => g.GroupId == groupId, includeProperties: "Administrators").FirstOrDefault();
            if (group == null)
            {
                return NotFound();
            }

            User adminUser = group.Administrators.FirstOrDefault(x => x.Id == userId);
            
            if (adminUser == null)
            {
                User user = this.userRepository.GetById(userId);
                if (user == null)
                {
                    return StatusCode(System.Net.HttpStatusCode.NotAcceptable);
                }

                group.Administrators.Add(user);
                this.groupRepository.Context.Commit();
                return Created(this.Url.ToString() + "admins", group.Administrators);
            }
            return this.BadRequest("User already in admins for group");
        }

    }
}