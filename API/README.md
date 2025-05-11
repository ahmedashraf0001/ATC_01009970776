# Event Booking System API

This repository contains the backend API for an Event Booking System, built using ASP.NET Web API. The API provides endpoints for user authentication, event management, booking operations, category management, and user management.

## Features

- **Authentication**: Register, login, and password reset functionality with email token verification
- **Event Management**: Create, read, update, delete, and search events
- **Booking Management**: Book events, view bookings, and cancel bookings
- **Category Management**: Create, manage, and organize events by categories
- **User Management**: User profile management and role-based authorization
- **Admin Dashboard**: Analytics and statistics for administrators

## Technologies Used

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Identity Framework for authentication
- JSON Web Tokens (JWT)
- Swagger/OpenAPI documentation

## Prerequisites

- .NET 6.0 SDK or later
- SQL Server (LocalDB or full installation)
- Visual Studio 2022 (recommended) or Visual Studio Code

## Getting Started

### Database Setup

1. Open the solution in Visual Studio
2. Open the Package Manager Console (Tools > NuGet Package Manager > Package Manager Console)
3. Run the following command to apply migrations and create the database:

```
Update-Database
```

This will create the database with seeded data including events and an admin account.

### Running the API

1. Set the API project as the startup project in Visual Studio
2. Press F5 or click the Run button to start the API
3. The Swagger UI will open automatically, showing all available endpoints

## API Endpoints

### Authentication

- POST `/api/auth/register` - Register a new user
- POST `/api/auth/login` - Authenticate a user
- POST `/api/auth/forgot-password` - Request password reset email
- POST `/api/auth/reset-password` - Reset password with token

### Events

- GET `/api/events` - Get all events
- GET `/api/events/{id}` - Get event by ID
- POST `/api/events/create` - Create a new event
- PUT `/api/events/edit/{eventId}` - Update an event
- DELETE `/api/events/delete/{eventId}` - Delete an event
- GET `/api/events/list/{pageNumber}/{pageSize}` - Get paginated events
- GET `/api/events/search` - Search events

### Bookings

- POST `/api/booking/book` - Book an event
- DELETE `/api/booking/unbook/{bookingId}` - Cancel a booking
- GET `/api/booking/id/{bookingId}` - Get booking by ID
- GET `/api/booking/list/all/{pageNumber}/{pageSize}` - Get all bookings (paginated)
- GET `/api/booking/list/current/{pageNumber}/{pageSize}` - Get current user's bookings
- GET `/api/booking/search/{pageNumber}/{pageSize}` - Search bookings

### Categories

- POST `/api/categories/create` - Create a new category
- DELETE `/api/categories/delete/{categoryname}` - Delete a category
- PUT `/api/categories/rename/{eventId}` - Rename a category
- GET `/api/categories/list/{pageNumber}/{pageSize}` - Get paginated categories
- GET `/api/categories/all` - Get all categories

### Users

- GET `/api/user/details` - Get current user details
- GET `/api/user/username/{username}` - Get user by username
- GET `/api/user/list/{pageNumber}/{pageSize}` - Get all users (paginated)
- GET `/api/user/search` - Search users
- PUT `/api/user/edit` - Update user profile
- DELETE `/api/user/delete/{userId}` - Delete user (admin only)

## Authentication

The API uses JWT (JSON Web Tokens) for authentication. After logging in, you will receive a token that must be included in the Authorization header of subsequent requests:

```
Authorization: Bearer {your-token}
```

## Default Admin Account

The system is seeded with an admin account:

- Username: Admin
- Password: Admin@123

## Error Handling

The API returns standard HTTP status codes:

- 200 - OK: The request was successful
- 400 - Bad Request: The request was invalid
- 401 - Unauthorized: Authentication is required
- 403 - Forbidden: The user does not have permission
- 404 - Not Found: The requested resource was not found
- 500 - Internal Server Error: An unexpected error occurred

## Documentation

Full API documentation is available through Swagger UI when the API is running. Access it at:

```
https://localhost:{port}/swagger
```

## License

[Specify your license here]

## Contact

[Your contact information]