namespace SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities
{
    public class AccountTest
    {
        [Theory]
        [InlineData("name")]
        public void Construct_WithName_MustFillProperties(string name) =>
            Assert.Equal(new Account(name).UserName, name);

        [Theory]
        [InlineData("name", "password")]
        public void Construct_WithNamePassword_MustFillProperties(string name, string password)
        {
            var actual = new Account(name, password);
            Assert.Equal(actual.UserName, name);
            Assert.Equal(password, actual.PasswordHash);
        }

        [Theory]
        [InlineData("name", "password")]
        public void Construct_WithNamePasswordRolesClaims_MustFillProperties(string name, string password)
        {
            var roles = Enumerable.Repeat<string>("name", 3).ToList();
            var claims = Enumerable.Repeat<AccountClaims>(AccountClaims.CLAIMS_USER_CAN_MODIFY, 3).ToList();

            var actual = new Account(name, password, roles, claims);
            Assert.Equal(actual.UserName, name);
            Assert.Equal(password, actual.PasswordHash);
            Assert.True(roles.SequenceEqual(actual.Roles));
            Assert.True(claims.SequenceEqual(actual.Claims));
        }

        [Theory]
        [InlineData(null, "password", "Username is required")]
        [InlineData("", "password", "Username is required")]
        [InlineData(" ", "password", "Username is required")]

        [InlineData("name", null, "Password is required")]
        [InlineData("name", "", "Password is required")]
        [InlineData("name", " ", "Password is required")]
        public void Validate_Name_Password_MustHave_Message(string name, string password, string errorMessage)
        {
            var actual = new Account(name, password);
            
            Assert.Throws<DomainValidationException>(() => actual.Validate()).Message.Contains(errorMessage);
        }
    }
}