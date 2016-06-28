using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CommentSystems.Startup))]

namespace CommentSystems
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}