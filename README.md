<h3>ASP.NET API</h3>
>**Running the Program**</br>
``>> dotnet build`` </br>
``>> dotnet run`` 

>**Install dotnet tools globally (If cannot Run Migrations due to PATH Error)** </br>
``>> dotnet tool install --global dotnet-ef``</br>

>**Run Migrations** </br>
``>> dotnet ef migrations add InitialCreate``</br>
``>> dotnet ef database update``</br>

>**Check Installed Packages** </br>
``>> dotnet list package``</br>

>**Instantiate .NET** </br>
``>> dotnet new webapi``</br>
``>> dotnet add package Microsoft.EntityFrameworkCore.Design``

>**PostgreSQL (SQL) Dependencies / Drivers** </br>
``>> dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL`` <br>

>**Set User Secrets** </br>
``>> dotnet user-secrets init`` <br>
``>> dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=payroll_db;Username=postgres;Password=yourpassword"`` 

