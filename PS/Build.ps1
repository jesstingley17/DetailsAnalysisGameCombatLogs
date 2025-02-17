param (
    [string]$PROJECT_PATH,
    [string]$TEST_PROJECT_PATH
)

# Restore dependencies
dotnet restore $PROJECT_PATH

# Build the application
dotnet build $PROJECT_PATH

# Tests the application
dotnet test $TEST_PROJECT_PATH

Write-Host "Build and tests completed successfully."