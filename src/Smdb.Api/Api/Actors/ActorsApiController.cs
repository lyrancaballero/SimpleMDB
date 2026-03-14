namespace Smdb.Api.Actors;

using System.Collections;
using System.Net;
using System.Text.Json;
using Shared.Http;
using Smdb.Core.Actors;

public class ActorsApiController
{
    private IActorService actorService;

    public ActorsApiController(IActorService actorService)
    {
        this.actorService = actorService;
    }

    // CRUD methods will go here
    public async Task ReadActors(
    HttpListenerRequest req,
    HttpListenerResponse res,
    Hashtable props
)
{
    int page = int.TryParse(req.QueryString["page"], out var p) ? p : 1;
    int size = int.TryParse(req.QueryString["size"], out var s) ? s : 10;

    var result = await actorService.ReadActors(page, size);

    res.StatusCode = (int)HttpStatusCode.OK;

    string json = JsonSerializer.Serialize(result.Value);

    using var writer = new StreamWriter(res.OutputStream);
    await writer.WriteAsync(json);
}
public async Task ReadActor(
    HttpListenerRequest req,
    HttpListenerResponse res,
    Hashtable props
)
{
    if (!int.TryParse(req.QueryString["aid"], out var aid))
    {
        res.StatusCode = (int)HttpStatusCode.BadRequest;

        string errorJson = JsonSerializer.Serialize(new { error = "Invalid or missing aid" });

        using var writer = new StreamWriter(res.OutputStream);
        await writer.WriteAsync(errorJson);
        return;
    }

    var result = await actorService.ReadActor(aid);

    if (!result.Success || result.Value == null)
    {
        res.StatusCode = (int)HttpStatusCode.NotFound;

        string errorJson = JsonSerializer.Serialize(new { error = "Actor not found" });

        using var writer = new StreamWriter(res.OutputStream);
        await writer.WriteAsync(errorJson);
        return;
    }

    res.StatusCode = (int)HttpStatusCode.OK;

    string json = JsonSerializer.Serialize(result.Value);

    using var writer = new StreamWriter(res.OutputStream);
    await writer.WriteAsync(json);
}
public async Task CreateActor(
    HttpListenerRequest req,
    HttpListenerResponse res,
    Hashtable props
)
{
    using var reader = new StreamReader(req.InputStream);
    string body = await reader.ReadToEndAsync();

    var data = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

    if (data == null ||
        !data.ContainsKey("name") ||
        !data.ContainsKey("birthYear"))
    {
        res.StatusCode = (int)HttpStatusCode.BadRequest;

        string errorJson = JsonSerializer.Serialize(new { error = "Missing required fields" });

        using var writer = new StreamWriter(res.OutputStream);
        await writer.WriteAsync(errorJson);
        return;
    }

    string name = data["name"].ToString();
    int birthYear = int.Parse(data["birthYear"].ToString());

    var actor = new Actor(0, name, birthYear);

    var result = await actorService.CreateActor(actor);

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
public async Task UpdateActor(
    HttpListenerRequest req,
    HttpListenerResponse res,
    Hashtable props
)
{
    if (!int.TryParse(req.QueryString["aid"], out var aid))
    {
        res.StatusCode = (int)HttpStatusCode.BadRequest;

        string errorJson = JsonSerializer.Serialize(new { error = "Invalid or missing aid" });

        using var writer = new StreamWriter(res.OutputStream);
        await writer.WriteAsync(errorJson);
        return;
    }

    using var reader = new StreamReader(req.InputStream);
    string body = await reader.ReadToEndAsync();

    var data = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

    if (data == null ||
        !data.ContainsKey("name") ||
        !data.ContainsKey("birthYear"))
    {
        res.StatusCode = (int)HttpStatusCode.BadRequest;

        string errorJson = JsonSerializer.Serialize(new { error = "Missing required fields" });

        using var writer = new StreamWriter(res.OutputStream);
        await writer.WriteAsync(errorJson);
        return;
    }

    string name = data["name"].ToString();
    int birthYear = int.Parse(data["birthYear"].ToString());

    var newData = new Actor(aid, name, birthYear);

    var result = await actorService.UpdateActor(aid, newData);

    if (!result.Success || result.Value == null)
    {
        res.StatusCode = (int)HttpStatusCode.NotFound;

        string errorJson = JsonSerializer.Serialize(new { error = "Actor not found" });

        using var writer = new StreamWriter(res.OutputStream);
        await writer.WriteAsync(errorJson);
        return;
    }

    res.StatusCode = (int)HttpStatusCode.OK;

    string json = JsonSerializer.Serialize(result.Value);

    using var writer = new StreamWriter(res.OutputStream);
    await writer.WriteAsync(json);
}
public async Task DeleteActor(
    HttpListenerRequest req,
    HttpListenerResponse res,
    Hashtable props
)
{
    if (!int.TryParse(req.QueryString["aid"], out var aid))
    {
        res.StatusCode = (int)HttpStatusCode.BadRequest;

        string errorJson = JsonSerializer.Serialize(new { error = "Invalid or missing aid" });

        using var writer = new StreamWriter(res.OutputStream);
        await writer.WriteAsync(errorJson);
        return;
    }

    var result = await actorService.DeleteActor(aid);

    if (!result.Success || result.Value == null)
    {
        res.StatusCode = (int)HttpStatusCode.NotFound;

        string errorJson = JsonSerializer.Serialize(new { error = "Actor not found" });

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
