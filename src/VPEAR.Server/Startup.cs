// <copyright file="Startup.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Data;
using VPEAR.Server.Filters;
using VPEAR.Server.Internals;
using static VPEAR.Server.Constants;

namespace VPEAR.Server
{
    /// <summary>
    /// Contains the configuration and startup process for the program.
    /// </summary>
    public class Startup
    {
        public Startup()
        {
            Configuration.EnsureLoaded(Environment.GetCommandLineArgs());
        }

        internal static Configuration? Config { get; set; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application to configure.</param>
        /// <param name="env">The environment to configure.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
#if DEBUG
            env.EnvironmentName = "Development";
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VPEAR.Server v1"));
#else
            env.EnvironmentName = "Production";
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
            builder.RegisterModule(new EventDetectorModule());
            builder.RegisterModule(new JobModule());
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

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(GlobalExceptionFilter));
            })
                .AddFluentValidation()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var status = HttpStatusCode.BadRequest;
                        var messages = new List<string>();

                        context.ModelState.ToList().ForEach(keyValuePair =>
                        {
                            var key = keyValuePair.Key;
                            var errors = new StringBuilder();

                            keyValuePair.Value.Errors.ToList().ForEach(error =>
                            {
                                errors.AppendJoin(' ', error.ErrorMessage);
                            });

                            messages.Add($"{key}: {errors}");
                        });

                        return new JsonResult(new ErrorResponse(status, messages));
                    };
                });

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<VPEARDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,

                        // TODO: read more about audience and issuer
                        // ValidAudience = Configuration["JWT:ValidAudience"],
                        // ValidIssuer = Configuration["JWT:ValidIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config!.Secret)),
                    };
                });

            services.AddHttpClient();

            this.ConfigureDatabase(services);
            this.ConfigureQuartz(services);
            this.ConfigureSwagger(services);
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
#if DEBUG
            services.AddDbContext<VPEARDbContext>(options =>
            {
                options.UseInMemoryDatabase(Schemas.DbSchema);
                options.EnableSensitiveDataLogging();
            });
#else
            services.AddDbContextPool<VPEARDbContext>(builder =>
            {
                builder.UseMySql(
                    Program.Configuration.DbConnection,
                    new MySqlServerVersion(new Version(Program.Configuration.DbVersion)),
                    options =>
                    {
                        options.CharSetBehavior(CharSetBehavior.NeverAppend);
                    });
            });
#endif
        }

        private void ConfigureQuartz(IServiceCollection services)
        {
            services.AddQuartz(options =>
            {
                options.SchedulerName = "Quartz Scheduler";
                options.UseMicrosoftDependencyInjectionScopedJobFactory();
            });

            services.AddQuartzServer(options =>
            {
                options.WaitForJobsToComplete = true;
            });
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "VPEAR API",
                    Version = "v1",
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                options.EnableAnnotations();
            });
        }
    }
}
