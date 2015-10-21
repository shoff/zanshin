namespace Zanshin.WebApi.Services
{
    public interface IUserService
    {
        /// <summary>Updates the post count.</summary>
        /// <param name="userId">The user identifier.</param>
        void UpdatePostCount(int userId);

        /// <summary>Updates the topic count.</summary>
        /// <param name="userId">The user identifier.</param>
        void UpdateTopicCount(int userId);
    }
}