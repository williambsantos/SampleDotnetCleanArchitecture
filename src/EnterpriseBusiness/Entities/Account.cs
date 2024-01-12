namespace SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities
{
    public sealed class Account
    {
        public string UserName { get; private set; }
        public string PasswordHash { get; private set; }
        
        public DateTime? CreationDate { get; set; }
        public string? CreatedBy { get; set; } = string.Empty;
        public DateTime? LastModifiedDate { get; set; }
        public string? LastModifiedBy { get; set; } = string.Empty;

        public ICollection<string> Roles { get; private set; } = new List<string>();
        public ICollection<AccountClaims> Claims { get; private set; } = new List<AccountClaims>();
        public ICollection<AccountToken> Tokens { get; private set; } = new List<AccountToken>();

        public Account(string username)
        {
            UserName = username;
            PasswordHash = string.Empty;
        }

        public Account(string username, string password)
        {
            UserName = username;
            PasswordHash = password;
        }

        public Account(string username, string password, ICollection<string>? roles, ICollection<AccountClaims>? claims) : this(username)
        {
            PasswordHash = password;

            if (roles != null)
            {
                foreach (var role in roles)
                    Roles.Add(role);
            }

            if (claims != null)
            {
                foreach (var claim in claims)
                    Claims.Add(claim);
            }
        }

        public void SetPassword(string password)
        {
            PasswordHash = password;
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(UserName))
                throw new DomainValidationException("Username is required");

            if (string.IsNullOrWhiteSpace(PasswordHash))
                throw new DomainValidationException("Password is required");
        }
    }
}
