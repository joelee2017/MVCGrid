using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCGrid.Startup))]
namespace MVCGrid
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
