using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcTheme1.Startup))]
namespace MvcTheme1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
