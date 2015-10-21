namespace Zanshin.WebApi.Services
{
    using System.Threading.Tasks;

    public interface IForumService
    {
        /// <summary>
        /// Determines if the requested name is in available and valid
        /// </summary>
        /// <param name="forumName">Name of the forum.</param>
        /// <returns></returns>
        Task<bool> NameValid(string forumName);
    }
}