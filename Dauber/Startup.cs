using Microsoft.Owin;
using Owin;
using System.Configuration;
using System.Data.SqlClient;

[assembly: OwinStartupAttribute(typeof(Dauber.Startup))]
namespace Dauber
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            SqlDependency.Start(ConfigurationManager.ConnectionStrings["DauberDB"].ConnectionString);
        }
    }
}
