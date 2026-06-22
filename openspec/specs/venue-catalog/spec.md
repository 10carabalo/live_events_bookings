# venue-catalog Specification

## Purpose
TBD - created by archiving change foundation-walking-skeleton. Update Purpose after archive.
## Requirements
### Requirement: List reference venues

The system SHALL expose `GET /api/venues` returning all seeded reference venues. Each venue in the
response MUST include `id`, `name`, `capacity`, and `city`. The endpoint MUST return HTTP 200 with
a JSON array.

#### Scenario: Returns all seeded venues

- **WHEN** a client sends `GET /api/venues`
- **THEN** the response status is 200
- **AND** the body is a JSON array containing the three seeded venues (Auditorio Central, Sala
  Norte, Arena Sur)
- **AND** each item includes `id`, `name`, `capacity`, and `city`

#### Scenario: Seed data values are correct

- **WHEN** a client sends `GET /api/venues`
- **THEN** the venue named "Auditorio Central" has capacity 200 and city "Bogotá"
- **AND** "Sala Norte" has capacity 50 and city "Bogotá"
- **AND** "Arena Sur" has capacity 500 and city "Medellín"

### Requirement: Venues are read-only reference data

The system SHALL NOT expose any endpoint to create, update, or delete venues. Venues exist only as
seeded reference data.

#### Scenario: No write endpoints for venues

- **WHEN** a client sends `POST`, `PUT`, `PATCH`, or `DELETE` to `/api/venues` (or a venue
  sub-resource)
- **THEN** the request is not handled by a venue write operation (no such route exists)

