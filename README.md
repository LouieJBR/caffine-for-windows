# CaffineApp

## Description
CaffineApp is a lightweight Windows desktop application designed to prevent your screen from timing out. It sits in the system tray and allows you to easily control your display settings. Users can choose to keep the screen active indefinitely, for 30 minutes, or for 1 hour.

## Features
- Prevents screen timeout to avoid interruptions
- Options to keep the screen active:
  - Indefinitely
  - 30 minutes
  - 1 hour
- System tray icon for easy access and management
- Simple and intuitive user interface

## Local Installation

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download) (version 8.0 or later)
- Windows operating system

### Steps to Install
1. **Clone the Repository**:
   ```bash
   git clone https://github.com/yourusername/caffine.git
   cd caffine/caffine-for-windows/CaffineApp/CaffineApp
   ```

2. **Restore Dependencies**:
   Run the following command to restore the necessary NuGet packages:
   ```bash
   dotnet restore
   ```

3. **Build the Project**:
   Build the project using the command:
   ```bash
   dotnet build
   ```

## Running the Application
After building the project, you can run the application with:
```bash
dotnet run
```