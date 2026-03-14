namespace Smdb.Api.Users;

using System.Collections;
using System.Collections.Specialized;
using System.Net;
using System.Text.Json;
using Shared.Http;
using Smdb.Core.Users;

public class UsersApiController
{
    private IUserService userService;

    public UsersApiController(IUserService userService)
    {
        this.userService = userService;
    }

    // API methods will go here
    public async Task ReadUsers(
    HttpListenerRequest req,
    HttpListenerResponse res,
    Hashtable props
)
{
    int page = int.TryParse(req.QueryString["page"], out var p) ? p : 1;
    int size = int.TryParse(req.QueryString["size"], out var s) ? s : 10;

    var result = await userService.ReadUsers(page, size);

    res.StatusCode = (int)HttpStatusCode.OK;

    string json = JsonSerializer.Serialize(result.Value);

    using var writer = new StreamWriter(res.OutputStream);
    await writer.WriteAsync(json);
}
public async Task ReadUser(
    HttpListenerRequest req,
    HttpListenerResponse res,
    Hashtable props
)
{
    if (!int.TryParse(req.QueryString["uid"], out var uid))
    {
        res.StatusCode = (int)HttpStatusCode.BadRequest;

        string errorJson = JsonSerializer.Serialize(new { error = "Invalid or missing uid" });

        using var writer = new StreamWriter(res.OutputStream);
        await writer.WriteAsync(errorJson);
        return;
    }

    var result = await userService.ReadUser(uid);

    if (!result.Success || result.Value == null)
    {
        res.StatusCode = (int)HttpStatusCode.NotFound;

        string errorJson = JsonSerializer.Serialize(new { error = "User not found" });

        using var writer = new StreamWriter(res.OutputStream);
        await writer.WriteAsync(errorJson);
        return;
    }

    res.StatusCode = (int)HttpStatusCode.OK;

    string json = JsonSerializer.Serialize(result.Value);

    using var writer = new StreamWriter(res.OutputStream);
    await writer.WriteAsync(json);
}
public async Task CreateUser(
    HttpListenerRequest req,
    HttpListenerResponse res,
    Hashtable props
)
{
    using var reader = new StreamReader(req.InputStream);
    string body = await reader.ReadToEndAsync();

    var data = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

    if (data == null ||
        !data.ContainsKey("username") ||
        !data.ContainsKey("email") ||
        !data.ContainsKey("password"))
    {
        res.StatusCode = (int)HttpStatusCode.BadRequest;

        string errorJson = JsonSerializer.Serialize(new { error = "Missing required fields" });

        using var writer = new StreamWriter(res.OutputStream);
        await writer.WriteAsync(errorJson);
        return;
    }

    string username = data["username"].ToString();
    string email = data["email"].ToString();
    string password = data["password"].ToString();

    var user = new User(0, username, email, password);

    var result = await userService.CreateUser(user);

    if (!result.Success)
    {
        res.StatusCode = (int)HttpStatusCode.BadRequest;

        string errorJson = JsonSerializer.Serialize(new { error = result.Error });

        using var writer = new StreamWriter(res.OutputStream);
        await writer.WriteAsync(errorJson);
        return;
    }

    res.StatusCode = (int)HttpStatusCode.Created;

    string json = JsonSerializer.Serialize(result.Value);

    using var writer = new StreamWriter(res.OutputStream);
    await writer.WriteAsync(json);
}
public async Task UpdateUser(
    HttpListenerRequest req,
    HttpListenerResponse res,
    Hashtable props
)
{
    if (!int.TryParse(req.QueryString["uid"], out var uid))
    {
        res.StatusCode = (int)HttpStatusCode.BadRequest;

        string errorJson = JsonSerializer.Serialize(new { error = "Invalid or missing uid" });

        using var writer = new StreamWriter(res.OutputStream);
        await writer.WriteAsync(errorJson);
        return;
    }

    using var reader = new StreamReader(req.InputStream);
    string body = await reader.ReadToEndAsync();

    var data = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

    if (data == null ||
        !data.ContainsKey("username") ||
        !data.ContainsKey("email") ||
        !data.ContainsKey("password"))
    {
        res.StatusCode = (int)HttpStatusCode.BadRequest;

        string errorJson = JsonSerializer.Serialize(new { error = "Missing required fields" });

        using var writer = new StreamWriter(res.OutputStream);
        await writer.WriteAsync(errorJson);
        return;
    }

    string username = data["username"].ToString();
    string email = data["email"].ToString();
    string password = data["password"].ToString();

    var newData = new User(uid, username, email, password);

    var result = await userService.UpdateUser(uid, newData);

    if (!result.Success || result.Value == null)
    {
        res.StatusCode = (int)HttpStatusCode.NotFound;

        string errorJson = JsonSerializer.Serialize(new { error = "User not found" });

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
