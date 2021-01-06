// <copyright file="Null.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("VPEAR.Server.Test")]

namespace VPEAR.Core
{
    /// <summary>
    /// Dummy class for the <see cref="Result{TSuccess}"/> success or error type.
    /// NEVER instantiate this class, it will throw an <see cref="InvalidOperationException"/>.
    /// </summary>
    public sealed class Null
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Null"/> class.
        /// NEVER instantiate this class, it will throw an <see cref="InvalidOperationException"/>.
        /// </summary>
        public Null()
        {
            throw new InvalidOperationException("Null should never be instantiate.");
        }
    }
}
