using System.Data.Common;
using System.Data;
using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;
using SampleDotnetCleanArchitecture.EnterpriseBusiness.Interfaces;
using Microsoft.Data.SqlClient;

namespace SampleDotnetCleanArchitecture.Infrastructure.SqlServer
{
    public class ClientRepository : IClientRepository, IDisposable
    {
        private readonly Lazy<SqlConnection> _dbConnectionLazy;

        public ClientRepository(IDbConnectionProject dbConnectionProject)
        {
            _dbConnectionLazy = new Lazy<SqlConnection>(() => dbConnectionProject.CreateConnection());
        }

        public async Task<ICollection<Client>> ListAsync()
        {
            using var command = _dbConnectionLazy.Value.CreateCommand();
            command.CommandText = @"
SELECT
    a.Id,
    a.FirstName,
    a.LastName,
    a.BirthDate,

    a.CreationDate,
    a.CreatedBy,
    a.LastModifiedDate,
    a.LastModifiedBy
FROM [Client] a
";
            command.CommandType = CommandType.Text;

            using var reader = await command.ExecuteReaderAsync();

            var map = new Dictionary<long, Client>();
            while (reader.Read())
            {
                var id = !reader.IsDBNull(0) ? reader.GetInt64(0) : default;
                var firstName = !reader.IsDBNull(1) ? reader.GetString(1) : default;
                var lastName  = !reader.IsDBNull(2) ? reader.GetString(2) : default;
                var birthDate = !reader.IsDBNull(3) ? reader.GetDateTime(3) : default;
                var creationDate = !reader.IsDBNull(4) ? reader.GetDateTime(4) : default;
                var createdBy = !reader.IsDBNull(5) ? reader.GetString(5) : default;
                var lastModifiedDate = !reader.IsDBNull(6) ? reader.GetDateTime(6) : default;
                var lastModifiedBy = !reader.IsDBNull(7) ? reader.GetString(7) : default;

                if (!map.TryGetValue(id, out var Client))
                {
                    Client = new Client
                    {
                        BirthDate = birthDate,
                        CreatedBy = createdBy,
                        CreationDate = creationDate,
                        FirstName = firstName,
                        Id = id,
                        LastName = lastName,
                        LastModifiedBy = lastModifiedBy,
                        LastModifiedDate = lastModifiedDate
                    };
                    map.Add(id, Client);
                }
            }
            return map.Values.ToList();
        }

        public async Task<Client?> GetByIdAsync(long id)
        {
            using var command = _dbConnectionLazy.Value.CreateCommand();
            command.CommandText = @"
SELECT
    a.Id,
    a.FirstName,
    a.LastName,
    a.BirthDate,

    a.CreationDate,
    a.CreatedBy,
    a.LastModifiedDate,
    a.LastModifiedBy
FROM [Client] a
WHERE a.Id = @id
";
            command.CommandType = CommandType.Text;
            command.AddParameter("@id", id);

            using var reader = await command.ExecuteReaderAsync();

            var map = new Dictionary<long, Client>();
            while (reader.Read())
            {
                var firstName = !reader.IsDBNull(1) ? reader.GetString(1) : default;
                var lastName = !reader.IsDBNull(2) ? reader.GetString(2) : default;
                var birthDate = !reader.IsDBNull(3) ? reader.GetDateTime(3) : default;
                var creationDate = !reader.IsDBNull(4) ? reader.GetDateTime(4) : default;
                var createdBy = !reader.IsDBNull(5) ? reader.GetString(5) : default;
                var lastModifiedDate = !reader.IsDBNull(6) ? reader.GetDateTime(6) : default;
                var lastModifiedBy = !reader.IsDBNull(7) ? reader.GetString(7) : default;

                if (!map.TryGetValue(id, out var Client))
                {
                    Client = new Client
                    {
                        BirthDate = birthDate,
                        CreatedBy = createdBy,
                        CreationDate = creationDate,
                        FirstName = firstName,
                        Id = id,
                        LastName = lastName,
                        LastModifiedBy = lastModifiedBy,
                        LastModifiedDate = lastModifiedDate
                    };
                    map.Add(id, Client);
                }
            }
            return map.Values.FirstOrDefault();
        }

        public async Task<Client> CreateAsync(Client client)
        {
            using var command = _dbConnectionLazy.Value.CreateCommand();
            command.CommandText = @"
INSERT INTO [Client] (
    FirstName,
    LastName,
    BirthDate,
    CreationDate,
    CreatedBy,
    LastModifiedDate,
    LastModifiedBy
) VALUES (
    @FirstName,
    @LastName,
    @BirthDate,
    @CreationDate,
    @CreatedBy,
    @LastModifiedDate,
    @LastModifiedBy
)
; SELECT SCOPE_IDENTITY();
";
            command.CommandType = CommandType.Text;
            command.AddParameter("@FirstName", client.FirstName);
            command.AddParameter("@LastName", client.LastName);
            command.AddParameter("@BirthDate", client.BirthDate);
            command.AddParameter("@CreationDate", client.CreationDate);
            command.AddParameter("@CreatedBy", client.CreatedBy);
            command.AddParameter("@LastModifiedDate", client.LastModifiedDate);
            command.AddParameter("@LastModifiedBy", client.LastModifiedBy);

            var objId = await command.ExecuteScalarAsync();
            if (objId != null)
                client.Id = Convert.ToInt64(objId);

            return client;
        }

        public async Task UpdateAsync(long id, Client client)
        {
            using var command = _dbConnectionLazy.Value.CreateCommand();
            command.CommandText = @"
UPDATE [Client]
SET
      FirstName         = @FirstName
    , LastName          = @LastName 
    , BirthDate         = @BirthDate
    , CreationDate      = @CreationDate
    , CreatedBy         = @CreatedBy
    , LastModifiedDate  = @LastModifiedDate
    , LastModifiedBy    = @LastModifiedBy
WHERE Id = @Id
";
            command.CommandType = CommandType.Text;
            command.AddParameter("@FirstName", client.FirstName);
            command.AddParameter("@LastName", client.LastName);
            command.AddParameter("@BirthDate", client.BirthDate);
            command.AddParameter("@CreationDate", client.CreationDate);
            command.AddParameter("@CreatedBy", client.CreatedBy);
            command.AddParameter("@LastModifiedDate", client.LastModifiedDate);
            command.AddParameter("@LastModifiedBy", client.LastModifiedBy);
            command.AddParameter("@Id", id);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(long id)
        {
            using var command = _dbConnectionLazy.Value.CreateCommand();
            command.CommandText = @"DELETE FROM [Client] WHERE Id = @id";
            command.CommandType = CommandType.Text;
            command.AddParameter("@id", id);

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
