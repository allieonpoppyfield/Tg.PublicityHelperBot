using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Tg.PublicityHelperBot.Infrastructure;
using Tg.PublicityHelperBot.Services;

namespace Tg.PublicityHelperBot
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUpdateService, UpdateService>();
            services.AddScoped<IBotService, BotService>();
            services.AddSingleton<IChatCollectionService, ChatCollectionService>();

            services.Configure<BotConfiguration>(Configuration.GetSection("BotConfiguration"));
            services
                .AddControllers()
                .AddNewtonsoftJson();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
