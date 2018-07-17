using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QueryRunner.Startup))]
namespace QueryRunner
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}