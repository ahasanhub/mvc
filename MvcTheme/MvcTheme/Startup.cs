using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcTheme.Startup))]
namespace MvcTheme
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
