namespace Smdb.Api.Actors;

using System.Net;
using System.Collections;
using System.Collections.Specialized;
using System.Text.Json;
using Shared.Http;
using Smdb.Core.Actors;

public class ActorsController
{
    private readonly IActorsService service;

    public ActorsController(IActorsService service)
    {
        this.service = service;
    }

    public async Task ReadActors(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        int page = int.TryParse(req.QueryString["page"], out int p) ? p : 1;
        int size = int.TryParse(req.QueryString["size"], out int s) ? s : 10;

        var result = await service.ReadActors(page, size);
        await JsonUtils.SendPagedResultResponse(req, res, props, result, page, size);
        await next();
    }

    public async Task CreateActor(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var text = (string)props["req.text"]!;
        var actor = JsonSerializer.Deserialize<Actor>(text, JsonUtils.DefaultOptions);

        var result = await service.CreateActor(actor!);
        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task ReadActor(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.Parse(uParams["id"]!);

        var result = await service.ReadActor(id);
        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task UpdateActor(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.Parse(uParams["id"]!);

        var text = (string)props["req.text"]!;
        var actor = JsonSerializer.Deserialize<Actor>(text, JsonUtils.DefaultOptions);

        var result = await service.UpdateActor(id, actor!);
        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task DeleteActor(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.Parse(uParams["id"]!);

        var result = await service.DeleteActor(id);
        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }
}
