// <copyright file="IUserService.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Threading.Tasks;
using VPEAR.Core.Wrappers;

namespace VPEAR.Core.Abstractions
{
    public interface IUserService
    {
        Task<Result<Container<GetUserResponse>>> GetAsync(string role = null);

        Task<Result<Null>> PutAsync(string id, PutUserRequest request);

        Task<Result<Null>> DeleteAsync(string id);

        Task<Result<Null>> PostRegisterAsync(PostRegisterRequest request);

        Task<Result<PutLoginResponse>> PutLoginAsync(PutLoginRequest request);
    }
}
