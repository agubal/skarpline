using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using Skarpline.Api;
using Skarpline.Api.DependencyResolution;
using Skarpline.Api.Filter;
using Skarpline.Dependencies;
using StructureMap;

[assembly: OwinStartup(typeof(Startup))]
namespace Skarpline.Api
{
    
    public partial class Startup
    {
        private readonly IContainer _container;

        public Startup()
        {
            //Initialize Inversion of Controle container
            _container = IoC.Initialize();
        }

        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration
            {
                DependencyResolver = new StructureMapWebApiDependencyResolver(_container),
                Filters = { new ChatErrorHandler() }
            };
            //Register routing:
            WebApiConfig.Register(config);

            //Allow Cors:
            app.UseCors(CorsOptions.AllowAll);

            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);
                map.RunSignalR();
            });


            //Add SignalR
//            app.MapSignalR();

            //Configure authorization logic:
            ConfigureOAuth(app);
            app.UseWebApi(config);
        }
    }
}