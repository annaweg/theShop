using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(theShop.WebUI.Startup))]
namespace theShop.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
