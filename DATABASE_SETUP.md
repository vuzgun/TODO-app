# PostgreSQL Database Setup Guide

This guide will walk you through setting up PostgreSQL and connecting it to your .NET Core TODO application.

## Prerequisites

- .NET 9.0 SDK installed
- PostgreSQL installed on your system
- Basic knowledge of command line operations

## Step 1: Install PostgreSQL

### On macOS (using Homebrew):
```bash
# Install PostgreSQL
brew install postgresql@15

# Start PostgreSQL service
brew services start postgresql@15

# Verify installation
psql --version
```


## Step 2: Create Database and User

### Connect to PostgreSQL:
```bash
# On macOS/Linux (if using default postgres user)
sudo -u postgres psql



### Create Database and User:
```sql
-- Create a new database
CREATE DATABASE todo_db;

-- Create a new user (optional, you can use postgres user)
CREATE USER todo_user WITH PASSWORD 'your_secure_password';

-- Grant privileges to the user
GRANT ALL PRIVILEGES ON DATABASE todo_db TO todo_user;

-- Connect to the new database
\c todo_db

-- Grant schema privileges
GRANT ALL ON SCHEMA public TO todo_user;

-- Exit PostgreSQL
\q
```

## Step 3: Update Connection String

Update the `appsettings.json` file with your actual database credentials:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=todo_db;Username=postgres;Password=your_actual_password;Port=5432"
  }
}
```

**Important Security Notes:**
- Never commit passwords to version control
- Use environment variables or user secrets in production
- Consider using a dedicated database user instead of postgres superuser

## Step 4: Install Entity Framework Tools

```bash
# Install EF tools globally
dotnet tool install --global dotnet-ef

# Verify installation
dotnet ef --version
```

## Step 5: Create and Apply Database Migrations

```bash
# Create initial migration
dotnet ef migrations add InitialCreate

# Apply migration to database
dotnet ef database update
```

## Step 6: Verify Database Connection

### Test the connection:
```bash
# Build the project
dotnet build

# Run the application
dotnet run
```

### Check if tables were created:
```bash
# Connect to your database
psql -U postgres -d todo_db

# List tables
\dt

# Check todo table structure
\d todos

# View sample data
SELECT * FROM todos;

# Exit
\q
```

## Step 7: Environment-Specific Configuration

### Development Environment
Create `appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=todo_db;Username=postgres;Password=dev_password;Port=5432"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.EntityFrameworkCore.Database.Command": "Information"
    }
  }
}
```

### Production Environment
Use environment variables:
```bash
export ConnectionStrings__DefaultConnection="Host=prod_host;Database=todo_db;Username=prod_user;Password=prod_password;Port=5432"
```

## Step 8: Troubleshooting Common Issues

### Connection Refused Error
```bash
# Check if PostgreSQL is running
brew services list | grep postgresql  # macOS
sudo systemctl status postgresql      # Linux
```

### Authentication Failed
- Verify username and password in connection string
- Check pg_hba.conf file for authentication settings
- Ensure user has proper permissions

### Database Does Not Exist
```sql
-- Connect as postgres user and create database
CREATE DATABASE todo_db;
```

### Migration Errors
```bash
# Remove existing migrations and start fresh
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Step 9: Database Management Commands

### Useful EF Commands:
```bash
# List migrations
dotnet ef migrations list

# Remove last migration
dotnet ef migrations remove

# Update database to specific migration
dotnet ef database update MigrationName

# Generate SQL script
dotnet ef migrations script

# Drop database (careful!)
dotnet ef database drop
```

### Useful PostgreSQL Commands:
```sql
-- Connect to database
\c database_name

-- List tables
\dt

-- Describe table structure
\d table_name

-- List databases
\l

-- List users
\du

-- Exit
\q
```

## Step 10: Performance and Security

### Performance Tips:
- Add indexes for frequently queried columns
- Use connection pooling
- Monitor query performance with `EXPLAIN ANALYZE`

### Security Best Practices:
- Use dedicated database users with minimal privileges
- Enable SSL connections in production
- Regularly update PostgreSQL
- Use strong passwords
- Restrict network access

## Step 11: Backup and Recovery

### Create Backup:
```bash
pg_dump -U postgres -d todo_db > todo_backup.sql
```

### Restore Backup:
```bash
psql -U postgres -d todo_db < todo_backup.sql
```

## Verification Checklist

- [ ] PostgreSQL is installed and running
- [ ] Database `todo_db` exists
- [ ] Connection string is correctly configured
- [ ] Entity Framework tools are installed
- [ ] Initial migration is created and applied
- [ ] Application builds successfully
- [ ] Application connects to database
- [ ] API endpoints work with database
- [ ] Sample data is visible in database

## Next Steps

After successful database setup:
1. Test all API endpoints
2. Add more complex queries if needed
3. Consider adding database indexes
4. Set up automated backups
5. Plan for production deployment

## Additional Resources

- [PostgreSQL Documentation](https://www.postgresql.org/docs/)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [Npgsql Documentation](https://www.npgsql.org/doc/)
- [ASP.NET Core Configuration](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/)
