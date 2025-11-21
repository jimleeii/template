# Microsoft Entra (Azure AD) Authentication Setup

This project now includes Microsoft Entra ID (formerly Azure Active Directory) authentication support for securing your minimal API endpoints.

## Configuration

### 1. Azure Portal Setup

1. **Register your application** in the [Azure Portal](https://portal.azure.com):
   - Navigate to "Microsoft Entra ID" > "App registrations" > "New registration"
   - Enter a name for your application
   - Select supported account types (e.g., "Accounts in this organizational directory only")
   - Click "Register"

2. **Configure API permissions** (if needed):
   - In your app registration, go to "API permissions"
   - Add any required permissions for your API

3. **Expose an API** (for protected API endpoints):
   - Go to "Expose an API"
   - Click "Add a scope"
   - Set the Application ID URI (or use the default)
   - Create scopes as needed (e.g., "access_as_user")

4. **Get your configuration values**:
   - **Tenant ID**: Found in "Overview" section
   - **Client ID**: Found in "Overview" section (Application/Client ID)
   - **Audience**: Typically the Application ID URI or Client ID

### 2. Update appsettings.json

Update the `appsettings.json` file with your Azure AD configuration:

```json
{
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "TenantId": "your-tenant-id-guid",
    "ClientId": "your-client-id-guid",
    "Audience": "api://your-client-id-guid"
  }
}
```

### 3. Environment-Specific Configuration

For development, you can use `appsettings.Development.json`:

```json
{
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "TenantId": "development-tenant-id",
    "ClientId": "development-client-id",
    "Audience": "api://development-client-id"
  }
}
```

For production, use environment variables or Azure Key Vault:
- `AzureAd__TenantId`
- `AzureAd__ClientId`
- `AzureAd__Audience`

## Protected Endpoints

The following endpoints are now available:

### `/api/secure` - Requires authentication
Any authenticated user can access this endpoint.

**Example request**:
```bash
curl -X GET https://localhost:5001/api/secure \
  -H "Authorization: Bearer {your-access-token}"
```

### `/api/admin` - Requires Admin role
Only users with the "Admin" role can access this endpoint.

**Example request**:
```bash
curl -X GET https://localhost:5001/api/admin \
  -H "Authorization: Bearer {your-access-token}"
```

## Getting Access Tokens

### Using Postman

1. Set up a new request
2. Go to the "Authorization" tab
3. Select "OAuth 2.0"
4. Configure:
   - Grant Type: `Authorization Code` or `Client Credentials`
   - Auth URL: `https://login.microsoftonline.com/{tenant-id}/oauth2/v2.0/authorize`
   - Access Token URL: `https://login.microsoftonline.com/{tenant-id}/oauth2/v2.0/token`
   - Client ID: Your application's client ID
   - Scope: `api://{client-id}/.default` or specific scopes

### Using Azure CLI

```bash
az login
az account get-access-token --resource api://{your-client-id}
```

### Using curl

```bash
curl -X POST https://login.microsoftonline.com/{tenant-id}/oauth2/v2.0/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "client_id={client-id}" \
  -d "scope=api://{client-id}/.default" \
  -d "client_secret={client-secret}" \
  -d "grant_type=client_credentials"
```

## Adding Authorization to Your Own Endpoints

### Require authentication:
```csharp
app.MapGet("/my-endpoint", () => "Protected")
    .RequireAuthorization();
```

### Require specific role:
```csharp
app.MapGet("/my-endpoint", () => "Admin only")
    .RequireAuthorization(policy => policy.RequireRole("Admin"));
```

### Require specific claim:
```csharp
app.MapGet("/my-endpoint", () => "Specific claim")
    .RequireAuthorization(policy => policy.RequireClaim("scope", "access_as_user"));
```

### Custom authorization policy:
```csharp
// In Program.cs, add custom policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CustomPolicy", policy =>
        policy.RequireClaim("department", "Engineering"));
});

// Use in endpoint
app.MapGet("/my-endpoint", () => "Custom policy")
    .RequireAuthorization("CustomPolicy");
```

## Accessing User Information

You can access user claims in your endpoint handlers:

```csharp
app.MapGet("/user-info", (ClaimsPrincipal user) => new
{
    Username = user.Identity?.Name,
    Claims = user.Claims.Select(c => new { c.Type, c.Value })
})
.RequireAuthorization();
```

## Testing Locally

For local development without Azure AD setup, you can:

1. Temporarily comment out authentication in `Program.cs`
2. Use a development authentication handler (not recommended for production)
3. Set up a development Azure AD tenant (free)

## Troubleshooting

### 401 Unauthorized
- Verify your token is valid and not expired
- Check that the `Audience` claim in the token matches your configuration
- Ensure the token was issued by the correct authority (tenant)

### 403 Forbidden
- Check that the user has the required roles or claims
- Verify role assignments in Azure AD

### Token validation errors
- Ensure your `appsettings.json` values are correct
- Check that the token's issuer matches your tenant
- Verify the audience claim

## Security Notes

⚠️ **Note**: The current Microsoft.Identity.Web package has a known moderate severity vulnerability (GHSA-rpq8-q44m-2rpg). Monitor for updates and upgrade when a patched version is available.

## Additional Resources

- [Microsoft Identity Web Documentation](https://learn.microsoft.com/en-us/azure/active-directory/develop/microsoft-identity-web)
- [Secure a .NET API with Microsoft Entra ID](https://learn.microsoft.com/en-us/azure/active-directory/develop/scenario-protected-web-api-overview)
- [Azure AD App Registration](https://learn.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app)
