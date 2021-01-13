// <copyright file="IUserService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Threading.Tasks;
using VPEAR.Core.Wrappers;

namespace VPEAR.Core.Abstractions
{
    /// <summary>
    /// Service definition and abstraction for dependency
    /// injection and webapi controllers.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Gets all users with the given role or all users if no role is provided.
        /// </summary>
        /// <param name="role">The user role.</param>
        /// <returns>The result, which contains the found users.</returns>
        Task<Result<Container<GetUserResponse>>> GetAsync(string role = null);

        /// <summary>
        /// Updates the given user.
        /// </summary>
        /// <param name="name">The user name.</param>
        /// <param name="request">The request data.</param>
        /// <returns>The result, which indicates the success of the operation.</returns>
        Task<Result<Null>> PutAsync(string name, PutUserRequest request);

        /// <summary>
        /// Deletes the given user.
        /// </summary>
        /// <param name="name">The user name.</param>
        /// <returns>The result, which indicates the success of the operation.</returns>
        Task<Result<Null>> DeleteAsync(string name);

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="request">The request data.</param>
        /// <returns>The result, which indicates the success of the operation.</returns>
        Task<Result<Null>> PostRegisterAsync(PostRegisterRequest request);

        /// <summary>
        /// Generates a new access token for the given user.
        /// </summary>
        /// <param name="request">The request data.</param>
        /// <returns>The result, which contains a new access token and the expires at date.</returns>
        Task<Result<PutLoginResponse>> PutLoginAsync(PutLoginRequest request);
    }
}
