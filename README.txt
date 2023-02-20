If dotnet-ef is not installed then run following command first:
 dotnet tool install --global dotnet-ef --version 6.*
Migration Command
 C:\Users\Muhammad Mutahhar\source\repos\bbd-be\src\host> dotnet ef migrations add <CommitMessage> --project .././Migrators/Migrators.MSSQL/ --context ApplicationDbContext -o Migrations/Application
Revert Migration Command
  dotnet ef migrations remove --project .././Migrators/Migrators.MSSQL/ --context ApplicationDbContext

  dotnet ef database update 20221229125455_ModifiedOnDeleteCascadeOnPlanAndPlanDivision