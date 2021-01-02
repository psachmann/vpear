// <copyright file="UserControllerTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Controllers;
using VPEAR.Server.Internals;
using Xunit;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test.Controllers
{
    public class UserControllerTest : IClassFixture<AutofacFixture>
    {
        private readonly UserController controller;

        public UserControllerTest(AutofacFixture fixture)
        {
            this.controller = fixture.Container.Resolve<UserController>();

            Configuration.EnsureLoaded(Environment.GetCommandLineArgs());
        }

        [Fact]
        public async Task OnGetAsync200OKTest()
        {
            var result = await this.controller.OnGetAsync(Roles.AdminRole);
            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = Assert.IsAssignableFrom<Container<GetUserResponse>>(jsonResult.Value);

            Assert.NotNull(response);
            Assert.InRange(response.Items.Count, 1L, long.MaxValue);

            result = await this.controller.OnGetAsync(Roles.UserRole);
            jsonResult = Assert.IsType<JsonResult>(result);
            response = Assert.IsAssignableFrom<Container<GetUserResponse>>(jsonResult.Value);

            Assert.NotNull(response);
            Assert.InRange(response.Items.Count, 1L, long.MaxValue);
        }

        [Theory]
        [InlineData(false, null, null)]
        [InlineData(true, null, null)]
        [InlineData(false, "newPassword", "oldPassword")]
        [InlineData(true, "newPassword", "oldPassword")]
        public async Task OnPutAsync200OKTest(
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
            var result = await this.controller.OnPutAsync(Mocks.User.Id.ToString(), request);
            var jsonResult = Assert.IsType<JsonResult>(result);

            Assert.Null(jsonResult.Value);
        }

        [Fact]
        public async Task OnPutAsync404NotFoundTest()
        {
            var result = await this.controller.OnPutAsync(Mocks.NotExisting.Id.ToString(), new PutUserRequest());
            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(jsonResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Contains(ErrorMessages.UserNotFound, response.Messages);
        }

        [Fact]
        public async Task OnDeleteAsync200OKTest()
        {
            var result = await this.controller.OnDeleteAsync(Mocks.User.Id.ToString());
            var jsonResult = Assert.IsType<JsonResult>(result);

            Assert.Null(jsonResult.Value);
        }

        [Fact]
        public async Task OnDeleteAsync403ForbiddenTest()
        {
            var result = await this.controller.OnDeleteAsync(Mocks.Admin.Id.ToString());
            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(jsonResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status403Forbidden, response.StatusCode);
            Assert.Contains(ErrorMessages.LastAdminError, response.Messages);
        }

        [Fact]
        public async Task OnDeleteAsync404NotFoundTest()
        {
            var result = await this.controller.OnDeleteAsync(Mocks.NotExisting.Id.ToString());
            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(jsonResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Contains(ErrorMessages.UserNotFound, response.Messages);
        }

        [Fact]
        public async Task OnPostRegisterAsync200OKTest()
        {
            var request = new PostRegisterRequest()
            {
                DisplayName = "display_name",
                Email = Mocks.UnconfirmedEmail,
                IsAdmin = false,
                Password = Mocks.ValidPassword,
            };
            var result = await this.controller.OnPostRegisterAsync(request);
            var jsonResult = Assert.IsType<JsonResult>(result);

            Assert.Null(jsonResult.Value);

            request = new PostRegisterRequest()
            {
                DisplayName = "display_name",
                Email = Mocks.UnconfirmedEmail,
                IsAdmin = true,
                Password = Mocks.ValidPassword,
            };
            result = await this.controller.OnPostRegisterAsync(request);
            jsonResult = Assert.IsType<JsonResult>(result);

            Assert.Null(jsonResult.Value);
        }

        [Fact]
        public async Task OnPostRegisterAsync409ConflictTest()
        {
            var request = new PostRegisterRequest()
            {
                DisplayName = "display_name",
                Email = Mocks.ConfirmedEmail,
                IsAdmin = false,
                Password = Mocks.ValidPassword,
            };
            var result = await this.controller.OnPostRegisterAsync(request);
            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(jsonResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status409Conflict, response.StatusCode);
            Assert.Contains(ErrorMessages.UserEmailAlreadyUsed, response.Messages);
        }

        [Fact]
        public async Task OnPutLoginAsync200OKTest()
        {
            var request = new PutLoginRequest()
            {
                Email = Mocks.ConfirmedEmail,
                Password = Mocks.ValidPassword,
            };
            var result = await this.controller.OnPutLoginAsync(request);
            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = Assert.IsAssignableFrom<PutLoginResponse>(jsonResult.Value);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task OnPutLoginAsync403ForbiddenTest()
        {
            var request = new PutLoginRequest()
            {
                Email = Mocks.ConfirmedEmail,
                Password = Mocks.InvalidPassword,
            };
            var result = await this.controller.OnPutLoginAsync(request);
            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(jsonResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status403Forbidden, response.StatusCode);
            Assert.Contains(ErrorMessages.InvalidPassword, response.Messages);
        }

        [Fact]
        public async Task OnPutLoginAsync404NotFoundTest()
        {
            var request = new PutLoginRequest()
            {
                Email = Mocks.UnconfirmedEmail,
                Password = Mocks.ValidPassword,
            };
            var result = await this.controller.OnPutLoginAsync(request);
            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(jsonResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Contains(ErrorMessages.UserNotFound, response.Messages);
        }
    }
}