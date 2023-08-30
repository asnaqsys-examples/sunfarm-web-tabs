using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ASNA.QSys.Expo.Model;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Http.Features;

namespace SunFarmSite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        void ConfigureMonaServer(IServiceCollection services, TimeSpan IdleTimeout)
        {
            MonaServerConfig monaServerConfig = new MonaServerConfig();
            Configuration.GetSection("MonaServer").Bind(monaServerConfig);

            if (string.Compare(monaServerConfig.HostName, "*InProcess", true) == 0)
            {
                if (monaServerConfig.JobIdleTimeout > 0)
                    IdleTimeout = TimeSpan.FromMinutes(monaServerConfig.JobIdleTimeout);
                var assemblies = string.Join(',', monaServerConfig.AssemblyList.ToArray());
                ASNA.QSys.MonaServer.Server.StartService("*LoopBack", monaServerConfig.Port, IdleTimeout, assemblies, (ASNA.QSys.MonaServer.TraceOptions)monaServerConfig.TraceOption, false);
            }

            services.AddSingleton<IMonaServerConfig>(s => monaServerConfig);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            TimeSpan IdleTimeout20 = TimeSpan.FromMinutes(20);

            // AJAX will need to read Request.Body to be read synchronously  
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = 4 * 1024;
            });

            services.ConfigureDisplayPagesOptions(Configuration);
            services.AddSession(options =>
            {
                options.IdleTimeout = IdleTimeout20;
                options.Cookie.IsEssential = true; // make the session cookie Essential -- Problematic for DGPR??
            });

            // Session data needs a store. AddMemoryCache activates a local memory-based store. 
            services.AddMemoryCache();

            services.AddRazorPages(razorOptions =>
            {
                razorOptions.Conventions.AddAreaPageRoute("SunFarmViews", "/CUSTDELIV", "");
            }).AddMvcOptions(mvcOptions =>
            {
                mvcOptions.ValueProviderFactories.Insert(0, new EditedValueProviderFactory());
            });

            ConfigureMonaServer(services, IdleTimeout20);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseSession();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

        }
    }
}
