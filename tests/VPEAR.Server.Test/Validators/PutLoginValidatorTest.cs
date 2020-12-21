using FluentValidation;
using VPEAR.Core.Wrappers;
using VPEAR.Server.Validators;
using Xunit;

namespace VPEAR.Server.Test.Validators
{
    public class PutLoginValidatorTest
    {
        private readonly IValidator<PutLoginRequest> validator;

        public PutLoginValidatorTest()
        {
            this.validator = new PutLoginValidator();
        }

        [Theory]
        [InlineData("example@domain.tld", "password")]
        public void ValidateSuccessTest(string? email, string? password)
        {
            var request = new PutLoginRequest()
            {
                Email = email,
                Password = password,
            };
            var result = validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(null, "")]
        [InlineData("", null)]
        [InlineData("example@domain.tld", "short")]
        [InlineData("exampledomain.tld", "password")]
        public void ValidateFailureTest(string? email, string? password)
        {
            var request = new PutLoginRequest()
            {
                Email = email,
                Password = password,
            };
            var result = validator.Validate(request);

            Assert.False(result.IsValid);
        }
    }
}
