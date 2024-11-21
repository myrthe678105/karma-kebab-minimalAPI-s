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

    public AuthResponse Authenticate(string username, string password)
    {
        var user = _loginData.FirstOrDefault(u => u.Username == username && u.Password == password);
        if (user == null)
        {
            return null;
        }

        return new AuthResponse
        {
            AccessToken = "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJWNlBsZFRyZWtzb0Ezb3BUTmFUUE96VTM3WjgwMzNnTHlWb09OOVV2YmlBIn0.eyJleHAiOjE3MzIxOTQxNTcsImlhdCI6MTczMjE5Mzg1NywianRpIjoiZDhlYjY5M2ItMWE4Yy00ODVkLTliNjUtODBkOTQzYzFmN2E1IiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo4MDgwL3JlYWxtcy9rYXJtYS1rZWJhYi1yZWFsbSIsInN1YiI6IjIxNGE2NDY1LTczNWItNDVhNi1hMWUxLTAyZjZiZGFkMzRjMiIsInR5cCI6IkJlYXJlciIsImF6cCI6Imthcm1hLWtlYmFiLWNsaWVudCIsInNpZCI6IjgwNzQ3ODVkLWUxNzItNGFiMS04YWQyLTg5MDZjZWVlZjk3YyIsImFjciI6IjEiLCJhbGxvd2VkLW9yaWdpbnMiOlsiaHR0cDovL2xvY2FsaG9zdDozMDA1IiwiaHR0cDovL2xvY2FsaG9zdDozMDA0IiwiaHR0cDovL2xvY2FsaG9zdDozMDAzIiwiaHR0cDovL2xvY2FsaG9zdDozMDAyIiwiaHR0cDovL2xvY2FsaG9zdDozMDA2IiwiaHR0cDovL2xvY2FsaG9zdDozMDAxIl0sInNjb3BlIjoicHJvZmlsZSBlbWFpbCIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJuYW1lIjoiVGVzdCBVc2VyIiwicHJlZmVycmVkX3VzZXJuYW1lIjoidGVzdHVzZXIiLCJnaXZlbl9uYW1lIjoiVGVzdCIsImZhbWlseV9uYW1lIjoiVXNlciIsImVtYWlsIjoidGVzdHVzZXJAZXhhbXBsZS5jb20ifQ.XjBOPvS5DZyKvafAL5q2FIEoIlPHUd02U_FFXLcC_cv7k-a9htxp0Cod5guQyNaxShgHw99SGEwE2EZcjd9mpgCNxeVgKhmfCw4plLtxWkj-JWu2iXauvHjDKYUp8hWkQeIfWRBBmOWp2yUI7IMgrHQsLKUoAvv8-rl0phicbGAUB5R_kjaU4V56AtuJGyog59USDLFZNq9DlLjOkPtWEYFDo7isJCtOjqLfzc9cn2m4b3RYD7x_Z0eHiwA2IJAL-6ks4_FNJ2qXiOKcPK9rIvvH_Y89Q3PMFOWHRuyAU2N6NcTE2DLtfPuHlg7w0QLiuA0589p0WEcplux0BK3pQA",
            ExpiresIn = 300,
            RefreshExpiresIn = 1800,
            RefreshToken = "eyJhbGciOiJIUzUxMiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJlNzA3MTgxNi04NjAwLTQ1MzEtYWFhNC1jMGZlNmQ3NjBkNWQifQ.eyJleHAiOjE3MzIxOTU2NTcsImlhdCI6MTczMjE5Mzg1NywianRpIjoiMDIwOGE5MGYtMjY5ZC00OTBlLTg0MDAtNGJhYzVkNzkwMzU0IiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo4MDgwL3JlYWxtcy9rYXJtYS1rZWJhYi1yZWFsbSIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6ODA4MC9yZWFsbXMva2FybWEta2ViYWItcmVhbG0iLCJzdWIiOiIyMTRhNjQ2NS03MzViLTQ1YTYtYTFlMS0wMmY2YmRhZDM0YzIiLCJ0eXAiOiJSZWZyZXNoIiwiYXpwIjoia2FybWEta2ViYWItY2xpZW50Iiwic2lkIjoiODA3NDc4NWQtZTE3Mi00YWIxLThhZDItODkwNmNlZWVmOTdjIiwic2NvcGUiOiJiYXNpYyBwcm9maWxlIGFjciB3ZWItb3JpZ2lucyByb2xlcyBlbWFpbCJ9.seLDHm_j-U5CNSq3SmssUWYBSepi9Qa-7Ig9qJSfiRBrGGVH5QC3SApmIaQnkASHs5d3nA42IJ3ZpNrp1emlNQ",
            TokenType = "Bearer",
            NotBeforePolicy = 0,
            SessionState = "8074785d-e172-4ab1-8ad2-8906ceeef97c",
            Scope = "profile email"
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