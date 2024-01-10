using AutoFixture;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;
using SampleDotnetCleanArchitecture.Infrastructure.SqlServer.Interfaces;
using SampleDotnetCleanArchitecture.Infrastructure.SqlServer.Repositories;
using System.Data;
using System.Data.Common;

namespace SampleDotnetCleanArchitecture.Infrastructure.SqlServer.Tests.Repositories
{
    public class AccountRepositoryTest
    {
        [Fact]
        public void Constructor_HasNoValue_ShouldBeOk()
        {
            // Arrange
            var databaseRepository = new DatabaseRepository(new SqlConnection());

            // Act
            var accountRepository = new AccountRepository(databaseRepository);

            // Assert
            accountRepository.Should().NotBeNull();
            accountRepository.Should().BeOfType<AccountRepository>();
        }

        [Fact]
        public async Task ListAsync_WhenCalled_ShouldBeOk()
        {
            // Arrange
            var expected = new List<AccountRecord>
            {
                new AccountRecord
                (
                    UserName: "test",
                    PasswordHash: "test",
                    CreationDate: DateTime.Now,
                    CreatedBy: "test",
                    LastModifiedDate: DateTime.Now,
                    LastModifiedBy: "test",
                    RoleName: "role1",
                    ClaimName: AccountClaims.CLAIMS_CLIENT_CAN_INSERT.ToString()
                ),
                new AccountRecord
                (
                    UserName: "test",
                    PasswordHash: "test",
                    CreationDate: DateTime.Now,
                    CreatedBy: "test",
                    LastModifiedDate: DateTime.Now,
                    LastModifiedBy: "test",
                    RoleName: "role2",
                    ClaimName: AccountClaims.CLAIMS_CLIENT_CAN_UPDATE.ToString()
                )
            };

            var expectedDataReader = expected.ToDataReader();

            var databaseRepositoryMock = new Mock<IDatabaseRepository>();
            databaseRepositoryMock
                .Setup(x => x.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<CommandType>(), It.IsAny<DbParameter[]>()))
                .ReturnsAsync(expectedDataReader);

            var accountRepository = new AccountRepository(databaseRepositoryMock.Object);

            // Act
            var accounts = await accountRepository.ListAsync();

            // Assert
            accounts.Should().NotBeNull();
            accounts.Should().BeOfType<List<Account>>();
            accounts.Count.Should().Be(1);
            accounts.First().Roles.Count.Should().Be(2);
            accounts.First().Claims.Count.Should().Be(2);
            accounts.First().UserName.Should().Be(expected[0].UserName);
            accounts.First().PasswordHash.Should().Be(expected[0].PasswordHash);
            accounts.First().CreationDate.Should().Be(expected[0].CreationDate);
            accounts.First().CreatedBy.Should().Be(expected[0].CreatedBy);
            accounts.First().LastModifiedDate.Should().Be(expected[0].LastModifiedDate);
            accounts.First().LastModifiedBy.Should().Be(expected[0].LastModifiedBy);
            accounts.First().LastModifiedDate.Should().Be(expected[0].LastModifiedDate);
            accounts.First().Roles.First().Should().Be(expected[0].RoleName);
            accounts.First().Roles.Last().Should().Be(expected[1].RoleName);
            accounts.First().Claims.First().Should().Be(Enum.Parse<AccountClaims>(expected[0].ClaimName));
            accounts.First().Claims.Last().Should().Be(Enum.Parse<AccountClaims>(expected[1].ClaimName));
        }

        [Fact]
        public async Task GetByNameAsync_WhenCalled_ShouldBeOk()
        {
            // Arrange
            var userName = "test";

            var expected = new List<AccountRecord>
            {
                new AccountRecord
                (
                    UserName: "test",
                    PasswordHash: "test",
                    CreationDate: DateTime.Now,
                    CreatedBy: "test",
                    LastModifiedDate: DateTime.Now,
                    LastModifiedBy: "test",
                    RoleName: "role1",
                    ClaimName: AccountClaims.CLAIMS_CLIENT_CAN_INSERT.ToString()
                ),
                new AccountRecord
                (
                    UserName: "test",
                    PasswordHash: "test",
                    CreationDate: DateTime.Now,
                    CreatedBy: "test",
                    LastModifiedDate: DateTime.Now,
                    LastModifiedBy: "test",
                    RoleName: "role2",
                    ClaimName: AccountClaims.CLAIMS_CLIENT_CAN_UPDATE.ToString()
                )
            };

            var expectedDataReader = expected.ToDataReader();

            var databaseRepositoryMock = new Mock<IDatabaseRepository>();
            databaseRepositoryMock
                .Setup(x => x.ExecuteReaderAsync(It.IsAny<string>(), It.IsAny<CommandType>(), It.IsAny<DbParameter[]>()))
                .ReturnsAsync(expectedDataReader);

            var accountRepository = new AccountRepository(databaseRepositoryMock.Object);

            // Act
            var account = await accountRepository.GetByNameAsync(userName);

            // Assert
            account.Should().NotBeNull();
            account.Should().BeOfType<Account>();
            account.Roles.Count.Should().Be(2);
            account.Claims.Count.Should().Be(2);
            account.UserName.Should().Be(expected[0].UserName);
            account.PasswordHash.Should().Be(expected[0].PasswordHash);
            account.CreationDate.Should().Be(expected[0].CreationDate);
            account.CreatedBy.Should().Be(expected[0].CreatedBy);
            account.LastModifiedDate.Should().Be(expected[0].LastModifiedDate);
            account.LastModifiedBy.Should().Be(expected[0].LastModifiedBy);
            account.LastModifiedDate.Should().Be(expected[0].LastModifiedDate);
            account.Roles.First().Should().Be(expected[0].RoleName);
            account.Roles.Last().Should().Be(expected[1].RoleName);
            account.Claims.First().Should().Be(Enum.Parse<AccountClaims>(expected[0].ClaimName));
            account.Claims.Last().Should().Be(Enum.Parse<AccountClaims>(expected[1].ClaimName));
        }

        [Fact]
        public async Task CreateAsync_WhenCalled_ShouldBeOk()
        {
            // Arrange
            var account = new Fixture().Create<Account>();

            var databaseRepositoryMock = new Mock<IDatabaseRepository>();
            databaseRepositoryMock
                .Setup(x => x.ExecuteNonQueryAsync(It.IsAny<string>(), It.IsAny<CommandType>(), It.IsAny<DbParameter[]>()))
                .ReturnsAsync(1);

            var accountRepository = new AccountRepository(databaseRepositoryMock.Object);

            // Act
            await accountRepository.CreateAsync(account);

            // Assert
            account.Should().NotBeNull();
            account.Should().BeOfType<Account>();
        }

        [Fact]
        public async Task DeleteAsync_WhenCalled_ShouldBeOk()
        {
            // Arrange
            var userName = "test";

            var databaseRepositoryMock = new Mock<IDatabaseRepository>();
            databaseRepositoryMock
                .Setup(x => x.ExecuteNonQueryAsync(It.IsAny<string>(), It.IsAny<CommandType>(), It.IsAny<DbParameter[]>()))
                .ReturnsAsync(1);

            var accountRepository = new AccountRepository(databaseRepositoryMock.Object);

            // Act
            await accountRepository.DeleteAsync(userName);

            // Assert
            databaseRepositoryMock.Verify(x => x.ExecuteNonQueryAsync(It.IsAny<string>(), It.IsAny<CommandType>(), It.IsAny<DbParameter[]>()), Times.Once());
        }

        [Fact]
        public async Task AddRoleToUserAsync_WhenCalled_ShouldBeOk()
        {
            // Arrange
            var userName = "test";
            var roleName = "role";

            var databaseRepositoryMock = new Mock<IDatabaseRepository>();
            databaseRepositoryMock
                .Setup(x => x.ExecuteNonQueryAsync(It.IsAny<string>(), It.IsAny<CommandType>(), It.IsAny<DbParameter[]>()))
                .ReturnsAsync(1);

            var accountRepository = new AccountRepository(databaseRepositoryMock.Object);

            // Act
            await accountRepository.AddRoleToUserAsync(userName, roleName);

            // Assert
            databaseRepositoryMock.Verify(x => x.ExecuteNonQueryAsync(It.IsAny<string>(), It.IsAny<CommandType>(), It.IsAny<DbParameter[]>()), Times.Once());
        }

        [Fact]
        public async Task DeleteRoleFromUserAsync_WhenCalled_ShouldBeOk()
        {
            // Arrange
            var userName = "test";
            var roleName = "role";

            var databaseRepositoryMock = new Mock<IDatabaseRepository>();
            databaseRepositoryMock
                .Setup(x => x.ExecuteNonQueryAsync(It.IsAny<string>(), It.IsAny<CommandType>(), It.IsAny<DbParameter[]>()))
                .ReturnsAsync(1);

            var accountRepository = new AccountRepository(databaseRepositoryMock.Object);

            // Act
            await accountRepository.DeleteRoleFromUserAsync(userName, roleName);

            // Assert
            databaseRepositoryMock.Verify(x => x.ExecuteNonQueryAsync(It.IsAny<string>(), It.IsAny<CommandType>(), It.IsAny<DbParameter[]>()), Times.Once());
        }

        [Fact]
        public async Task AddClaimToUserAsync_WhenCalled_ShouldBeOk()
        {
            // Arrange
            var userName = "test";
            var claimName = AccountClaims.CLAIMS_CLIENT_CAN_INSERT;

            var databaseRepositoryMock = new Mock<IDatabaseRepository>();
            databaseRepositoryMock
                .Setup(x => x.ExecuteNonQueryAsync(It.IsAny<string>(), It.IsAny<CommandType>(), It.IsAny<DbParameter[]>()))
                .ReturnsAsync(1);

            var accountRepository = new AccountRepository(databaseRepositoryMock.Object);

            // Act
            await accountRepository.AddClaimToUserAsync(userName, claimName);

            // Assert
            databaseRepositoryMock.Verify(x => x.ExecuteNonQueryAsync(It.IsAny<string>(), It.IsAny<CommandType>(), It.IsAny<DbParameter[]>()), Times.Once());
        }

        [Fact]
        public async Task DeleteClaimFromUserAsync_WhenCalled_ShouldBeOk()
        {
            // Arrange
            var userName = "test";
            var claimName = AccountClaims.CLAIMS_CLIENT_CAN_INSERT;

            var databaseRepositoryMock = new Mock<IDatabaseRepository>();
            databaseRepositoryMock
                .Setup(x => x.ExecuteNonQueryAsync(It.IsAny<string>(), It.IsAny<CommandType>(), It.IsAny<DbParameter[]>()))
                .ReturnsAsync(1);

            var accountRepository = new AccountRepository(databaseRepositoryMock.Object);

            // Act
            await accountRepository.DeleteClaimFromUserAsync(userName, claimName);

            // Assert
            databaseRepositoryMock.Verify(x => x.ExecuteNonQueryAsync(It.IsAny<string>(), It.IsAny<CommandType>(), It.IsAny<DbParameter[]>()), Times.Once());
        }
    }
}
