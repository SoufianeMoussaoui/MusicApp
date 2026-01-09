# Music App - Setup Guide

This repository contains a Music Streaming Application built with ASP.NET Core MVC. Follow the instructions below to set up and run the project locally.

---

## 🧰 Prerequisites

Make sure you have the following installed:

- .NET 9.0 SDK or higher
- Visual Studio 2022 or Visual Studio Code
- PostgreSQL 9.0 or higher
- Git

---

## 🚀 Installation

### 1. Clone the repository

```bash
git clone https://github.com/Dom-Akz/MusicApp.git
cd MusicApp
```

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Update database connection string

Update the connection string in `appsettings.json` to match your database configuration.

### 4. Apply database migrations

```bash
dotnet ef database update
```

### 5. Run the application

```bash
dotnet run
```

Open your browser at https://localhost:5001 or http://localhost:5000 to view the project.

---

## 🏗️ Build for Production

Build the app for production:

```bash
dotnet build --configuration Release
```

Publish the application:

```bash
dotnet publish --configuration Release --output ./publish
```

---

## 🧪 Running Tests

Run tests using:

```bash
dotnet test
```

---

## ⚙️ Configuration

Update configuration settings in `appsettings.json` and `appsettings.Development.json` as needed. Configure your database connection string and other application settings here.

---

## 📁 Project Structure

```
MusicApp/
├── Controllers/
│   └── ...
├── Models/
│   └── ...
├── Views/
│   └── ...
├── Data/
│   └── ...
├── Migrations/
│   └── ...
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── ...
├── Program.cs
├── appsettings.json
├── musicApp.csproj
└── README.md
```

---
