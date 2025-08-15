#!/bin/bash

echo "🚀 TODO App Database Setup Script"
echo "=================================="
echo ""

# Add PostgreSQL to PATH for Apple Silicon Macs
export PATH="/opt/homebrew/opt/postgresql@15/bin:$PATH"

# Check if PostgreSQL is installed
if ! command -v psql &> /dev/null; then
    echo "❌ PostgreSQL is not installed!"
    echo "Please install PostgreSQL first:"
    echo "  macOS: brew install postgresql@15"
    echo "  Ubuntu: sudo apt install postgresql postgresql-contrib"
    echo "  Windows: Download from https://www.postgresql.org/download/windows/"
    exit 1
fi

echo "✅ PostgreSQL is installed"
echo ""

# Check if PostgreSQL service is running
if ! pg_isready -q; then
    echo "⚠️  PostgreSQL service is not running"
    echo "Starting PostgreSQL service..."
    
    if [[ "$OSTYPE" == "darwin"* ]]; then
        brew services start postgresql@15
    elif [[ "$OSTYPE" == "linux-gnu"* ]]; then
        sudo systemctl start postgresql
    fi
    
    sleep 3
fi

echo "✅ PostgreSQL service is running"
echo ""

# Get database credentials
echo "📝 Database Setup"
echo "================="
read -p "Enter database name (default: todo_db): " DB_NAME
DB_NAME=${DB_NAME:-todo_db}

read -p "Enter PostgreSQL username (default: postgres): " DB_USER
DB_USER=${DB_USER:-postgres}

read -s -p "Enter PostgreSQL password: " DB_PASSWORD
echo ""

read -p "Enter PostgreSQL port (default: 5432): " DB_PORT
DB_PORT=${DB_PORT:-5432}

echo ""
echo "🔧 Creating database and user..."

# Create database and user
PGPASSWORD=$DB_PASSWORD psql -U $DB_USER -h localhost -p $DB_PORT -c "CREATE DATABASE $DB_NAME;" 2>/dev/null || echo "Database might already exist"

echo "✅ Database '$DB_NAME' created/verified"
echo ""

# Update connection string in appsettings.json
echo "🔧 Updating connection string..."
CONNECTION_STRING="Host=localhost;Database=$DB_NAME;Username=$DB_USER;Password=$DB_PASSWORD;Port=$DB_PORT"

# Create a backup of the original file
cp appsettings.json appsettings.json.backup

# Update the connection string
if [[ "$OSTYPE" == "darwin"* ]]; then
    # macOS
    sed -i '' "s|Host=localhost;Database=todo_db;Username=postgres;Password=your_password;Port=5432|$CONNECTION_STRING|g" appsettings.json
else
    # Linux
    sed -i "s|Host=localhost;Database=todo_db;Username=postgres;Password=your_password;Port=5432|$CONNECTION_STRING|g" appsettings.json
fi

echo "✅ Connection string updated in appsettings.json"
echo ""

# Check if Entity Framework tools are installed
if ! command -v dotnet-ef &> /dev/null; then
    echo "📦 Installing Entity Framework tools..."
    dotnet tool install --global dotnet-ef
    echo "✅ Entity Framework tools installed"
else
    echo "✅ Entity Framework tools are already installed"
fi

echo ""

# Create and apply migrations
echo "🗄️  Setting up database schema..."
dotnet ef migrations add InitialCreate
dotnet ef database update

echo ""
echo "🎉 Database setup complete!"
echo ""
echo "📋 Next steps:"
echo "1. Run 'dotnet run' to start the application"
echo "2. Test the API endpoints"
echo "3. Check the database: psql -U $DB_USER -d $DB_NAME -c '\\dt'"
echo ""
echo "🔒 Security note: Consider updating the connection string with environment variables for production use."
echo ""
echo "📚 For more information, see DATABASE_SETUP.md"
echo ""
echo "💡 To make PostgreSQL commands available permanently, add this to your ~/.zshrc:"
echo "export PATH=\"/opt/homebrew/opt/postgresql@15/bin:\$PATH\""
