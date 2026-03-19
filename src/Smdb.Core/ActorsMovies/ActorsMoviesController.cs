namespace Smdb.Api.ActorsMovies;

using System.Net;
using System.Collections;
using System.Collections.Specialized;
using System.Text.Json;
using Shared.Http;
using Smdb.Core.ActorsMovies;

public class ActorsMoviesController
{
    private readonly IActorsMoviesService service;

    public ActorsMoviesController(IActorsMoviesService service)
    {
        this.service = service;
    }

    public async Task ReadActorsMovies(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        int page = int.TryParse(req.QueryString["page"], out int p) ? p : 1;
        int size = int.TryParse(req.QueryString["size"], out int s) ? s : 10;

        var result = await service.ReadActorsMovies(page, size);
        await JsonUtils.SendPagedResultResponse(req, res, props, result, page, size);
        await next();
    }

    public async Task CreateActorMovie(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var text = (string)props["req.text"]!;
        var am = JsonSerializer.Deserialize<ActorMovie>(text, JsonUtils.DefaultOptions);

        var result = await service.CreateActorMovie(am!);
        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task ReadActorMovie(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.Parse(uParams["id"]!);

        var result = await service.ReadActorMovie(id);
        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task UpdateActorMovie(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.Parse(uParams["id"]!);

        var text = (string)props["req.text"]!;
        var am = JsonSerializer.Deserialize<ActorMovie>(text, JsonUtils.DefaultOptions);

        var result = await service.UpdateActorMovie(id, am!);
        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    public async Task DeleteActorMovie(HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.Parse(uParams["id"]!);

        var result = await service.DeleteActorMovie(id);
        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }
}
