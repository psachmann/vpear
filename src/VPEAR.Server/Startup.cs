// <copyright file="Startup.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using VPEAR.Server.Db;

namespace VPEAR.Server
{
    /// <summary>
    /// Contains the configuration and startup process for the program.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application to configure.</param>
        /// <param name="env">The environment to configure.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // TODO: hosting environment is wrong
#if DEBUG
            env.EnvironmentName = "Debug";
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VPEAR.Server v1"));
#else
            env.EnvironmentName = "Release";
            app.UseHttpsRedirection();
#endif
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// Register all services for dependency injection.
        /// </summary>
        /// <param name="builder">Autofac container builder.</param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new ClientModule());
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new ServiceModule());
            builder.RegisterModule(new ValidatorModule());
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Service collection to register the services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            this.ConfigureDatabase(services);
            this.ConfigureSwagger(services);
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
#if DEBUG
            services.AddDbContext<VPEARDbContext>(options =>
            {
                options.UseInMemoryDatabase(Constants.Db.DefaultSchema);
            });
#else
            // TODO: configure for real database
#endif
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "VPEAR API",
                    Version = "v1",
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.EnableAnnotations();
            });
        }
    }
}
