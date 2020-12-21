using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPEAR.Core.Wrappers;
using VPEAR.Core.Abstractions;
using VPEAR.Server.Services;
using Xunit;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test.Services
{
    public class UserServiceTest : IClassFixture<VPEARDbContextFixture>
    {
        private readonly Guid admin = new Guid("00000000-0000-0000-0000-000000000001");
        private readonly Guid user = new Guid("00000000-0000-0000-0000-000000000002");
        private readonly Guid tester = new Guid("00000000-0000-0000-0000-000000000003");
        private readonly Guid notVerifiedUser = new Guid("00000000-0000-0000-0000-000000000004");
        private readonly Guid notExistingUser = new Guid();
        private readonly IUserService service;

        public UserServiceTest(VPEARDbContextFixture fixture)
        {
            this.service = new UserService();
        }

        [Fact]
        public async Task GetAsync200OKTest()
        {
            var response = await this.service.GetAsync(id: this.admin);

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);

            response = await this.service.GetAsync(role: Roles.AdminRole);

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);

            response = await this.service.GetAsync(id: this.admin, role: Roles.AdminRole);

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAsync404NotFoundTest()
        {
            var response = await this.service.GetAsync(id: this.notExistingUser);

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData("new@domain.tld", null, null)]
        [InlineData("new@domain.tld", "new_password", null)]
        [InlineData("new@domain.tld", "new_password", Roles.UserRole)]
        [InlineData(null, "new_password", Roles.UserRole)]
        [InlineData(null, null, Roles.UserRole)]
        public async Task PutAsync200OKTest(
            string? email,
            string? password,
            string? role)
        {
            var request = new PutUserRequest()
            {
                Email = email,
                Password = password,
                Role = role,
            };
            var response = await this.service.PutAsync(this.tester, request);

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PutAsync404NotFoundTest()
        {
            var response = await this.service.PutAsync(this.notExistingUser, new PutUserRequest());

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("example1@domain.tld", null)] // email already exists in db
        [InlineData(null, Roles.UserRole)] // no admin in the db
        [InlineData("example1@domain.tld", Roles.UserRole)] // cross test
        public async Task PutAsync409ConflictTest(string? email, string? role)
        {
            var request = new PutUserRequest()
            {
                Email = email,
                Password = "password",
                Role = role,
            };
            var response = await this.service.PutAsync(this.admin, request);

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status409Conflict, response.StatusCode);
        }

        [Fact]
        public async Task DeleteAsync200OKTest()
        {
            var response = await this.service.DeleteAsync(this.notVerifiedUser);

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteAsync403ForbiddenTest()
        {
            var response = await this.service.DeleteAsync(this.admin);

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status403Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task DeleteAsync404NotFoundTest()
        {
            var response = await this.service.DeleteAsync(this.notExistingUser);

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PostRegisterAsync200OKTest()
        {
            var request = new PostRegisterRequest();
            var response = await this.service.PostRegisterAsync(request);

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        }

        [Fact]
        public async Task PostRegisterAsync409ConflictTest()
        {
            var request = new PostRegisterRequest();
            var response = await this.service.PostRegisterAsync(request);

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status409Conflict, response.StatusCode);
        }

        [Fact]
        public async Task PutLoginAsync200Test()
        {
            var request = new PutLoginRequest();
            var response = await this.service.PutLoginAsync(this.notExistingUser, request);

            Assert.NotNull(response.Payload);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PutLoginAsync404NotFoundTest()
        {
            var response = await this.service.PutLoginAsync(this.notExistingUser, new PutLoginRequest());

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PutLoginAsync900NotVerifiedTest()
        {
            var response = await this.service.PutLoginAsync(this.notVerifiedUser, new PutLoginRequest());

            Assert.Null(response.Payload);
            Assert.Equal(900, response.StatusCode);
        }

        [Fact]
        public async Task PutLogoutAsync200OKTest()
        {
            var response = await this.service.PutLogoutAsync(this.notExistingUser);

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        }

        [Fact]
        public async Task PutLogoutAsync404NotFoundTest()
        {
            var response = await this.service.PutLogoutAsync(this.notExistingUser);

            Assert.Null(response.Payload);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        }
    }
}
