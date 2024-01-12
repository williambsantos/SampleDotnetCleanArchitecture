using FluentAssertions;
using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;

namespace SampleDotnetCleanArchitecture.EnterpriseBusiness.Tests.Entities;

public class ClientTest
{
    [Theory]
    [InlineData(null, "lastName", "2023-12-01", "FirstName is required")]
    [InlineData("", "lastName", "2023-12-01", "FirstName is required")]
    [InlineData(" ", "lastName", "2023-12-01", "FirstName is required")]

    [InlineData("firstName", null, "2023-12-01", "LastName is required")]
    [InlineData("firstName", "", "2023-12-01", "LastName is required")]
    [InlineData("firstName", " ", "2023-12-01", "LastName is required")]

    [InlineData("firstName", "lastName", null, "BirthDate is required")]
    [InlineData("firstName", "lastName", "", "BirthDate is required")]
    [InlineData("firstName", "lastName", " ", "BirthDate is required")]

    public void Validate_Name_Password_MustHave_Message(string firstName, string lastName, string birthDate, string errorMessage)
    {
        var birthDateDt = string.IsNullOrWhiteSpace(birthDate) ? DateTime.MinValue : DateTime.Parse(birthDate);

        var actual = new Client(firstName, lastName, birthDateDt);

        actual
            .Invoking(x => x.Validate())
            .Should()
            .Throw<DomainValidationException>()
            .WithMessage(errorMessage)
        ;
    }
}
