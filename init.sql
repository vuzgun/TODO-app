-- TODO Database Initialization Script
-- This script runs when the PostgreSQL container starts

-- Create the todos table if it doesn't exist
CREATE TABLE IF NOT EXISTS todos (
    "Id" SERIAL PRIMARY KEY,
    title VARCHAR(200) NOT NULL,
    description VARCHAR(1000),
    is_completed BOOLEAN NOT NULL DEFAULT FALSE,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    completed_at TIMESTAMP WITH TIME ZONE,
    priority INTEGER NOT NULL DEFAULT 1
);

-- Insert sample data
INSERT INTO todos (title, description, priority, is_completed, created_at) VALUES
    ('Learn .NET Core', 'Study the fundamentals of .NET Core development', 3, false, '2025-08-09 12:00:00+00'),
    ('Build TODO API', 'Create a RESTful API for managing TODO items', 2, false, '2025-08-10 12:00:00+00')
ON CONFLICT DO NOTHING;

-- Create indexes for better performance
CREATE INDEX IF NOT EXISTS idx_todos_priority ON todos(priority);
CREATE INDEX IF NOT EXISTS idx_todos_is_completed ON todos(is_completed);
CREATE INDEX IF NOT EXISTS idx_todos_created_at ON todos(created_at);

-- Grant permissions
GRANT ALL PRIVILEGES ON TABLE todos TO todo_user;
GRANT USAGE, SELECT ON SEQUENCE todos_"Id"_seq TO todo_user;
