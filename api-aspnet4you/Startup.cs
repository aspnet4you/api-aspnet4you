using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(api.aspnet4you.mvc5.Startup))]
namespace api.aspnet4you.mvc5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}