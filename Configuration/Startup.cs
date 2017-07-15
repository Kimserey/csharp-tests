using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Configuration
{
    public interface IMyService { }

    public class MyService : IMyService
    {
        public MyService(Endpoint endpoint)
        { }
    }

    public class Endpoint
    {
        public string Address { get; set; }
        public int Port { get; set; }
        public Test Test { get; set; }
    }

    public class Test
    {
        public string X { get; set; }
    }

    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IMyService>(_ => new MyService(Configuration.GetSection("endpoint").Get<Endpoint>()));
            services.AddMvc();
            services.AddOptions();
            services.Configure<Endpoint>(Configuration.GetSection("endpoint"));

            var port = Configuration.GetSection("endpoint:port").Get<int>();
            var test1 = Configuration.GetSection("endpoint").Get<Endpoint>();
            var test2 = Configuration.GetValue<int>("endpoint:port");
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            app.UseMvc();
        }
    }
}
