# SampleDotnetCleanArchitecture

## Objective: sample to Clean Architecture with .Net C# Language

### Create an API

**API(Presenter) -> ApplicationBusiness (UseCases)**

**ApplicationBusiness (UseCases) -> EnterpriseBusiness (Entities)**

**API map interfaces of EnterpriseBusiness to**
* Infrastructure.SqlServer (Infra)
* Infrastructure.Security (Infra)