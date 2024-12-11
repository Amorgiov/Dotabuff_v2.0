
# DotaBuffClone

## Description
DotaBuffClone is an ASP.NET Core application mimicking DotaBuff's functionalities, using .NET 8.

## Prerequisites
- .NET 8 SDK installed.
- PostgreSQL installed and running.

## Setup Instructions
1. Update `appsettings.json` with your PostgreSQL credentials:
    ```json
    {
        "ConnectionStrings": {
            "DefaultConnection": "Host=localhost;Database=dotabuff_clone;Username=postgres;Password=root"
        }
    }
    ```

2. Restore NuGet packages:
    ```bash
    dotnet restore
    ```

3. Create and apply migrations:
    ```bash
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```

4. Run the application:
    ```bash
    dotnet run
    ```

## Default Admin Credentials
- **Username**: admin
- **Password**: Admin123!
