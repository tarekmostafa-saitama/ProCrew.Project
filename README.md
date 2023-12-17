# ProCrew Task Project

Welcome to the ProCrew Task Project! This project follows the Clean Architecture pattern, consisting of the following layers: Domain, Application, Infrastructure, and API.

## Technologies Used

- ASP.NET Core API
- Entity Framework Core (PostgreSQL)
- Firestore
- MediatR
- Fluent Validation
- Serilog

## How to Run

Follow these steps to run the ProCrew Task Project:

1. Set the API project as the startup project.

2. If needed, update the PostgreSQL connection string path found in the `appsettings.json` file.

3. Open the terminal or command prompt and navigate to the project directory.

4. Run the following command to apply migrations and create the database:

    ```bash
    dotnet ef database update
    ```

5. After the database has been created, run the application. There is a seeding method in place to automatically populate the database with initial data.

6. Open your web browser and navigate to `/swagger` to explore the API using Swagger documentation.


