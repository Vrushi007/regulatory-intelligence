# RIM POC - PostgreSQL Integration

This project demonstrates how to connect a .NET 9 console application to a PostgreSQL database using Entity Framework Core.

## Prerequisites

1. **.NET 9 SDK** installed on your machine
2. **PostgreSQL** installed and running locally
3. **PostgreSQL client tools** (optional, for database management)

## Setting up PostgreSQL

### On macOS (using Homebrew)

```bash
# Install PostgreSQL
brew install postgresql@15

# Start PostgreSQL service
brew services start postgresql@15

# Create a database user (if needed)
createuser -s postgres

# Create the database
createdb -U postgres rim_poc_db
```

### On Windows

1. Download and install PostgreSQL from https://www.postgresql.org/download/windows/
2. During installation, remember the password for the `postgres` user
3. Use pgAdmin or command line to create a database named `rim_poc_db`

### On Linux (Ubuntu/Debian)

```bash
# Install PostgreSQL
sudo apt update
sudo apt install postgresql postgresql-contrib

# Switch to postgres user and create database
sudo -u postgres createdb rim_poc_db
```

## Configuration

The database connection string is configured in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=rim_poc_db;Username=postgres;Password=postgres"
  }
}
```

**Important**: Update the connection string with your actual PostgreSQL credentials:

- `Host`: Your PostgreSQL server host (usually `localhost` for local development)
- `Port`: PostgreSQL port (default is `5432`)
- `Database`: Name of your database (`rim_poc_db`)
- `Username`: Your PostgreSQL username
- `Password`: Your PostgreSQL password

## Running the Application

1. **Restore packages**:

   ```bash
   dotnet restore
   ```

2. **Build the project**:

   ```bash
   dotnet build
   ```

3. **Run the application**:
   ```bash
   dotnet run
   ```

The application will:

- Connect to PostgreSQL
- Create the database tables if they don't exist
- Create sample countries (USA, UK, India)
- Display all countries in the database

## Database Structure

The application includes:

### Entities

- **Country**: Entity with comprehensive country information including:
  - Id, Name, Code (unique identifier)
  - IsoCode2, IsoCode3 (ISO country codes)
  - PhoneCode, Capital, Currency, Region
  - IsActive flag for soft deletes
  - CreatedAt, UpdatedAt timestamps

### Services

- **ICountryService/CountryService**: Service layer for country operations (CRUD operations)
- Additional methods for filtering by region and code lookups

### DbContext

- **ApplicationDbContext**: Entity Framework DbContext for database operations

## Entity Framework Migrations (Optional)

If you want to use migrations instead of `EnsureCreatedAsync()`:

1. **Add initial migration**:

   ```bash
   dotnet ef migrations add InitialCreate
   ```

2. **Update database**:
   ```bash
   dotnet ef database update
   ```

## Packages Used

- `Microsoft.EntityFrameworkCore` - Core Entity Framework functionality
- `Npgsql.EntityFrameworkCore.PostgreSQL` - PostgreSQL provider for EF Core
- `Microsoft.EntityFrameworkCore.Tools` - EF Core tools for migrations
- `Microsoft.Extensions.Configuration` - Configuration management
- `Microsoft.Extensions.Configuration.Json` - JSON configuration support
- `Microsoft.Extensions.Hosting` - Host builder for dependency injection

## Troubleshooting

### Connection Issues

- Ensure PostgreSQL is running: `brew services list | grep postgresql` (macOS)
- Check if the database exists: `psql -U postgres -l`
- Verify connection string credentials

### Permission Issues

- Make sure the PostgreSQL user has the necessary permissions
- Try connecting manually: `psql -U postgres -d rim_poc_db`

### Port Issues

- Check if PostgreSQL is running on the expected port (5432)
- Use `netstat -an | grep 5432` to verify

## Next Steps

- Add more entities and relationships
- Implement proper error handling
- Add logging configuration
- Consider using migrations for production deployments
- Add unit tests for the service layer
