
A simple project to test minimal APIs, SQLite, Dapper and DbUp

---
Add connection string to both DatabaseMigrations and Notes.Net projects

`dotnet user-secrets set "ConnectionStrings:Notes" "Data Source=Path\To\The\database.db"`

Then simply run DatabaseMigrations project to apply migrations, and create the database if necessary.
