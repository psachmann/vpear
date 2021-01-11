// <copyright file="UserServiceTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using VPEAR.Core.Abstractions;
using VPEAR.Core.Wrappers;
using Xunit;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test.Services
{
    public class UserServiceTest : IClassFixture<AutofacFixture>
    {
        private readonly IUserService service;

        public UserServiceTest(AutofacFixture fixture)
        {
            this.service = fixture.Container.Resolve<IUserService>();

            Configuration.EnsureLoaded(Environment.GetCommandLineArgs());
        }

        [Fact]
        public async Task GetAsync200OKTest()
        {
            var result = await this.service.GetAsync(Roles.AdminRole);

            Assert.NotNull(result.Value);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.InRange(result.Value!.Count, 1L, long.MaxValue);

            result = await this.service.GetAsync(Roles.UserRole);

            Assert.NotNull(result.Value);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.InRange(result.Value!.Count, 1L, long.MaxValue);
        }

        [Theory]
        [InlineData(false, null, null)]
        [InlineData(true, null, null)]
        [InlineData(false, "newPassword", "oldPassword")]
        [InlineData(true, "newPassword", "oldPassword")]
        public async Task PutAsync200OKTest(
            bool isVerified,
            string? newPassword,
            string? oldPassword)
        {
            var request = new PutUserRequest()
            {
                IsVerified = isVerified,
                NewPassword = newPassword,
                OldPassword = oldPassword,
            };
            var result = await this.service.PutAsync(Mocks.User.Name, request);

            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }

        [Fact]
        public async Task PutAsync404NotFoundTest()
        {
            var result = await this.service.PutAsync(Mocks.NotExisting.Id.ToString(), new PutUserRequest());

            Assert.NotNull(result.Error);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.Contains(ErrorMessages.UserNotFound, result.Error!.Messages);
        }

        [Fact]
        public async Task DeleteAsync200OKTest()
        {
            var result = await this.service.DeleteAsync(Mocks.User.Name);

            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }

        [Fact]
        public async Task DeleteAsync403ForbiddenTest()
        {
            var result = await this.service.DeleteAsync(Mocks.Admin.Name);

            Assert.NotNull(result.Error);
            Assert.Equal(StatusCodes.Status403Forbidden, result.StatusCode);
            Assert.Contains(ErrorMessages.LastAdminError, result.Error!.Messages);
        }

        [Fact]
        public async Task DeleteAsync404NotFoundTest()
        {
            var result = await this.service.DeleteAsync(Mocks.NotExisting.Id.ToString());

            Assert.NotNull(result.Error);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.Contains(ErrorMessages.UserNotFound, result.Error!.Messages);
        }

        [Fact]
        public async Task PostRegisterAsync200OKTest()
        {
            var request = new PostRegisterRequest()
            {
                Name = Mocks.UnconfirmedUser,
                IsAdmin = false,
                Password = Mocks.ValidPassword,
            };
            var result = await this.service.PostRegisterAsync(request);

            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

            request = new PostRegisterRequest()
            {
                Name = Mocks.UnconfirmedUser,
                IsAdmin = true,
                Password = Mocks.ValidPassword,
            };
            result = await this.service.PostRegisterAsync(request);

            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task PostRegisterAsync409ConflictTest()
        {
            var request = new PostRegisterRequest()
            {
                Name = Mocks.ConfirmedUser,
                IsAdmin = false,
                Password = Mocks.ValidPassword,
            };
            var result = await this.service.PostRegisterAsync(request);

            Assert.NotNull(result.Error);
            Assert.Equal(StatusCodes.Status409Conflict, result.StatusCode);
            Assert.Contains(ErrorMessages.UserNameAlreadyUsed, result.Error!.Messages);
        }

        [Fact]
        public async Task PutLoginAsync200OKTest()
        {
            var request = new PutLoginRequest()
            {
                Name = Mocks.ConfirmedUser,
                Password = Mocks.ValidPassword,
            };
            var result = await this.service.PutLoginAsync(request);

            Assert.NotNull(result.Value);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task PutLoginAsync403ForbiddenTest()
        {
            var request = new PutLoginRequest()
            {
                Name = Mocks.ConfirmedUser,
                Password = Mocks.InvalidPassword,
            };
            var result = await this.service.PutLoginAsync(request);

            Assert.NotNull(result.Error);
            Assert.Equal(StatusCodes.Status403Forbidden, result.StatusCode);
            Assert.Contains(ErrorMessages.InvalidPassword, result.Error!.Messages);
        }

        [Fact]
        public async Task PutLoginAsync404NotFoundTest()
        {
            var request = new PutLoginRequest()
            {
                Name = Mocks.UnconfirmedUser,
                Password = Mocks.ValidPassword,
            };
            var result = await this.service.PutLoginAsync(request);

            Assert.NotNull(result.Error);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.Contains(ErrorMessages.UserNotFound, result.Error!.Messages);
        }
    }
}
