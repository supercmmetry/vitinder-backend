# VITinder Backend
ASP.Net Core backend for VITinder written with Mediator and CQRS

Uses cloudinary for image hosting

## Configuration
- Requires PostgreSQL database.

- Manage app settings (appsettings.json)

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=5432;Database=vitinder;User Id=postgres;Password=postgres;"
  },
  "Cloudinary": {
    "CloudName": "cloudinary-cloud-name",
    "ApiKey": "cloudinary-api-key",
    "ApiSecret": "cloudinary-api-secret",
    "Folders": {
      "ProfileImages": "cloudinary-folder-for-profile-pics"
    }
  },
  "JwtBearerOptions": {
    "Authority": "https://securetoken.google.com/<firebase-project-id>",
    "ValidIssuer": "https://securetoken.google.com/<firebase-project-id>",
    "ValidAudience": "firebase-project-id"
  }
}

```

- Create migrations

  `dotnet ef migrations add InitialCreate --project Persistence --startup-project Api`

- Apply migrations

  `dotnet ef database update --project Persistence --startup-project Api`
