# SampleDotnetCleanArchitecture

## Use Case: Client Management System

### Overview

- **Goal**: Develop an API for managing client records.
- **Users**: Company employees, internal systems, and authorized partners.
- **Requirements**: Authentication and authorization, data validation, error logging and database integration.

### API Features

#### 1. Register New Client (Create)
- **Endpoint**: `POST /api/clients`
- **Description**: Adds a new client to the system.
- **Input Payload**: FirstName, LastName, BirthDate.
- **Validations**: Check if all data was filled.
- **Response**: Details of the created client.

#### 2. Retrieve Clients (Read)
- **Endpoint**: `GET /api/clients` and `GET /api/clients/{id}`
- **Description**: Returns a list of all clients or the details of a specific client.
- **Response**: List of clients or details of a client.

#### 3. Update Client Information (Update)
- **Endpoint**: `PUT /api/clients/{id}`
- **Description**: Updates the information of an existing client.
- **Input Payload**: FirstName, LastName, BirthDate
- **Validations**: Verify if the client exists, validate data changes.
- **Response**: Updated client details.

#### 4. Remove Client (Delete)
- **Endpoint**: `DELETE /api/clients/{id}`
- **Description**: Removes a client from the system.
- **Validations**: Check if the client exists, validate business rules for removal.
- **Response**: Confirmation of removal.

### Security and Compliance

- **Authentication**: Use JWT tokens authenticate requests.
- **Authorization**: Implement Policies based on Claims user claims.
- **Logs**: Record activities of creation, update, and deletion for auditing purposes.

### Technical Considerations

- **Database**: Use SQL Server 2022.
- **API Documentation**: Use OpenAPI (Swagger) to document the API.