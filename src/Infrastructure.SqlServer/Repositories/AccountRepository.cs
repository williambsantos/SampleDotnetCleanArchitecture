using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;
using SampleDotnetCleanArchitecture.EnterpriseBusiness.Interfaces;
using SampleDotnetCleanArchitecture.Infrastructure.SqlServer.Extensions;
using SampleDotnetCleanArchitecture.Infrastructure.SqlServer.Interfaces;
using System.Data;
using System.Data.Common;

namespace SampleDotnetCleanArchitecture.Infrastructure.SqlServer.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDatabaseRepository _databaseRepository;

        public AccountRepository(IDatabaseRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;
        }

        public async Task<ICollection<Account>> ListAsync()
        {
            var commandText = @"
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

            using var reader = await _databaseRepository.ExecuteReaderAsync(
                commandText, CommandType.Text, parameters: null
            );

            var records = new List<AccountRecord>();
            while (reader.Read())
            {
                var record = reader.ToRecord();
                records.Add(record);
            }
            var map = records.ToUserNameAccountDictionary() ?? [];
            return map.Values.ToList() ?? [];
        }

        public async Task<Account?> GetByNameAsync(string username)
        {
            var commandText = @"
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

            var parameters = new List<DbParameter>();
            parameters.AddParameter("@UserName", username);

            using var reader = await _databaseRepository.ExecuteReaderAsync(
                commandText, CommandType.Text, parameters: parameters.ToArray()
            );

            var records = new List<AccountRecord>();
            while (reader.Read())
            {
                var record = reader.ToRecord();
                records.Add(record);
            }
            var map = records.ToUserNameAccountDictionary() ?? [];
            return map.Values.FirstOrDefault();
        }

        public async Task CreateAsync(Account account)
        {
            var commandText = @"
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
            var parameters = new List<DbParameter>();

            parameters.AddParameter("@UserName", account.UserName);
            parameters.AddParameter("@PasswordHash", account.PasswordHash);
            parameters.AddParameter("@CreationDate", account.CreationDate ?? DateTime.Now);
            parameters.AddParameter("@CreatedBy", account.CreatedBy ?? string.Empty);
            parameters.AddParameter("@LastModifiedDate", account.LastModifiedDate ?? DateTime.Now);
            parameters.AddParameter("@LastModifiedBy", account.LastModifiedBy ?? string.Empty);

            _ = await _databaseRepository.ExecuteNonQueryAsync(
                 commandText, CommandType.Text, parameters.ToArray()
             );

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
        }

        public async Task DeleteAsync(string userName)
        {
            var commandText = @"
DELETE FROM [Account] WHERE UserName = @UserName;
DELETE FROM [AccountRoles] WHERE Account_UserName = @UserName;
DELETE FROM [AccountClaims] WHERE Account_UserName = @UserName;
";
            var parameters = new List<DbParameter>();
            parameters.AddParameter("@UserName", userName);

            _ = await _databaseRepository.ExecuteNonQueryAsync(
                 commandText, CommandType.Text, parameters.ToArray()
             );
        }

        public async Task AddRoleToUserAsync(string userName, string roleName)
        {
            var commandText = @"INSERT INTO [AccountRoles] (Account_UserName, RoleName) VALUES (@UserName, @RoleName)";

            var parameters = new List<DbParameter>();
            parameters.AddParameter("@UserName", userName);
            parameters.AddParameter("@RoleName", roleName);

            _ = await _databaseRepository.ExecuteNonQueryAsync(
                 commandText, CommandType.Text, parameters.ToArray()
             );
        }

        public async Task DeleteRoleFromUserAsync(string userName, string roleName)
        {
            var commandText = @"DELETE [AccountRoles] WHERE Account_UserName = @UserName AND RoleName = @RoleName";

            var parameters = new List<DbParameter>();
            parameters.AddParameter("@UserName", userName);
            parameters.AddParameter("@RoleName", roleName);

            _ = await _databaseRepository.ExecuteNonQueryAsync(
                 commandText, CommandType.Text, parameters.ToArray()
             );
        }

        public async Task AddClaimToUserAsync(string userName, AccountClaims claimName)
        {
            var commandText = @"INSERT INTO [AccountClaims] (Account_UserName, ClaimName) VALUES (@UserName, @ClaimName)";

            var parameters = new List<DbParameter>();
            parameters.AddParameter("@UserName", userName);
            parameters.AddParameter("@ClaimName", claimName);

            _ = await _databaseRepository.ExecuteNonQueryAsync(
                 commandText, CommandType.Text, parameters.ToArray()
             );
        }

        public async Task DeleteClaimFromUserAsync(string userName, AccountClaims claimName)
        {
            var commandText = @"DELETE [AccountClaims] WHERE Account_UserName = @UserName AND ClaimName = @ClaimName";

            var parameters = new List<DbParameter>();
            parameters.AddParameter("@UserName", userName);
            parameters.AddParameter("@ClaimName", claimName);

            _ = await _databaseRepository.ExecuteNonQueryAsync(
                 commandText, CommandType.Text, parameters.ToArray()
             );
        }
    }

    public record AccountRecord(
        string UserName,
        string PasswordHash,
        DateTime CreationDate,
        string CreatedBy,
        DateTime LastModifiedDate,
        string LastModifiedBy,
        string RoleName,
        string ClaimName
    );

    public static class AccountRecordExtensions
    {
        public static AccountRecord ToRecord(this DbDataReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(paramName: nameof(reader));

            var userName = !reader.IsDBNull(0) ? reader.GetString(0) : string.Empty;
            var passwordHash = !reader.IsDBNull(1) ? reader.GetString(1) : string.Empty;
            var creationDate = !reader.IsDBNull(2) ? reader.GetDateTime(2) : default;
            var createdBy = !reader.IsDBNull(3) ? reader.GetString(3) : string.Empty;
            var lastModifiedDate = !reader.IsDBNull(4) ? reader.GetDateTime(4) : default;
            var lastModifiedBy = !reader.IsDBNull(5) ? reader.GetString(5) : string.Empty;

            var roleName = !reader.IsDBNull(6) ? reader.GetString(6) : string.Empty;
            var claimName = !reader.IsDBNull(7) ? reader.GetString(7) : string.Empty;

            return new AccountRecord(
                userName,
                passwordHash,
                creationDate,
                createdBy,
                lastModifiedDate,
                lastModifiedBy,
                roleName,
                claimName
            );
        }

        public static Dictionary<string, Account> ToUserNameAccountDictionary(this ICollection<AccountRecord> records)
        {
            var map = new Dictionary<string, Account>();
            if (records != null)
            {
                foreach (var record in records)
                {
                    if (!map.TryGetValue(record.UserName, out _))
                    {
                        var account = new Account(record.UserName, record.PasswordHash)
                        {
                            CreationDate = record.CreationDate,
                            CreatedBy = record.CreatedBy,
                            LastModifiedDate = record.LastModifiedDate,
                            LastModifiedBy = record.LastModifiedBy
                        };
                        map.Add(record.UserName, account);
                    }

                    if (!string.IsNullOrWhiteSpace(record.RoleName))
                        map[record.UserName].Roles.Add(record.RoleName);

                    if (!string.IsNullOrWhiteSpace(record.ClaimName))
                    {
                        if (Enum.TryParse<AccountClaims>(record.ClaimName, out var accountClaim))
                            map[record.UserName].Claims.Add(accountClaim);
                    }
                }
            }
            return map;
        }
    }
}
