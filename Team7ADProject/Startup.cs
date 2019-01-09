using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Team7ADProject.Startup))]
namespace Team7ADProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
