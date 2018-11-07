using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ThePriceBot.Startup))]

namespace ThePriceBot
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}