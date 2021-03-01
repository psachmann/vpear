// <copyright file="Startup.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
using VPEAR.Server.Modules;
using VPEAR.Server.Services;
using static VPEAR.Server.Constants;

namespace VPEAR.Server
{
    /// <summary>
    /// Contains the configuration and startup process for the program.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        internal static IConfiguration? Configuration { get; set; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application to configure.</param>
        /// <param name="env">The environment to configure.</param>
        /// <param name="roles">The role manager to seed roles.</param>
        /// <param name="users">The user manager to seed users.</param>
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            RoleManager<IdentityRole> roles,
            UserManager<IdentityUser> users)
        {
            DataSeed.Seed(roles, users);
#if DEBUG
            env.EnvironmentName = "Development";
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VPEAR.Server v1"));
#else
            env.EnvironmentName = "Production";
            app.UseHsts();
            app.UseHttpsRedirection();
#endif
            app.UseRouting();
            app.UseAuthentication();
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
            builder.RegisterModule(new HandlerModule());
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
                    options.InvalidModelStateResponseFactory = InvalidModelStateResponseFactory;
                });

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = Limits.MinPasswordLength;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("Secret"))),
                    };
                });

            services.AddHttpClient("default", client =>
            {
                client.Timeout = TimeSpan.FromMilliseconds(Defaults.DefaultHttpTimeout);
            });

            services.AddMediatR(typeof(Startup));

            ConfigureDatabase(services);
            ConfigureQuartz(services);
            ConfigureSwagger(services);
        }

        private static void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<VPEARDbContext>(builder =>
            {
                builder.UseMySql(
                    Configuration.GetValue<string>("MariaDb:Connection"),
                    new MySqlServerVersion(new Version(Configuration.GetValue<string>("MariaDb:Version"))),
                    options =>
                    {
                        options.CharSetBehavior(CharSetBehavior.NeverAppend);
                    });
            });
        }

        private static void ConfigureQuartz(IServiceCollection services)
        {
            services.AddQuartz(options =>
            {
                options.UseMicrosoftDependencyInjectionScopedJobFactory();
            });

            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = false;
            });
        }

        private static void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "VPEAR API",
                    Version = "v1",
                });

                options.AddSecurityDefinition(
                    JwtBearerDefaults.AuthenticationScheme,
                    new OpenApiSecurityScheme()
                    {
                        Description = "Please enter into field the word 'Bearer' following by space and JWT.",
                        In = ParameterLocation.Header,
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                    });

                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer",
                                },
                            },
                            new List<string>()
                        },
                    });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                options.EnableAnnotations();
            });
        }

        private static IActionResult InvalidModelStateResponseFactory(ActionContext context)
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

            context.HttpContext.Response.StatusCode = (int)status;

            return new JsonResult(new ErrorResponse(status, messages));
        }
    }
}
