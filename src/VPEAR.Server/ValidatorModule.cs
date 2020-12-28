// <copyright file="ValidatorModule.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;

namespace VPEAR.Server
{
    /// <summary>
    /// Encapsulates the Autofac validator registration in a single module.
    /// </summary>
    public class ValidatorModule : Module
    {
        /// <summary>
        /// Register all validators with the Autofac container.
        /// </summary>
        /// <param name="builder">The container builder to build the Autofac container.</param>
        protected override void Load(ContainerBuilder builder)
        {
        }
    }
}
