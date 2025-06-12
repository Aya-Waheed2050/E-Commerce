## Configuration

Please create a file named appsettings.Development.json in the root directory with the following structure:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your connection string",
    "IdentityConnection": "your connection string",
    "RedisConnectionString": "your redis string"
  },
  "Urls": {
    "BaseUrl": "https://localhost:xxxx"
  },
  "JWTOptions": {
    "SecretKey": "your secret key",
    "Issuer": "your issuer",
    "Audience": "your audience"
  },
  "StripeSettings": {
    "SecretKey": "your stripe secret",
    "EndPointSecret": "your stripe endpoint"
  }
}