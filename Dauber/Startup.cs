using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Dauber.Startup))]
namespace Dauber
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
