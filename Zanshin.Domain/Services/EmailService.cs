namespace Zanshin.Domain.Services
{
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;

    public sealed class EmailService : IIdentityMessageService
    {
        /// <summary>
        /// This method should send the message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }
}