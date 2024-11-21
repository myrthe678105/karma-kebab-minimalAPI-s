using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class AuthService
{
    private readonly List<LoginData> _loginData;

    public AuthService(string filePath)
    {
        var jsonData = File.ReadAllText(filePath);
        _loginData = JsonConvert.DeserializeObject<List<LoginData>>(jsonData);
    }

    // Login
    public AuthResponse Authenticate(string username, string password)
    {
        var user = _loginData.FirstOrDefault(u => u.Username == username && u.Password == password);
        if (user == null)
        {
            return null;
        }

        return new AuthResponse
        {
            AccessToken = "mock-access-token",
            ExpiresIn = 300,
            RefreshExpiresIn = 1800,
            RefreshToken = "mock-refresh-token",
            TokenType = "Bearer",
            NotBeforePolicy = 0,
            SessionState = "mock-session-state",
            Scope = "profile email"
        };
    }

    // Refresh Token
    public AuthResponse RefreshToken(string refreshToken)
    {
        if (refreshToken != "mock-refresh-token")
        {
            return null;
        }

        return new AuthResponse
        {
            AccessToken = "mock-access-token",
            ExpiresIn = 300,
            RefreshExpiresIn = 1800,
            RefreshToken = "mock-refresh-token",
            TokenType = "Bearer",
            NotBeforePolicy = 0,
            SessionState = "mock-session-state",
            Scope = "profile email"
        };
    }

    // Logout
    public IResult Logout(string refreshToken)
    {
        if (refreshToken != "mock-refresh-token")
        {
            return Results.BadRequest(new { error = "invalid_request", error_description = "Refresh token is missing" });
        }
        return Results.NoContent();
    }

    // User Info
    public UserInfoResponse GetUserInfo(string accessToken)
    {
        if (accessToken != "mock-access-token")
        {
            return null;
        }

        return new UserInfoResponse
        {
            Sub = "mock-sub-id",
            Name = "Test User",
            Email = "testuser@example.com",
            PreferredUsername = "testuser",
            GivenName = "Test",
            FamilyName = "User",
            EmailVerified = true
        };
    }
}

public class LoginData
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class AuthResponse
{
    public string AccessToken { get; set; }
    public int ExpiresIn { get; set; }
    public int RefreshExpiresIn { get; set; }
    public string RefreshToken { get; set; }
    public string TokenType { get; set; }
    public int NotBeforePolicy { get; set; }
    public string SessionState { get; set; }
    public string Scope { get; set; }
}

public class UserInfoResponse
{
    public string Sub { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PreferredUsername { get; set; }
    public string GivenName { get; set; }
    public string FamilyName { get; set; }
    public bool EmailVerified { get; set; }
}
