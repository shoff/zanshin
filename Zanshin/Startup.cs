using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Zanshin.Startup))]
namespace Zanshin
{
    using System;
    using NLog;

    public partial class Startup
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Configurations the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        public void Configuration(IAppBuilder app)
        {
            try
            {
                ConfigureAuth(app);
            }
            catch (OverflowException ofe)
            {
                log.Error(ofe.Message);
            }
        }
    }
}
