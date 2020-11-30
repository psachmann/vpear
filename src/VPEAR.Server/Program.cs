// <copyright file="Program.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace VPEAR.Server
{
    /// <summary>
    /// Contains the main method for the program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The entrypoint for the progeam.
        /// </summary>
        /// <param name="args">The command line arguments for the program.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .Run();
        }

        /// <summary>
        /// Creates and configures the host for the program.
        /// </summary>
        /// <param name="args">The command line arguments for the host.</param>
        /// <returns>The host to run the server.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseStartup<Startup>();
                });

            return builder;
        }
    }
}
