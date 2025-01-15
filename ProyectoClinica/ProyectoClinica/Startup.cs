using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProyectoClinica.Startup))]
namespace ProyectoClinica
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
