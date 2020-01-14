using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using WooliesXTest.Api.Handlers;
using WooliesXTest.Data.Configs;
using WooliesXTest.Services.Factory;
using WooliesXTest.Services.Interfaces;
using WooliesXTest.Services.Services;
using WooliesXTest.Services.Services.ProductSort;

namespace WooliesXTest.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // bind configs
            services.Configure<ApiConfig>(Configuration.GetSection("apiConfig"));

            // register services
            services.AddSingleton<IProductsService, ProductsService>();
            services.AddSingleton<IProductSortFactory, ProductSortFactory>();
            services.AddSingleton<IProductSortService, AscendingSortService>();
            services.AddSingleton<IProductSortService, DescendingSortService>();
            services.AddSingleton<IProductSortService, HighSortService>();
            services.AddSingleton<IProductSortService, LowSortService>();
            services.AddSingleton<IProductSortService, RecommendedSortService>();
            services.AddSingleton<ITrolleyService, TrolleyService>();

            // register http clients
            services.AddTransient<TokenQueryHandler>();
            var config = Configuration.GetSection("apiConfig").Get<ApiConfig>();

            services.AddHttpClient<IProductsService, ProductsService>(c =>
            {
                c.BaseAddress = new Uri(config.ResourceUrl);
            }).AddHttpMessageHandler<TokenQueryHandler>();

            services.AddHttpClient<IShopperHistoryService, ShopperHistoryService>(c =>
            {
                c.BaseAddress = new Uri(config.ResourceUrl);
            }).AddHttpMessageHandler<TokenQueryHandler>();

            // setup ExternalTrolleyService here in order to test that one

            //services.AddHttpClient<ITrolleyService, ExternalTrolleyService>(c =>
            //{
            //    c.BaseAddress = new Uri(config.ResourceUrl);
            //}).AddHttpMessageHandler<TokenQueryHandler>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
