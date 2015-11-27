﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Framework.Caching.Redis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNet.Session;
using MVC6RedisSessionAndCache.Web.Helpers;


namespace MVC6RedisSessionAndCache.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //This can be injected to any service, controller...
            services.Add(
                 new ServiceDescriptor(
                     typeof(Microsoft.Framework.OptionsModel.IOptions<RedisCacheOptions>),
                     Configuration.Get<RedisCacheOptions>("RedisCacheOptions")
                 )
             );

            //At the moment the redis cache implements the wrong interface, that's why the OwnRedisCache
            services.AddSingleton<Microsoft.Extensions.Caching.Distributed.IDistributedCache, OwnRedisCache>();
            services.AddSession(o =>
            {
                o.IdleTimeout = TimeSpan.FromMinutes(15);
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                //app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseIISPlatformHandler();

            app.UseSession();//Before MVC always

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
