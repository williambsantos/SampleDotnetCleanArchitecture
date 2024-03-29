namespace SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities
{
    public class Client
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }

        public DateTime? CreationDate { get; set; }
        public string? CreatedBy { get; set; } = string.Empty;
        public DateTime? LastModifiedDate { get; set; }
        public string? LastModifiedBy { get; set; } = string.Empty;

        public int Age => DateTime.Now.Year - BirthDate.Year;        

        public Client(string firstName, string lastName, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
        }

        public Client(long id, string firstName, string lastName, DateTime birthDate, DateTime? creationDate, string? createdBy, DateTime? lastModifiedDate, string? lastModifiedBy)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.BirthDate = birthDate;
            this.CreationDate = creationDate;
            this.CreatedBy = createdBy;
            this.LastModifiedDate = lastModifiedDate;
            this.LastModifiedBy = lastModifiedBy;
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
                throw new DomainValidationException("FirstName is required");

            if (string.IsNullOrWhiteSpace(LastName))
                throw new DomainValidationException("LastName is required");

            if (BirthDate == default ||
                BirthDate == DateTime.MinValue ||
                BirthDate == DateTime.MaxValue)
                throw new DomainValidationException("BirthDate is required");
        }
    }
}
