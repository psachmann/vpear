// <copyright file="UserControllerTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Controllers;
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
        }

        [Fact]
        public async Task OnGetAsync200OKTest()
        {
            var result = await this.controller.OnGetAsync(Roles.AdminRole);
            var objectResult = Assert.IsType<ObjectResult>(result);
            var response = Assert.IsAssignableFrom<Container<GetUserResponse>>(objectResult.Value);

            Assert.NotNull(response);
            Assert.InRange(response.Items.Count, 1L, long.MaxValue);

            result = await this.controller.OnGetAsync(Roles.UserRole);
            objectResult = Assert.IsType<ObjectResult>(result);
            response = Assert.IsAssignableFrom<Container<GetUserResponse>>(objectResult.Value);

            Assert.NotNull(response);
            Assert.InRange(response.Items.Count, 1L, long.MaxValue);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task OnPutAsync204NoContentTest(bool isVerified)
        {
            var request = new PutVerifyRequest()
            {
                IsVerified = isVerified,
            };
            var result = await this.controller.OnPutVerifyAsync(Mocks.User.Name, request);
            var objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Null(objectResult.Value);
        }

        [Fact]
        public async Task OnPutAsync404NotFoundTest()
        {
            var result = await this.controller.OnPutVerifyAsync(Mocks.NotExisting.Id.ToString(), new PutVerifyRequest());
            var objectResult = Assert.IsType<ObjectResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(objectResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Contains(ErrorMessages.UserNotFound, response.Messages);
        }

        [Fact]
        public async Task OnDeleteAsync204NoContentTest()
        {
            var result = await this.controller.OnDeleteAsync(Mocks.User.Name);
            var objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Null(objectResult.Value);
        }

        [Fact]
        public async Task OnDeleteAsync403ForbiddenTest()
        {
            var result = await this.controller.OnDeleteAsync(Mocks.Admin.Name);
            var objectResult = Assert.IsType<ObjectResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(objectResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status403Forbidden, response.StatusCode);
            Assert.Contains(ErrorMessages.LastAdminError, response.Messages);
        }

        [Fact]
        public async Task OnDeleteAsync404NotFoundTest()
        {
            var result = await this.controller.OnDeleteAsync(Mocks.NotExisting.Id.ToString());
            var objectResult = Assert.IsType<ObjectResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(objectResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Contains(ErrorMessages.UserNotFound, response.Messages);
        }

        [Fact]
        public async Task OnPostRegisterAsync204NoContentTest()
        {
            var request = new PostRegisterRequest()
            {
                Name = Mocks.UnconfirmedUser,
                IsAdmin = false,
                Password = Mocks.ValidPassword,
            };
            var result = await this.controller.OnPostRegisterAsync(request);
            var objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Null(objectResult.Value);

            request = new PostRegisterRequest()
            {
                Name = Mocks.UnconfirmedUser,
                IsAdmin = true,
                Password = Mocks.ValidPassword,
            };
            result = await this.controller.OnPostRegisterAsync(request);
            objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Null(objectResult.Value);
        }

        [Fact]
        public async Task OnPostRegisterAsync409ConflictTest()
        {
            var request = new PostRegisterRequest()
            {
                Name = Mocks.ConfirmedUser,
                IsAdmin = false,
                Password = Mocks.ValidPassword,
            };
            var result = await this.controller.OnPostRegisterAsync(request);
            var objectResult = Assert.IsType<ObjectResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(objectResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status409Conflict, response.StatusCode);
            Assert.Contains(ErrorMessages.UserNameAlreadyUsed, response.Messages);
        }

        [Fact]
        public async Task OnPutLoginAsync404NotFoundTest()
        {
            var request = new PutLoginRequest()
            {
                Name = Mocks.UnconfirmedUser,
                Password = Mocks.ValidPassword,
            };
            var result = await this.controller.OnPutLoginAsync(request);
            var objectResult = Assert.IsType<ObjectResult>(result);
            var response = Assert.IsAssignableFrom<ErrorResponse>(objectResult.Value);

            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Contains(ErrorMessages.UserNotFound, response.Messages);
        }
    }
}
