# Conference Room Booking API

## Overview

Conference Room Booking API is a RESTful Web API for managing conference rooms, additional services and room bookings.

The system allows users to manage conference halls and available services, create and update bookings, check room availability, calculate booking costs based on the selected time period, and generate booking and popularity reports.

The project is built using ASP.NET Core and follows a layered architecture with a clear separation of responsibilities between the API, Application, Domain, and Infrastructure layers.


## Business Requirements

The system is designed to simplify the management and booking of conference rooms.

The main business requirements are:

- Manage conference halls, including their names, capacity, hourly rental rates, and available services
- Manage additional services that can be provided with a hall booking
- Create, view, update, and delete room bookings
- Prevent deletion of halls or services that are currently in use
- Ensure that selected services are available for the chosen conference hall
- Check which conference halls are available for a specified capacity and time period
- Calculate the rental price based on the booking duration and time-based pricing rules
- Calculate the total booking cost, including the hall rental and selected services
- Prevent deletion of halls and services that are currently used by existing bookings or hall configurations
- Provide statistical reports on bookings, revenue, hall popularity, and service popularity


## Architecture

The project follows a layered architecture with separation of responsibilities:

- **API** – HTTP controllers, middleware, Swagger configuration and application startup
- **Application** – business logic, DTOs, service interfaces and repository interfaces
- **Domain** – core business entities and relationships
- **Infrastructure** – Entity Framework Core, database context, repository implementations and database seeding

The application uses Dependency Injection to provide dependencies between layers.

### Main Technical Decisions

- **ASP.NET Core Web API** – used to build the REST API
- **Entity Framework Core** – used for database access and ORM
- **Microsoft SQL Server** – used as the relational database
- **Repository Pattern** – separates data access logic from business logic
- **Unit of Work** – provides a common way to persist changes through the shared `DbContext`
- **Dependency Injection** – used to manage application dependencies
- **Swagger / OpenAPI** – used to document and test API endpoints
- **Middleware** – used for centralized exception handling
- **Rate Limiting** – limits the number of requests from a single client to protect the API from excessive load


## Features

### Conference Halls

- Create, view, update, and delete conference halls
- Configure hall capacity and hourly rental rate
- Assign available services to each hall
- Search for available halls by capacity and booking time

### Additional Services

- Create, view, update, and delete additional services
- Set a price for each service
- Prevent deletion of services that are used by halls or bookings

### Bookings

- Create, view, update, and delete bookings
- Select a conference hall and additional services
- Validate booking time intervals
- Prevent overlapping bookings for the same hall
- Verify that selected services are available for the selected hall
- Automatically calculate hall rental, services, and total booking prices

### Reports

- Get overall booking statistics for a specified period (total bookings, total revenue, average booking price)
- Get hall popularity statistics
- Get service popularity statistics

### API Protection and Error Handling

- Centralized exception handling with consistent HTTP responses
- Rate limiting of 100 requests per minute per client IP address
- Input validation for booking, hall, service, and report requests


## Technologies

- **C#**
- **.NET 10**
- **ASP.NET Core Web API**
- **Entity Framework Core 10**
- **Microsoft SQL Server**
- **Swagger / OpenAPI**
- **xUnit**
- **Moq**
- **Git**


## Database

The application uses **Microsoft SQL Server** with **Entity Framework Core**.

The database contains the following main entities:

- **Hall** – conference hall with capacity and hourly rental rate
- **Service** – additional service that can be provided in a hall
- **Booking** – reservation of a conference hall for a specific time period
- **HallServiceItem** – many-to-many relationship between halls and services
- **BookingServiceItem** – services selected for a specific booking

### Relationships

- One hall can have many bookings
- A hall can provide multiple services
- A service can be available in multiple halls
- A booking can include multiple services
- A service can be included in multiple bookings


### Initial Seed Data

On first run, the database is seeded with the following initial data:

| Hall   | Capacity | Hourly Rate |
|--------|----------|-------------|
| Hall A | 50       | 2000 UAH    |
| Hall B | 100      | 3500 UAH    |
| Hall C | 30       | 1500 UAH    |

| Service   | Price   |
|-----------|---------|
| Projector | 500 UAH |
| Wi-Fi     | 300 UAH |
| Sound     | 700 UAH |


Entity Framework Core migrations are used to create and update the database schema.

The application automatically applies pending migrations and seeds initial data when started.


## API Documentation

The API is documented using **Swagger / OpenAPI**.

Swagger UI provides an interactive interface for exploring and testing all available API endpoints, including request parameters, request bodies, response types, and HTTP status codes.

When running the application in the Development environment, Swagger UI is available at:

```text
https://localhost:<port>/swagger
```
![Swagger UI](https://github.com/user-attachments/assets/c397f3c9-cc54-446c-8fbd-230b77fdccdc)

![Swagger Endpoint](https://github.com/user-attachments/assets/cb5757b3-04f3-4281-8a3f-b817129a271a)


## Testing

The project includes unit tests covering the main business logic of the application.

The tests cover:

- Booking creation and update validation
- Prevention of overlapping bookings
- Validation of hall and service availability
- Hall and service validation
- Rental price calculation for different time periods
- Report period validation

The tests are implemented using **xUnit** and **Moq**.


## Getting Started

### Prerequisites

Make sure the following tools are installed:

- **.NET 10 SDK**
- **SQL Server** (Express, LocalDB, or full instance)
- **Git**

### Installation

1. Clone the repository:

```bash
git clone <repository-url>
cd ConferenceRoomBooking
```

2. Verify the database connection string in:

```text
ConferenceRoomBooking.API/appsettings.json
```

The default configuration uses SQL Server:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS01;Database=ConfBookDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

If you use another SQL Server instance, update the `Server` value in the connection string accordingly.

### Run the Application

Start the application from the `ConferenceRoomBooking.API` project:

```bash
dotnet run --project ConferenceRoomBooking.API
```

On application startup:

- Pending Entity Framework Core migrations are applied automatically
- The database schema is created or updated automatically
- Initial halls and services are added to the database

After the application starts, open Swagger UI to explore and test the API:

```text
https://localhost:<port>/swagger
```


## Project Structure

```text
ConferenceRoomBooking/
├── ConferenceRoomBooking.API/
│   ├── Controllers/
│   ├── Middleware/
│   └── Program.cs
│
├── ConferenceRoomBooking.Application/
│   ├── DTOs/
│   ├── Interfaces/
│   └── Services/
│
├── ConferenceRoomBooking.Domain/
│   └── Entities/
│
├── ConferenceRoomBooking.Infrastructure/
│   ├── Data/
│   │   ├── Configurations/
│   │   └── Seed/
│   ├── Migrations/
│   └── Repositories/
│
└── ConferenceRoomBooking.Tests/
    └── Tests/
```

### Layer Responsibilities

- **API** – handles HTTP requests, controllers, middleware, Swagger and application configuration
- **Application** – contains business logic, DTOs and abstractions for services and repositories
- **Domain** – contains core business entities and their relationships
- **Infrastructure** – implements data access, Entity Framework Core configuration, repositories and database seeding
- **Tests** – contains automated tests for the application's business logic


## Possible Improvements

The current implementation covers the main requirements of the task.  
For a production application, the following improvements could be considered:

**Architecture**
- `IUnitOfWork` provides a simple abstraction over `DbContext.SaveChangesAsync`. 
  A separate implementation could be introduced if more complex transaction management is required.

**Concurrency**
- The current booking overlap check may have a race condition if multiple requests try to book the same hall at the same time.
  A production application could use database transactions or other concurrency control mechanisms.

**Security**
- Authentication and authorization were not implemented because they were outside the scope of the task.

**Scalability**
- Add pagination and filtering for large collections
- Add caching for frequently requested data such as halls and services

**Business Rules**
- Bookings are restricted to the 06:00–23:00 time range. This was treated as a reasonable business rule because the pricing system is also based on these time periods.
- The initial seed data assigns specific services to specific halls as example data.
