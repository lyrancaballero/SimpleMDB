namespace Smdb.Api.Auth;

using System.Collections;
using System.Net;
using System.Text.Json;
using Shared.Http;
using Smdb.Core.Auth;

public class AuthApiController
{
    private IAuthService authService;

    public AuthApiController(IAuthService authService)
    {
        this.authService = authService;
    }

    public async Task Login(
        HttpListenerRequest req,
        HttpListenerResponse res,
        Hashtable props
    )
    {
        using var reader = new StreamReader(req.InputStream);
        string body = await reader.ReadToEndAsync();

        var data = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

        if (data == null ||
            !data.ContainsKey("email") ||
            !data.ContainsKey("password"))
        {
            res.StatusCode = (int)HttpStatusCode.BadRequest;

            string errorJson = JsonSerializer.Serialize(new { error = "Missing email or password" });

            using var writer = new StreamWriter(res.OutputStream);
            await writer.WriteAsync(errorJson);
            return;
        }

        string email = data["email"].ToString();
        string password = data["password"].ToString();

        var result = await authService.Login(email, password);

        if (!result.Success || result.Value == null)
        {
            res.StatusCode = (int)HttpStatusCode.Unauthorized;

            string errorJson = JsonSerializer.Serialize(new { error = "Invalid credentials" });

            using var writer = new StreamWriter(res.OutputStream);
            await writer.WriteAsync(errorJson);
            return;
        }

        res.StatusCode = (int)HttpStatusCode.OK;

        string json = JsonSerializer.Serialize(result.Value);

        using var writer = new StreamWriter(res.OutputStream);
        await writer.WriteAsync(json);
    }
}
