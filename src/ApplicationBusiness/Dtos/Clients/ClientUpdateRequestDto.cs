﻿using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;

namespace SampleDotnetCleanArchitecture.ApplicationBusiness.Dtos.Clients
{
    public record ClientUpdateRequestDto(string FirstName, string LastName, DateTime BirthDate)
    {
        public Client ToClient() =>
            new Client(FirstName, LastName, BirthDate);
    }
}
