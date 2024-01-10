using System.Data;
using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;
using SampleDotnetCleanArchitecture.EnterpriseBusiness.Interfaces;
using SampleDotnetCleanArchitecture.Infrastructure.SqlServer.Extensions;
using SampleDotnetCleanArchitecture.Infrastructure.SqlServer.Interfaces;
using System.Data.Common;

namespace SampleDotnetCleanArchitecture.Infrastructure.SqlServer.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly IDatabaseRepository _databaseRepository;

        public ClientRepository(IDatabaseRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;
        }

        public async Task<ICollection<Client>> ListAsync()
        {
            var commandText = @"
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

            using var reader = await _databaseRepository.ExecuteReaderAsync(
                commandText, CommandType.Text, parameters: null
            );

            var map = new Dictionary<long, Client>();
            while (reader.Read())
            {
                var id = !reader.IsDBNull(0) ? reader.GetInt64(0) : default;
                var firstName = !reader.IsDBNull(1) ? reader.GetString(1) : string.Empty;
                var lastName = !reader.IsDBNull(2) ? reader.GetString(2) : string.Empty;
                var birthDate = !reader.IsDBNull(3) ? reader.GetDateTime(3) : default;
                var creationDate = !reader.IsDBNull(4) ? reader.GetDateTime(4) : default;
                var createdBy = !reader.IsDBNull(5) ? reader.GetString(5) : default;
                var lastModifiedDate = !reader.IsDBNull(6) ? reader.GetDateTime(6) : default;
                var lastModifiedBy = !reader.IsDBNull(7) ? reader.GetString(7) : default;

                if (!map.TryGetValue(id, out var Client))
                {
                    Client = new Client(
                        id,
                        firstName,
                        lastName,
                        birthDate,
                        creationDate,
                        createdBy,
                        lastModifiedDate,
                        lastModifiedBy
                    );

                    map.Add(id, Client);
                }
            }
            return map.Values.ToList();
        }

        public async Task<Client?> GetByIdAsync(long id)
        {
            var commandText = @"
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
            var parameters = new List<DbParameter>();
            parameters.AddParameter("@id", id);

            using var reader = await _databaseRepository.ExecuteReaderAsync(
                commandText, CommandType.Text, parameters: parameters.ToArray()
            );

            var map = new Dictionary<long, Client>();
            while (reader.Read())
            {
                var firstName = !reader.IsDBNull(1) ? reader.GetString(1) : string.Empty;
                var lastName = !reader.IsDBNull(2) ? reader.GetString(2) : string.Empty;
                var birthDate = !reader.IsDBNull(3) ? reader.GetDateTime(3) : default;
                var creationDate = !reader.IsDBNull(4) ? reader.GetDateTime(4) : default;
                var createdBy = !reader.IsDBNull(5) ? reader.GetString(5) : default;
                var lastModifiedDate = !reader.IsDBNull(6) ? reader.GetDateTime(6) : default;
                var lastModifiedBy = !reader.IsDBNull(7) ? reader.GetString(7) : default;

                if (!map.TryGetValue(id, out var Client))
                {
                    Client = new Client(
                        id,
                        firstName,
                        lastName,
                        birthDate,
                        creationDate,
                        createdBy,
                        lastModifiedDate,
                        lastModifiedBy
                    );
                    map.Add(id, Client);
                }
            }
            return map.Values.FirstOrDefault();
        }

        public async Task<Client> CreateAsync(Client client)
        {
            var commandText = @"
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
            var parameters = new List<DbParameter>();

            parameters.AddParameter("@FirstName", client.FirstName);
            parameters.AddParameter("@LastName", client.LastName);
            parameters.AddParameter("@BirthDate", client.BirthDate);
            parameters.AddParameter("@CreationDate", client.CreationDate ?? DateTime.Now);
            parameters.AddParameter("@CreatedBy", client.CreatedBy ?? string.Empty);
            parameters.AddParameter("@LastModifiedDate", client.LastModifiedDate ?? DateTime.Now);
            parameters.AddParameter("@LastModifiedBy", client.LastModifiedBy ?? string.Empty);

            var objId = await _databaseRepository.ExecuteScalarAsync(
                commandText, CommandType.Text, parameters: parameters.ToArray()
            );

            if (objId != null)
                client.Id = Convert.ToInt64(objId);

            return client;
        }

        public async Task UpdateAsync(long id, Client client)
        {
            var commandText = @"
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
            var parameters = new List<DbParameter>();

            parameters.AddParameter("@FirstName", client.FirstName);
            parameters.AddParameter("@LastName", client.LastName);
            parameters.AddParameter("@BirthDate", client.BirthDate);
            parameters.AddParameter("@CreationDate", client.CreationDate ?? DateTime.Now);
            parameters.AddParameter("@CreatedBy", client.CreatedBy ?? string.Empty);
            parameters.AddParameter("@LastModifiedDate", client.LastModifiedDate ?? DateTime.Now);
            parameters.AddParameter("@LastModifiedBy", client.LastModifiedBy ?? string.Empty);
            parameters.AddParameter("@Id", id);

            _ = await _databaseRepository.ExecuteNonQueryAsync(
                commandText, CommandType.Text, parameters: parameters.ToArray()
            );
        }

        public async Task DeleteAsync(long id)
        {
            var commandText = @"DELETE FROM [Client] WHERE Id = @id";
            
            var parameters = new List<DbParameter>();
            parameters.AddParameter("@id", id);

            _ = await _databaseRepository.ExecuteNonQueryAsync(
                commandText, CommandType.Text, parameters: parameters.ToArray()
            );
        }
    }
}
