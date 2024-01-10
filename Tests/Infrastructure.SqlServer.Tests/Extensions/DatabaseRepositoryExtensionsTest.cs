using System.Data.Common;
using FluentAssertions;
using SampleDotnetCleanArchitecture.Infrastructure.SqlServer.Extensions;

namespace SampleDotnetCleanArchitecture.Infrastructure.SqlServer.Tests.Extensions
{
    public class DatabaseRepositoryExtensionsTest
    {
        [Fact]
        public void AddParameter_HasValue_ShouldBeOk()
        {
            // Arrange
            var expectedList = new List<DbParameter>();
            var name = "name";
            var value = "value";

            // Act
            expectedList.AddParameter(name, value);

            // Assert
            expectedList.Should().NotBeNull();
            expectedList.Count.Should().Be(1);
            expectedList[0].ParameterName.Should().Be(name);
            expectedList[0].IsNullable.Should().BeFalse();
            expectedList[0].Value.Should().Be(value);
        }

        [Fact]
        public void AddParameter_HasNoValue_ShouldBeOk()
        {
            // Arrange
            var expectedList = new List<DbParameter>();
            var name = "name";

            // Act
            expectedList.AddParameter(name, null);

            // Assert
            expectedList.Should().NotBeNull();
            expectedList.Count.Should().Be(1);
            expectedList[0].ParameterName.Should().Be(name);
            expectedList[0].IsNullable.Should().BeTrue();
            expectedList[0].Value.Should().Be(DBNull.Value);
        }
    }
}
