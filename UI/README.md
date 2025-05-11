# Event Booking System MVC Frontend

This repository contains the frontend MVC application for an Event Booking System, built using ASP.NET MVC. The application provides a user-friendly interface for browsing events, booking tickets, managing user profiles, and an admin panel for comprehensive system management.

## Features

### User Features
- **Authentication**: Register, login, and forgot password functionality with email verification
- **Event Browsing**: View featured events and browse all events with pagination
- **Event Details**: View comprehensive event information
- **Booking System**: Book VIP or admission tickets for events
- **Booking Management**: View booking receipts and cancel bookings
- **User Profile**: Edit personal information and password
- **Booking History**: View all bookings with status information

### Admin Features
- **Dashboard**: Overview of system statistics and revenue information
- **Event Management**: Create, edit, and delete events
- **Category Management**: Create and manage event categories
- **User Management**: View and manage user accounts
- **Booking Management**: View and manage all bookings in the system

## Technologies Used

- ASP.NET Core MVC
- Bootstrap for responsive design
- JavaScript/jQuery for client-side interactions
- Font Awesome for icons
- Chart.js for dashboard visualizations

## Prerequisites

- .NET 6.0 SDK or later
- The Event Booking System API running
- Visual Studio 2022 (recommended) or Visual Studio Code

## Getting Started

### Configuration

1. Open the solution in Visual Studio
2. Locate the `appsettings.json` file and ensure the API URL is correctly configured:

```json
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:7001/api/"
  }
}
```

Replace the URL with the actual URL where your API is running.

### Running the Application

1. Set the MVC project as the startup project in Visual Studio
2. Ensure the API project is already running
3. Press F5 or click the Run button to start the MVC application
4. The application will open in your default browser

## Application Structure

### Public Pages
- **Home Page**: Displays featured events and navigation options
- **Events Page**: Lists all events with pagination and filtering
- **Event Details Page**: Shows comprehensive information about a specific event
- **Authentication Pages**: Register, Login, and Forgot Password forms

### Authenticated User Pages
- **Booking Receipt**: Confirmation page after booking an event
- **Profile Page**: User profile management and password change
- **My Bookings**: List of all the user's bookings with status information

### Admin Panel
- **Dashboard**: Overview of system statistics (users, events, bookings, revenue)
- **Event Management**: CRUD operations for events
- **Category Management**: CRUD operations for categories
- **User Management**: View and manage users
- **Booking Management**: View and manage all bookings

## User Roles

The application supports two main roles:

1. **Regular User**: Can browse events, book tickets, and manage their own profile and bookings
2. **Administrator**: Has access to the admin panel and can manage all aspects of the system

## Default Admin Account

The system comes with a pre-configured admin account:

- Username: Admin
- Password: Admin@123

## Usage Guide

### For Regular Users

1. **Registration**: Create a new account from the Register page
2. **Browse Events**: View all available events on the Events page
3. **Event Details**: Click on an event to see detailed information
4. **Book Event**: Click "Book Now" on an event detail page to book a ticket (VIP or regular admission)
5. **View Bookings**: Access your bookings from the profile dropdown menu
6. **Cancel Booking**: Cancel any booking from the My Bookings page
7. **Profile Management**: Update your profile information or change password

### For Administrators

1. **Access Admin Panel**: Log in with admin credentials and click on "Admin Panel" in the navigation menu
2. **Dashboard**: View system statistics and analytics
3. **Manage Events**: Create, edit, or delete events
4. **Manage Categories**: Create or manage event categories
5. **Manage Users**: View user information and modify user roles if needed
6. **Manage Bookings**: View and manage all bookings in the system

## Troubleshooting

- **API Connection Issues**: Ensure the API is running and the BaseUrl in appsettings.json is correct
- **Authentication Errors**: Verify user credentials and ensure the API is handling authentication requests properly
- **Missing Features**: Certain features might require specific API endpoints to be functioning correctly

