namespace Smdb.Api.Users;

using System.Collections;
using System.Collections.Specialized;
using System.Text.Json;
using Shared.Http;
using Smdb.Core.Users;

using System.Net;


public class UsersController
{
    private readonly IUsersService service;

    public UsersController(IUsersService service)
    {
        this.service = service;
    }

    public async Task ReadUsers(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        int page = int.TryParse(req.QueryString["page"], out int p) ? p : 1;
        int size = int.TryParse(req.QueryString["size"], out int s) ? s : 10;

        var result = await service.ReadUsers(page, size);
        await JsonUtils.SendPagedResultResponse(req, res, props, result, page, size);
        await next();
    }

    public async Task CreateUser(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var text = (string)props["req.text"]!;
        var user = JsonSerializer.Deserialize<User>(text, JsonUtils.DefaultOptions);

        var result = await service.CreateUser(user!);
        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task ReadUser(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.Parse(uParams["id"]!);

        var result = await service.ReadUser(id);
        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task UpdateUser(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.Parse(uParams["id"]!);

        var text = (string)props["req.text"]!;
        var user = JsonSerializer.Deserialize<User>(text, JsonUtils.DefaultOptions);

        var result = await service.UpdateUser(id, user!);
        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task DeleteUser(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.Parse(uParams["id"]!);

        var result = await service.DeleteUser(id);
        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }
}
