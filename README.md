# Event Booking System

A comprehensive event booking platform built with ASP.NET Core, featuring a Web API backend and MVC frontend. This system allows users to browse events, purchase tickets, manage bookings, and provides administrators with tools to manage the entire platform.

## Repository Structure

This repository contains two main components:
- **API Project**: Backend RESTful API handling data and business logic
- **MVC Project**: Frontend web application providing the user interface

## Features

### Core Features
- **User Authentication**: Register, login, and password reset with email verification
- **Event Management**: Browse, search, and filter events with detailed information
- **Booking System**: Book VIP or admission tickets with booking confirmation
- **User Profiles**: Manage personal information and view booking history
- **Admin Panel**: Comprehensive tools for managing events, users, categories, and bookings

### Technical Features
- **Secure Authentication**: JWT-based authentication with role-based authorization
- **Responsive Design**: User-friendly interface that works across devices
- **Pagination**: Efficient data loading with pagination across all listings
- **Search Functionality**: Fast and relevant search across events, users, and bookings
- **Dashboard Analytics**: Visual representation of system data for administrators

## Technologies Used

### Backend
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Identity Framework
- JWT Authentication

### Frontend
- ASP.NET Core MVC
- Bootstrap
- JavaScript/jQuery
- Chart.js for analytics

## Getting Started

### Prerequisites
- .NET 6.0 SDK or later
- SQL Server (LocalDB or full installation)
- Visual Studio 2022 (recommended) or Visual Studio Code

### Installation Steps

1. **Clone the repository**
   ```
   git clone https://github.com/ahmedashraf0001/ATC_01009970776
   cd Event-booking-system-project
   ```

2. **Set up the database**
   - Open the solution in Visual Studio
   - Open Package Manager Console
   - Run `Update-Database` to create and seed the database

3. **Run the API project**
   - Set the API project as the startup project
   - Press F5 or click the Run button
   - Note the URL where the API is running

4. **Configure and run the MVC project**
   - Update the API URL in `appsettings.json` of the MVC project if needed
   - Set the MVC project as the startup project
   - Press F5 or click the Run button

5. **Access the application**
   - The MVC application will open in your default browser
   - You can also access the Swagger documentation for the API at its URL

### Default Admin Account
- Username: Admin
- Password: Admin@123

## Detailed Documentation

For more detailed information about each component:

- [API Documentation](./API/API_README.md) - Details about the backend API, endpoints, and data structures
- [MVC Documentation](./MVC/MVC_README.md) - Information about the frontend application, views, and user interactions

## System Workflow

1. **User Registration/Login**: Users create accounts or log in to existing accounts
2. **Browse Events**: Users can view featured events or browse all events with filtering options
3. **Event Details**: Users view comprehensive information about specific events
4. **Book Tickets**: Users can book VIP or admission tickets for events
5. **Manage Bookings**: Users can view their bookings and cancel if needed
6. **Admin Management**: Administrators can manage all aspects of the system through the admin panel

## Screenshots

[Consider adding screenshots of key pages/features here]

## Future Enhancements

- Payment gateway integration
- Email notifications for booking updates
- Social media sharing functionality
- Event recommendations based on user preferences
- Mobile application development

