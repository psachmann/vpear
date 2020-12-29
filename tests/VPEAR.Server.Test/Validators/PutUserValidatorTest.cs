// <copyright file="PutUserValidatorTest.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

/*
using Autofac;
using FluentValidation;
using VPEAR.Core.Wrappers;
using Xunit;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test.Validators
{
    public class PutUserValidatorTest : IClassFixture<AutofacFixture>
    {
        private readonly IValidator<PutUserRequest> validator;

        public PutUserValidatorTest(AutofacFixture fixture)
        {
            this.validator = fixture.Container.Resolve<IValidator<PutUserRequest>>();
        }

        [Theory]
        [InlineData(null, null, false, null, null)]
        [InlineData("display_name", null, false, null, null)]
        [InlineData(null, "example@domain.tld", false, null, null)]
        [InlineData(null, null, false, "password", null)]
        [InlineData(null, null, true, null, Roles.AdminRole)]
        [InlineData(null, null, true, null, Roles.UserRole)]
        [InlineData("display_name", "example@domain.tld", true, "password", Roles.UserRole)]
        public void ValidateSuccessTest(
            string? displayName,
            string? email,
            bool isVerified,
            string? password,
            string? role)
        {
            var request = new PutUserRequest()
            {
                DisplayName = displayName,
                Email = email,
                IsVerified = isVerified,
                Password = password,
                Role = role,
            };
            var result = this.validator.Validate(request);

            Assert.True(result.IsValid, "This should be a valid request.");
        }

        [Theory]
        [InlineData("", null, false, null, null)]
        [InlineData(null, "", false, null, null)]
        [InlineData(null, "exampledomain.tld", false, null, null)]
        [InlineData(null, null, true, null, null)]
        [InlineData(null, null, true, null, "")]
        [InlineData(null, null, true, null, "none")]
        [InlineData(null, null, false, "", null)]
        [InlineData(null, null, false, "short", null)]
        [InlineData(null, null, false, null, "admin")]
        [InlineData(null, null, false, null, "user")]
        public void ValidateFailureTest(
            string? displayName,
            string? email,
            bool isVerified,
            string? password,
            string? role)
        {
            var request = new PutUserRequest()
            {
                DisplayName = displayName,
                Email = email,
                IsVerified = isVerified,
                Password = password,
                Role = role,
            };
            var result = this.validator.Validate(request);

            Assert.False(result.IsValid, "This should NOT be a valid request.");
        }
    }
}
*/
