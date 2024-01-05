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

        public void Validate()
        {
            if (string.IsNullOrEmpty(FirstName))
                throw new Exception("FirstName is required");

            if (string.IsNullOrEmpty(LastName))
                throw new Exception("LastName is required");

            if (BirthDate == DateTime.MinValue)
                throw new Exception("BirthDate is required");
        }
    }
}
