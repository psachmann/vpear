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
        }

        [Fact]
        public async Task GetAsync200OKTest()
        {
            var result = await this.service.GetAsync(Roles.AdminRole);

            Assert.NotNull(result.Value);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.InRange(result.Value!.Count, 0, int.MaxValue);

            result = await this.service.GetAsync(Roles.UserRole);

            Assert.NotNull(result.Value);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.InRange(result.Value!.Count, 0, int.MaxValue);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task PutAsync200OKTest(bool isVerified)
        {
            var request = new PutVerifyRequest()
            {
                IsVerified = isVerified,
            };
            var result = await this.service.PutVerifyAsync(Mocks.User.Name, request);

            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }

        [Fact]
        public async Task PutAsync404NotFoundTest()
        {
            var result = await this.service.PutVerifyAsync(Mocks.NotExisting.Id.ToString(), new PutVerifyRequest());

            Assert.NotNull(result.Error);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.Contains(ErrorMessages.UserNotFound, result.Error!.Messages);
        }

        [Fact]
        public async Task DeleteAsync204NoContentTest()
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
        public async Task PostRegisterAsync204NoContentTest()
        {
            var request = new PostRegisterRequest()
            {
                Name = Mocks.UnconfirmedUser,
                IsAdmin = false,
                Password = Mocks.ValidPassword,
            };
            var result = await this.service.PostRegisterAsync(request);

            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);

            request = new PostRegisterRequest()
            {
                Name = Mocks.UnconfirmedUser,
                IsAdmin = true,
                Password = Mocks.ValidPassword,
            };
            result = await this.service.PostRegisterAsync(request);

            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
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
