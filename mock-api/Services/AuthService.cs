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
            AccessToken = "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJWNlBsZFRyZWtzb0Ezb3BUTmFUUE96VTM3WjgwMzNnTHlWb09OOVV2YmlBIn0.eyJleHAiOjE3MzIyMDUwODgsImlhdCI6MTczMjIwNDc4OCwianRpIjoiYTg4NTRhOTYtMGYxOC00MzhlLWIzZDctYjQ1MjIxMDVjZjlmIiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo4MDgwL3JlYWxtcy9rYXJtYS1rZWJhYi1yZWFsbSIsInN1YiI6IjIxNGE2NDY1LTczNWItNDVhNi1hMWUxLTAyZjZiZGFkMzRjMiIsInR5cCI6IkJlYXJlciIsImF6cCI6Imthcm1hLWtlYmFiLWNsaWVudCIsInNpZCI6IjE5M2U2MGIyLTcwYjAtNDY1Zi1iOWNiLWVjZGNkYWZiYTAyMCIsImFjciI6IjEiLCJhbGxvd2VkLW9yaWdpbnMiOlsiaHR0cDovL2xvY2FsaG9zdDozMDA1IiwiaHR0cDovL2xvY2FsaG9zdDozMDA0IiwiaHR0cDovL2xvY2FsaG9zdDozMDAzIiwiaHR0cDovL2xvY2FsaG9zdDozMDAyIiwiaHR0cDovL2xvY2FsaG9zdDozMDA2IiwiaHR0cDovL2xvY2FsaG9zdDozMDAxIl0sInNjb3BlIjoib3BlbmlkIHByb2ZpbGUgZW1haWwiLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwibmFtZSI6IlRlc3QgVXNlciIsInByZWZlcnJlZF91c2VybmFtZSI6InRlc3R1c2VyIiwiZ2l2ZW5fbmFtZSI6IlRlc3QiLCJmYW1pbHlfbmFtZSI6IlVzZXIiLCJlbWFpbCI6InRlc3R1c2VyQGV4YW1wbGUuY29tIn0.pljUBoAP4rJBoHXgLSW3dCBDzMQi071Jl0zljtLih4ATZcJfRVUHtCCAjUqD-jppcgwN1wTGDvUDDfhI4qndZfHeXbniiZMqcH0nfbcoJzyygLOf_j5o-y5-F2dvzUcXuubgcF4mAyujYgEqWFcKR4gvkT2gC7O6RrCu2RnuqkjZaFm2QKVR4KFJMXQci51CBobxaALOVWXZRZ6PJGO4H_efYQupmVR7AZdOjnaaCtxjz56DQGf8OWwzfejtopGsj_J6Hg0e5-P4MYB9B4ExICl09wy2xo-7Me18mbnMT0JWmLmIGyP9VOfKR5AstQb1vcX8wXnV4cFtekKKxDBBHw",
            ExpiresIn = 300,
            RefreshExpiresIn = 1800,
            RefreshToken = "eyJhbGciOiJIUzUxMiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJlNzA3MTgxNi04NjAwLTQ1MzEtYWFhNC1jMGZlNmQ3NjBkNWQifQ.eyJleHAiOjE3MzIyMDY1ODgsImlhdCI6MTczMjIwNDc4OCwianRpIjoiODE3OTc1MjctOGIyYy00Mzc0LTg4N2MtOGM5ZTQxNjQwNGEyIiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo4MDgwL3JlYWxtcy9rYXJtYS1rZWJhYi1yZWFsbSIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6ODA4MC9yZWFsbXMva2FybWEta2ViYWItcmVhbG0iLCJzdWIiOiIyMTRhNjQ2NS03MzViLTQ1YTYtYTFlMS0wMmY2YmRhZDM0YzIiLCJ0eXAiOiJSZWZyZXNoIiwiYXpwIjoia2FybWEta2ViYWItY2xpZW50Iiwic2lkIjoiMTkzZTYwYjItNzBiMC00NjVmLWI5Y2ItZWNkY2RhZmJhMDIwIiwic2NvcGUiOiJvcGVuaWQgYmFzaWMgcHJvZmlsZSBhY3Igd2ViLW9yaWdpbnMgcm9sZXMgZW1haWwifQ.OKshBBUhJMHI_wDD3A_AayM6q0NGZuDk49EQw75IvNHJLs_NM1_Znx3shnjI3yHCW-7Gp8QyDzYQpVygO2gVpg",
            TokenType = "Bearer",
            NotBeforePolicy = 0,
            SessionState = "193e60b2-70b0-465f-b9cb-ecdcdafba020",
            Scope = "openid profile email"
        };
    }

    // Refresh Token
    public AuthResponse RefreshToken(string refreshToken)
    {
        if (refreshToken != "eyJhbGciOiJIUzUxMiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJlNzA3MTgxNi04NjAwLTQ1MzEtYWFhNC1jMGZlNmQ3NjBkNWQifQ.eyJleHAiOjE3MzIyMDY1ODgsImlhdCI6MTczMjIwNDc4OCwianRpIjoiODE3OTc1MjctOGIyYy00Mzc0LTg4N2MtOGM5ZTQxNjQwNGEyIiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo4MDgwL3JlYWxtcy9rYXJtYS1rZWJhYi1yZWFsbSIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6ODA4MC9yZWFsbXMva2FybWEta2ViYWItcmVhbG0iLCJzdWIiOiIyMTRhNjQ2NS03MzViLTQ1YTYtYTFlMS0wMmY2YmRhZDM0YzIiLCJ0eXAiOiJSZWZyZXNoIiwiYXpwIjoia2FybWEta2ViYWItY2xpZW50Iiwic2lkIjoiMTkzZTYwYjItNzBiMC00NjVmLWI5Y2ItZWNkY2RhZmJhMDIwIiwic2NvcGUiOiJvcGVuaWQgYmFzaWMgcHJvZmlsZSBhY3Igd2ViLW9yaWdpbnMgcm9sZXMgZW1haWwifQ.OKshBBUhJMHI_wDD3A_AayM6q0NGZuDk49EQw75IvNHJLs_NM1_Znx3shnjI3yHCW-7Gp8QyDzYQpVygO2gVpg")
        {
            return null;
        }

        return new AuthResponse
        {
            AccessToken = "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJWNlBsZFRyZWtzb0Ezb3BUTmFUUE96VTM3WjgwMzNnTHlWb09OOVV2YmlBIn0.eyJleHAiOjE3MzIyMDUwODgsImlhdCI6MTczMjIwNDc4OCwianRpIjoiYTg4NTRhOTYtMGYxOC00MzhlLWIzZDctYjQ1MjIxMDVjZjlmIiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo4MDgwL3JlYWxtcy9rYXJtYS1rZWJhYi1yZWFsbSIsInN1YiI6IjIxNGE2NDY1LTczNWItNDVhNi1hMWUxLTAyZjZiZGFkMzRjMiIsInR5cCI6IkJlYXJlciIsImF6cCI6Imthcm1hLWtlYmFiLWNsaWVudCIsInNpZCI6IjE5M2U2MGIyLTcwYjAtNDY1Zi1iOWNiLWVjZGNkYWZiYTAyMCIsImFjciI6IjEiLCJhbGxvd2VkLW9yaWdpbnMiOlsiaHR0cDovL2xvY2FsaG9zdDozMDA1IiwiaHR0cDovL2xvY2FsaG9zdDozMDA0IiwiaHR0cDovL2xvY2FsaG9zdDozMDAzIiwiaHR0cDovL2xvY2FsaG9zdDozMDAyIiwiaHR0cDovL2xvY2FsaG9zdDozMDA2IiwiaHR0cDovL2xvY2FsaG9zdDozMDAxIl0sInNjb3BlIjoib3BlbmlkIHByb2ZpbGUgZW1haWwiLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwibmFtZSI6IlRlc3QgVXNlciIsInByZWZlcnJlZF91c2VybmFtZSI6InRlc3R1c2VyIiwiZ2l2ZW5fbmFtZSI6IlRlc3QiLCJmYW1pbHlfbmFtZSI6IlVzZXIiLCJlbWFpbCI6InRlc3R1c2VyQGV4YW1wbGUuY29tIn0.pljUBoAP4rJBoHXgLSW3dCBDzMQi071Jl0zljtLih4ATZcJfRVUHtCCAjUqD-jppcgwN1wTGDvUDDfhI4qndZfHeXbniiZMqcH0nfbcoJzyygLOf_j5o-y5-F2dvzUcXuubgcF4mAyujYgEqWFcKR4gvkT2gC7O6RrCu2RnuqkjZaFm2QKVR4KFJMXQci51CBobxaALOVWXZRZ6PJGO4H_efYQupmVR7AZdOjnaaCtxjz56DQGf8OWwzfejtopGsj_J6Hg0e5-P4MYB9B4ExICl09wy2xo-7Me18mbnMT0JWmLmIGyP9VOfKR5AstQb1vcX8wXnV4cFtekKKxDBBHw",
            ExpiresIn = 300,
            RefreshExpiresIn = 1800,
            RefreshToken = "eyJhbGciOiJIUzUxMiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJlNzA3MTgxNi04NjAwLTQ1MzEtYWFhNC1jMGZlNmQ3NjBkNWQifQ.eyJleHAiOjE3MzIyMDY1ODgsImlhdCI6MTczMjIwNDc4OCwianRpIjoiODE3OTc1MjctOGIyYy00Mzc0LTg4N2MtOGM5ZTQxNjQwNGEyIiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo4MDgwL3JlYWxtcy9rYXJtYS1rZWJhYi1yZWFsbSIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6ODA4MC9yZWFsbXMva2FybWEta2ViYWItcmVhbG0iLCJzdWIiOiIyMTRhNjQ2NS03MzViLTQ1YTYtYTFlMS0wMmY2YmRhZDM0YzIiLCJ0eXAiOiJSZWZyZXNoIiwiYXpwIjoia2FybWEta2ViYWItY2xpZW50Iiwic2lkIjoiMTkzZTYwYjItNzBiMC00NjVmLWI5Y2ItZWNkY2RhZmJhMDIwIiwic2NvcGUiOiJvcGVuaWQgYmFzaWMgcHJvZmlsZSBhY3Igd2ViLW9yaWdpbnMgcm9sZXMgZW1haWwifQ.OKshBBUhJMHI_wDD3A_AayM6q0NGZuDk49EQw75IvNHJLs_NM1_Znx3shnjI3yHCW-7Gp8QyDzYQpVygO2gVpg",
            TokenType = "Bearer",
            NotBeforePolicy = 0,
            SessionState = "193e60b2-70b0-465f-b9cb-ecdcdafba020",
            Scope = "openid profile email"
        };
    }

    // Logout
    public IResult Logout(string refreshToken)
    {
        if (refreshToken != "eyJhbGciOiJIUzUxMiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJlNzA3MTgxNi04NjAwLTQ1MzEtYWFhNC1jMGZlNmQ3NjBkNWQifQ.eyJleHAiOjE3MzIyMDY1ODgsImlhdCI6MTczMjIwNDc4OCwianRpIjoiODE3OTc1MjctOGIyYy00Mzc0LTg4N2MtOGM5ZTQxNjQwNGEyIiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo4MDgwL3JlYWxtcy9rYXJtYS1rZWJhYi1yZWFsbSIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6ODA4MC9yZWFsbXMva2FybWEta2ViYWItcmVhbG0iLCJzdWIiOiIyMTRhNjQ2NS03MzViLTQ1YTYtYTFlMS0wMmY2YmRhZDM0YzIiLCJ0eXAiOiJSZWZyZXNoIiwiYXpwIjoia2FybWEta2ViYWItY2xpZW50Iiwic2lkIjoiMTkzZTYwYjItNzBiMC00NjVmLWI5Y2ItZWNkY2RhZmJhMDIwIiwic2NvcGUiOiJvcGVuaWQgYmFzaWMgcHJvZmlsZSBhY3Igd2ViLW9yaWdpbnMgcm9sZXMgZW1haWwifQ.OKshBBUhJMHI_wDD3A_AayM6q0NGZuDk49EQw75IvNHJLs_NM1_Znx3shnjI3yHCW-7Gp8QyDzYQpVygO2gVpg")
        {
            return Results.BadRequest(new { error = "invalid_request", error_description = "Refresh token is missing" });
        }
        return Results.NoContent();
    }

    // User Info
    public UserInfoResponse GetUserInfo(string accessToken)
    {
        if (accessToken != "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJWNlBsZFRyZWtzb0Ezb3BUTmFUUE96VTM3WjgwMzNnTHlWb09OOVV2YmlBIn0.eyJleHAiOjE3MzIyMDUwODgsImlhdCI6MTczMjIwNDc4OCwianRpIjoiYTg4NTRhOTYtMGYxOC00MzhlLWIzZDctYjQ1MjIxMDVjZjlmIiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo4MDgwL3JlYWxtcy9rYXJtYS1rZWJhYi1yZWFsbSIsInN1YiI6IjIxNGE2NDY1LTczNWItNDVhNi1hMWUxLTAyZjZiZGFkMzRjMiIsInR5cCI6IkJlYXJlciIsImF6cCI6Imthcm1hLWtlYmFiLWNsaWVudCIsInNpZCI6IjE5M2U2MGIyLTcwYjAtNDY1Zi1iOWNiLWVjZGNkYWZiYTAyMCIsImFjciI6IjEiLCJhbGxvd2VkLW9yaWdpbnMiOlsiaHR0cDovL2xvY2FsaG9zdDozMDA1IiwiaHR0cDovL2xvY2FsaG9zdDozMDA0IiwiaHR0cDovL2xvY2FsaG9zdDozMDAzIiwiaHR0cDovL2xvY2FsaG9zdDozMDAyIiwiaHR0cDovL2xvY2FsaG9zdDozMDA2IiwiaHR0cDovL2xvY2FsaG9zdDozMDAxIl0sInNjb3BlIjoib3BlbmlkIHByb2ZpbGUgZW1haWwiLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwibmFtZSI6IlRlc3QgVXNlciIsInByZWZlcnJlZF91c2VybmFtZSI6InRlc3R1c2VyIiwiZ2l2ZW5fbmFtZSI6IlRlc3QiLCJmYW1pbHlfbmFtZSI6IlVzZXIiLCJlbWFpbCI6InRlc3R1c2VyQGV4YW1wbGUuY29tIn0.pljUBoAP4rJBoHXgLSW3dCBDzMQi071Jl0zljtLih4ATZcJfRVUHtCCAjUqD-jppcgwN1wTGDvUDDfhI4qndZfHeXbniiZMqcH0nfbcoJzyygLOf_j5o-y5-F2dvzUcXuubgcF4mAyujYgEqWFcKR4gvkT2gC7O6RrCu2RnuqkjZaFm2QKVR4KFJMXQci51CBobxaALOVWXZRZ6PJGO4H_efYQupmVR7AZdOjnaaCtxjz56DQGf8OWwzfejtopGsj_J6Hg0e5-P4MYB9B4ExICl09wy2xo-7Me18mbnMT0JWmLmIGyP9VOfKR5AstQb1vcX8wXnV4cFtekKKxDBBHw")
        {
            return null;
        }

        return new UserInfoResponse
        {
            Sub = "214a6465-735b-45a6-a1e1-02f6bdad34c2",
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
