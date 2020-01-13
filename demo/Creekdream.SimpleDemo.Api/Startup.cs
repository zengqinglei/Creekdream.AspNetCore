using Creekdream.AspNetCore;
using Creekdream.Orm;
using Creekdream.Orm.EntityFrameworkCore;
using Creekdream.SimpleDemo.Api.Filters;
using Creekdream.SimpleDemo.Api.Middlewares;
using Creekdream.SimpleDemo.EntityFrameworkCore;
using Creekdream.SimpleDemo.Migrations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;

namespace Creekdream.SimpleDemo.Api
{
    /// <inheritdoc />
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <inheritdoc />
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(
                options =>
                {
                    options.Filters.Add(typeof(CustomExceptionFilter));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.AddHealthChecks();

            services.AddDbContext<DbContextBase, SimpleDemoDbContext>(
                options =>
                {
                    options.UseSqlServer(_configuration.GetConnectionString("Default"));
                });

            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "简单示例项目API" });
                    var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    options.IncludeXmlComments(Path.Combine(baseDirectory, $"Creekdream.SimpleDemo.Application.xml"));
                    options.IncludeXmlComments(Path.Combine(baseDirectory, $"Creekdream.SimpleDemo.Api.xml"));
                });

            services.AddCreekdream(
                options =>
                {
                    options.UseEfCore();
                    options.AddSimpleDemoCore();
                    options.AddSimpleDemoApplication();
                });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        public void Configure(IApplicationBuilder app)
        {
            app.UseCreekdream();

            SeedData.Initialize(app.ApplicationServices).Wait();

            if (_webHostEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseCustomRewriter();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(
                c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "简单示例项目API");
                });

            app.UseRouting();
            app.UseCors();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}


