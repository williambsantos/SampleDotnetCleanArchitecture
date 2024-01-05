using Microsoft.Data.SqlClient;
using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;
using SampleDotnetCleanArchitecture.EnterpriseBusiness.Interfaces;
using System.Data;

namespace SampleDotnetCleanArchitecture.Infrastructure.SqlServer
{
    public interface IDbConnectionProject
    {
        SqlConnection CreateConnection();
    }

    public class DbConnectionProject : IDbConnectionProject
    {
        private readonly string _connectionString;

        public DbConnectionProject(string connectionString)
        {
            _connectionString = connectionString;
        }   

        public SqlConnection CreateConnection()
        {
            var connection = new Microsoft.Data.SqlClient.SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }

    public class AccountRepository : IAccountRepository, IDisposable
    {
        private readonly Lazy<SqlConnection> _dbConnectionLazy;
        public AccountRepository(IDbConnectionProject dbConnectionProject)
        {
            _dbConnectionLazy = new Lazy<SqlConnection>(() => dbConnectionProject.CreateConnection());
        }

        public async Task<ICollection<Account>> ListAsync()
        {
            using var command = _dbConnectionLazy.Value.CreateCommand();
            command.CommandText = @"
SELECT
    a.UserName,
    a.PasswordHash,
    a.CreationDate,
    a.CreatedBy,
    a.LastModifiedDate,
    a.LastModifiedBy,
    ar.RoleName,
    ac.ClaimName
FROM [Account] a
LEFT JOIN [AccountRoles] ar ON ar.Account_UserName = a.UserName
LEFT JOIN [AccountClaims] ac ON ac.Account_UserName = a.UserName
";
            command.CommandType = CommandType.Text;

            using var reader = await command.ExecuteReaderAsync();

            var map = new Dictionary<string, Account>();
            while (reader.Read())
            {
                var userName = !reader.IsDBNull(0) ? reader.GetString(0) : default;
                var passwordHash = !reader.IsDBNull(1) ? reader.GetString(1) : default;
                var creationDate = !reader.IsDBNull(2) ? reader.GetDateTime(2) : default;
                var createdBy = !reader.IsDBNull(3) ? reader.GetString(3) : default;
                var lastModifiedDate = !reader.IsDBNull(4) ? reader.GetDateTime(4) : default;
                var lastModifiedBy = !reader.IsDBNull(5) ? reader.GetString(5) : default;

                var roleName = !reader.IsDBNull(6) ? reader.GetString(6) : default;
                var claimName = !reader.IsDBNull(7) ? reader.GetString(7) : default;

                if (!map.TryGetValue(userName, out var account))
                {
                    account = new Account(userName, passwordHash);
                    account.CreationDate = creationDate;
                    account.CreatedBy = createdBy;
                    account.LastModifiedDate = lastModifiedDate;
                    account.LastModifiedBy = lastModifiedBy;
                    map.Add(userName, account);
                }

                if (!string.IsNullOrWhiteSpace(roleName))
                    map[userName].Roles.Add(roleName);

                if (!string.IsNullOrWhiteSpace(claimName))
                {
                    if (Enum.TryParse<AccountClaims>(claimName, out var accountClaim))
                        map[userName].Claims.Add(accountClaim);
                }
            }
            return map.Values.ToList() ?? new List<Account>();
        }

        public async Task<Account?> GetByNameAsync(string username)
        {
            using var command = _dbConnectionLazy.Value.CreateCommand();
            command.CommandText = @"
SELECT
    a.UserName,
    a.PasswordHash,
    a.CreationDate,
    a.CreatedBy,
    a.LastModifiedDate,
    a.LastModifiedBy,
    ar.RoleName,
    ac.ClaimName
FROM [Account] a
LEFT JOIN [AccountRoles] ar ON ar.Account_UserName = a.UserName
LEFT JOIN [AccountClaims] ac ON ac.Account_UserName = a.UserName
WHERE a.UserName = @UserName
";
            command.CommandType = CommandType.Text;
            command.AddParameter("@UserName", username);

            using (var reader = await command.ExecuteReaderAsync())
            {
                var map = new Dictionary<string, Account>();
                while (reader.Read())
                {
                    var userName = !reader.IsDBNull(0) ? reader.GetString(0) : default;
                    var passwordHash = !reader.IsDBNull(1) ? reader.GetString(1) : default;
                    var creationDate = !reader.IsDBNull(2) ? reader.GetDateTime(2) : default;
                    var createdBy = !reader.IsDBNull(3) ? reader.GetString(3) : default;
                    var lastModifiedDate = !reader.IsDBNull(4) ? reader.GetDateTime(4) : default;
                    var lastModifiedBy = !reader.IsDBNull(5) ? reader.GetString(5) : default;

                    var roleName = !reader.IsDBNull(6) ? reader.GetString(6) : default;
                    var claimName = !reader.IsDBNull(7) ? reader.GetString(7) : default;

                    if (!map.TryGetValue(userName, out var account))
                    {
                        account = new Account(userName, passwordHash);
                        account.CreationDate = creationDate;
                        account.CreatedBy = createdBy;
                        account.LastModifiedDate = lastModifiedDate;
                        account.LastModifiedBy = lastModifiedBy;
                        map.Add(userName, account);
                    }

                    if (!string.IsNullOrWhiteSpace(roleName))
                        map[userName].Roles.Add(roleName);

                    if (!string.IsNullOrWhiteSpace(claimName))
                    {
                        if (Enum.TryParse<AccountClaims>(claimName, out var accountClaim))
                            map[userName].Claims.Add(accountClaim);
                    }
                }
                return map.Values.FirstOrDefault();
            }
        }

        public async Task<Account> CreateAsync(Account account)
        {
            using var command = _dbConnectionLazy.Value.CreateCommand();
            command.CommandText = @"
INSERT INTO [Account] (
    UserName,
    PasswordHash,
    CreationDate,
    CreatedBy,
    LastModifiedDate,
    LastModifiedBy
) VALUES (
    @UserName,
    @PasswordHash,
    @CreationDate,
    @CreatedBy,
    @LastModifiedDate,
    @LastModifiedBy
)
";
            command.CommandType = CommandType.Text;
            command.AddParameter("@UserName", account.UserName);
            command.AddParameter("@PasswordHash", account.PasswordHash);
            command.AddParameter("@CreationDate", account.CreationDate ?? DateTime.Now);
            command.AddParameter("@CreatedBy", account.CreatedBy);
            command.AddParameter("@LastModifiedDate", account.LastModifiedDate);
            command.AddParameter("@LastModifiedBy", account.LastModifiedBy);

            await command.ExecuteNonQueryAsync();

            if (account.Roles.Any())
            {
                foreach (var role in account.Roles)
                    await AddRoleToUserAsync(account.UserName, role);
            }

            if (account.Claims.Any())
            {
                foreach (var claims in account.Claims)
                    await AddClaimToUserAsync(account.UserName, claims);
            }

            return account;
        }

        public async Task DeleteAsync(string userName)
        {
            using var command = _dbConnectionLazy.Value.CreateCommand();
            command.CommandText = @"
DELETE FROM [Account] WHERE UserName = @UserName;
DELETE FROM [AccountRoles] WHERE Account_UserName = @UserName;
DELETE FROM [AccountClaims] WHERE Account_UserName = @UserName;
";
            command.CommandType = CommandType.Text;
            command.AddParameter("@UserName", userName);

            await command.ExecuteNonQueryAsync();
        }

        public async Task AddRoleToUserAsync(string userName, string roleName)
        {
            using var command = _dbConnectionLazy.Value.CreateCommand();
            command.CommandText = @"INSERT INTO [AccountRoles] (Account_UserName, RoleName) VALUES (@UserName, @RoleName)";
            command.CommandType = CommandType.Text;
            command.AddParameter("@UserName", userName);
            command.AddParameter("@RoleName", roleName);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteRoleFromUserAsync(string userName, string roleName)
        {
            using var command = _dbConnectionLazy.Value.CreateCommand();
            command.CommandText = @"DELETE [AccountRoles] WHERE Account_UserName = @UserName AND RoleName = @RoleName";
            command.CommandType = CommandType.Text;
            command.AddParameter("@UserName", userName);
            command.AddParameter("@RoleName", roleName);

            await command.ExecuteNonQueryAsync();
        }

        public async Task AddClaimToUserAsync(string userName, AccountClaims claimName)
        {
            using var command = _dbConnectionLazy.Value.CreateCommand();
            command.CommandText = @"INSERT INTO [AccountClaims] (Account_UserName, ClaimName) VALUES (@UserName, @ClaimName)";
            command.CommandType = CommandType.Text;
            command.AddParameter("@UserName", userName);
            command.AddParameter("@ClaimName", claimName);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteClaimFromUserAsync(string userName, AccountClaims claimName)
        {
            using var command = _dbConnectionLazy.Value.CreateCommand();
            command.CommandText = @"DELETE [AccountClaims] WHERE Account_UserName = @UserName AND ClaimName = @ClaimName";
            command.CommandType = CommandType.Text;
            command.AddParameter("@UserName", userName);
            command.AddParameter("@ClaimName", claimName);

            await command.ExecuteNonQueryAsync();
        }

        public void Dispose()
        {
            if (_dbConnectionLazy != null && _dbConnectionLazy.IsValueCreated)
            {
                _dbConnectionLazy.Value.Dispose();
            }
        }
    }
}
