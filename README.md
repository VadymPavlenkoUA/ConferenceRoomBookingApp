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
