# api-foundation Specification

## Purpose
TBD - created by archiving change foundation-walking-skeleton. Update Purpose after archive.
## Requirements
### Requirement: Errors are returned as RFC 7807 ProblemDetails

The API SHALL return all error responses using the RFC 7807 `application/problem+json`
`ProblemDetails` format, produced by global exception-handling middleware. Internal error responses
MUST NOT leak stack traces or implementation details to the client.

#### Scenario: Unknown route returns ProblemDetails 404

- **WHEN** a client requests a route that does not exist
- **THEN** the response status is 404
- **AND** the body is a `ProblemDetails` object with at least `status` and `title`

#### Scenario: Unhandled exception returns ProblemDetails 500 without internals

- **WHEN** an unhandled exception occurs while processing a request
- **THEN** the response status is 500
- **AND** the body is a `ProblemDetails` object with a generic message
- **AND** the body does not contain a stack trace or framework-internal details

### Requirement: OpenAPI document and Swagger UI are available

The API SHALL publish a built-in OpenAPI document and serve an interactive UI (Swagger UI) in the
development environment.

#### Scenario: OpenAPI document is served

- **WHEN** a client requests the OpenAPI document endpoint
- **THEN** the response status is 200
- **AND** the body is a valid OpenAPI document describing the available endpoints

#### Scenario: Swagger UI is reachable in development

- **WHEN** the application runs in the Development environment and a client opens `/swagger`
- **THEN** the Swagger UI page loads

### Requirement: CORS permits the configured SPA origin

The API SHALL apply a CORS policy that allows requests from the configured Angular application
origin.

#### Scenario: Allowed origin receives CORS headers

- **WHEN** a browser request from the configured Angular origin calls an API endpoint
- **THEN** the response includes the `Access-Control-Allow-Origin` header for that origin

### Requirement: Database schema and reference seed present on startup

On application startup the system SHALL ensure the database schema exists (apply pending
migrations) and the reference venues are seeded, so the API is usable without manual setup.

#### Scenario: Migration and seed applied at startup

- **WHEN** the application starts against an empty database file
- **THEN** the schema is created by applying the EF Core migration
- **AND** the three reference venues are present in the database

