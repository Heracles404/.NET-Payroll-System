<h3>ASP.NET API</h3>

**Instantiate .NET**
> dotnet new webapi

**PostgreSQL (SQL) Dependencies**
> dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL <br>
> dotnet add package Microsoft.EntityFrameworkCore.Design

**Set User Secrets**
> dotnet user-secrets init
> dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=payroll_db;Username=postgres;Password=yourpassword"
