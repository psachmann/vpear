// <copyright file="Program.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using VPEAR.Server.Internals;

namespace VPEAR.Server
{
    /// <summary>
    /// Contains the main method for the program.
    /// </summary>
    public class Program
    {
        internal static Configuration Configuration { get; private set; } = new Configuration();

        /// <summary>
        /// The entrypoint for the progeam.
        /// </summary>
        /// <param name="args">The command line arguments for the program.</param>
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args)
                    .Build()
                    .Run();
            }
            catch (Exception exception)
            {
                Log.Error("Message \"{@Error}\"", exception.Message);
                Log.Fatal("Terminated host due an error");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// Creates and configures the host for the program.
        /// </summary>
        /// <param name="args">The command line arguments for the host.</param>
        /// <returns>The host to run the server.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            // TODO: choose the right loglevel
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(LogEventLevel.Debug)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Configuration = ConfigurationHelpers.LoadConfiguration(args);

            var builder = Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseSerilog();
                    builder.UseStartup<Startup>();
                    builder.UseUrls(Configuration.Urls.ToArray());
                });

            return builder;
        }
    }
}
