using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GBHS_HospitalProject.Startup))]
namespace GBHS_HospitalProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
