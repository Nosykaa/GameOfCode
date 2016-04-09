using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GameOfCode2016.Startup))]
namespace GameOfCode2016
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
