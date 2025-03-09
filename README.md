# MusicApp - Music Listening & Social Sharing Platform

MusicApp is a music listening application where users can share their music playlists with each other and enjoy listening together in real time. The main goal is to foster a social experience around music, enabling users to create, share, and enjoy collaborative playlists.

## Table of Contents

- [Features](#features)
- [Architecture](#architecture)
  - [Domain Layer](#domain-layer)
  - [Data Layer (Repository Pattern)](#data-layer-repository-pattern)
  - [Business Layer](#business-layer)
  - [Web API Layer](#web-api-layer)
- [Technologies Used](#technologies-used)
- [Installation and Setup](#installation-and-setup)
- [Cloning and Contributing](#cloning-and-contributing)
- [License](#license)

## Features

- **User Registration and Authentication:**  
  Users can register using a unique username, email, first name, last name, and password. Passwords are securely hashed.
  
- **Playlist Management:**  
  Users can create and manage their own playlists.
  
- **Social Interaction:**  
  Users can add friends, share their playlists, and chat or sync their listening sessions.
  
- **Real-Time Listening:**  
  Listen to music simultaneously with friends on the platform.

## Architecture

MusicApp follows a **three-tier architecture** with a clear separation of concerns:

### Domain Layer
- Contains the core business entities (e.g., `AppUser`, `Song`, `Playlist`, `Friendship`, etc.).
- **Location:** `MusicApp.Domain` project

### Data Layer (Repository Pattern)
- Implements the Entity Framework Core DbContext and the generic repository pattern.
- All data access logic is encapsulated in the repositories.
- **Location:** `MusicApp.Data` project
- **Example:**  
  - `IRepository<T>` and `Repository<T>` are defined here.
  - EF Core mappings and composite key configurations are done in `AppDbContext.cs`.

### Business Layer
- Contains service classes that implement business logic and orchestrate repository calls.
- **Location:** `MusicApp.Business` project
- **Example:**  
  - `AccountService`, `PlaylistService`, etc., which handle user registration, login, and playlist management.

### Web API Layer
- An ASP.NET Core Web API project that exposes endpoints for the frontend.
- **Location:** `MusicApp.Web` project
- Utilizes JWT authentication, CORS, and Swagger for API documentation.

## Technologies Used

- **Backend:** ASP.NET Core (.NET 6/7)  
  - Entity Framework Core with a code‑first approach
  - MySQL (using Pomelo EntityFrameworkCore.MySql)
  - Repository pattern for data access
  - JWT authentication

- **Frontend:** Angular 18  
  - Responsive UI for music listening and social interactions
  - Integration with the backend API

## Installation and Setup

### Prerequisites

- [.NET 6 or later SDK](https://dotnet.microsoft.com/download)
- [Node.js and npm](https://nodejs.org/en/download/)
- [MySQL Server](https://dev.mysql.com/downloads/)

### Steps to Setup the Backend

1. **Clone the Repository:**  
   Navigate to the backend project folder:
   ```bash
   git clone https://github.com/yourusername/MusicApp.git
   cd MusicApp/MusicApp.Web
   ```

2. **Configure the Database Connection:**
    Update the appsettings.json file with your MySQL connection string:
    ```bash
      {
    "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port={{Portno}};Database={{DbName}};Uid={{Username}};Pwd={{password}}    ;SslMode=Preferred;"
    },
     "Jwt": {
    "Key": "Your_Secret_Key_Here"
     },
     "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
       }
      },
      "AllowedHosts": "*"
    }   
    ```
    
3. **Apply Migrations:**
    Open a terminal in the solution directory and run:
    ```bash
    dotnet ef migrations add InitialWithDbMusicApplication --project MusicApp.Data
    dotnet ef database update --project MusicApp.Data
    ````
4. **Run the Application:**
    In the terminal, navigate to the web project and run:
    ```bash
    dotnet run --project MusicApp.Web
    ```


### Steps to Setup the Frontend

**Install The Following Prerequisites**
1. Node v20.11.1
2. Angular CLI: 18.2.14
3. npm 10.2.4



1. **Navigate to the Angular Project Folder:**
   ```bash
   cd MusicApp/Frontend
   ```

2. **Install Dependencies:**
    ```bash
    npm install
    ````
3. **Run the Angular App:**
   ```bash
   ng serve
   ````
   The application will be available at http://localhost:4200.

## Cloning and Contributing

We welcome contributions! Follow these steps to get started:

1. **Fork the Repository:**
    Click the "Fork" button on the repository’s GitHub page to create your own copy.

2. **Clone Your Fork:**
    ````bash
    git clone https://github.com/yourusername/MusicApp.git
    cd MusicApp
    ````
3. **Create a Branch:**
    Create a new branch for your feature or bug fix:
    ```bash
    git checkout -b feature/your-feature-name
    ````
4. **Make Changes and Commit:**
    Make your changes and commit them with clear messages:
    ```bash
    git commit -m "Add feature: description"
    ````
5. **Push Your Branch:**
    ```bash
    git push origin feature/your-feature-name
    ````
6. **Create a Pull Request:**
    Go to the GitHub page of your fork and click "New Pull Request". Provide a clear description of your changes for review.

## License
This project is licensed under the **MIT License.**

Happy coding and enjoy😍 collaborating on MusicApp🎶🎧🤩 – where music brings people together🙌🫡!



    

