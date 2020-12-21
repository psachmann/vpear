using FluentValidation;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Validators;
using Xunit;
using static VPEAR.Server.Constants;

namespace VPEAR.Server.Test.Validators
{
    public class PutUserValidatorTest
    {
        private readonly IValidator<PutUserRequest> validator;

        public PutUserValidatorTest()
        {
            this.validator = new PutUserValidator();
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
            var result = validator.Validate(request);

            Assert.True(result.IsValid);
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
            var result = validator.Validate(request);

            Assert.False(result.IsValid);
        }
    }
}
