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
- Prevent double booking of the same conference hall for overlapping time periods
- Ensure that selected services are available for the chosen conference hall
- Check which conference halls are available for a specified capacity and time period
- Calculate the rental price based on the booking duration and time-based pricing rules
- Calculate the total booking cost, including the hall rental and selected services
- Prevent deletion of halls and services that are currently used by existing bookings or hall configurations
- Provide statistical reports on bookings, revenue, hall popularity, and service popularity


## Architecture

The project follows a layered architecture with separation of responsibilities:

- **API** — HTTP controllers, middleware, Swagger configuration and application startup
- **Application** — business logic, DTOs, service interfaces and repository interfaces
- **Domain** — core business entities and relationships
- **Infrastructure** — Entity Framework Core, database context, repository implementations and database seeding

The application uses Dependency Injection to provide dependencies between layers.

### Main Technical Decisions

- **ASP.NET Core Web API** — used to build the REST API
- **Entity Framework Core** — used for database access and ORM
- **Microsoft SQL Server** — used as the relational database
- **Repository Pattern** — separates data access logic from business logic
- **Unit of Work** — provides a common way to persist changes through the shared `DbContext`
- **Dependency Injection** — used to manage application dependencies
- **Swagger / OpenAPI** — used to document and test API endpoints
- **Middleware** — used for centralized exception handling
- **Rate Limiting** — limits the number of requests from a single client to protect the API from excessive load


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

- Get overall booking statistics for a specified period
- Calculate total revenue and average booking price
- Get hall popularity statistics
- Get service popularity statistics

### API Protection and Error Handling

- Centralized exception handling with consistent HTTP responses
- Rate limiting of 100 requests per minute per client IP address
- Input validation for booking, hall, service, and report requests
