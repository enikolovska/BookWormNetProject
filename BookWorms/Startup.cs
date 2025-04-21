using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BookWorms.Startup))]
namespace BookWorms
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
