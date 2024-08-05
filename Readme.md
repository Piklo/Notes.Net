Add connection string to both DatabaseMigrations and Notes.Net projects

`dotnet user-secrets set "ConnectionStrings:Notes" "Data Source=Path\To\The\database.db"`

Then simply run DatabaseMigrations project to apply migrations.
