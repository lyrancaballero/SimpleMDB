public class ActorsMoviesApiController
{
    private IActorsMoviesService service;

    public ActorsMoviesApiController(IActorsMoviesService service)
    {
        this.service = service;
    }

    // Methods will go here
    public async Task ReadActorsMoviesByActor(
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

    var result = await service.ReadByActor(aid);

    if (!result.Success || result.Value == null)
    {
        res.StatusCode = (int)HttpStatusCode.NotFound;

        string errorJson = JsonSerializer.Serialize(new { error = "Actor not found or has no movies" });

        using var writer = new StreamWriter(res.OutputStream);
        await writer.WriteAsync(errorJson);
        return;
    }

    res.StatusCode = (int)HttpStatusCode.OK;

    string json = JsonSerializer.Serialize(result.Value);

    using var writer = new StreamWriter(res.OutputStream);
    await writer.WriteAsync(json);
}
public async Task ReadActorsMoviesByMovie(
    HttpListenerRequest req,
    HttpListenerResponse res,
    Hashtable props
)
{
    if (!int.TryParse(req.QueryString["mid"], out var mid))
    {
        res.StatusCode = (int)HttpStatusCode.BadRequest;

        string errorJson = JsonSerializer.Serialize(new { error = "Invalid or missing mid" });

        using var writer = new StreamWriter(res.OutputStream);
        await writer.WriteAsync(errorJson);
        return;
    }

    var result = await service.ReadByMovie(mid);

    if (!result.Success || result.Value == null)
    {
        res.StatusCode = (int)HttpStatusCode.NotFound;

        string errorJson = JsonSerializer.Serialize(new { error = "Movie not found or has no actors" });

        using var writer = new StreamWriter(res.OutputStream);
        await writer.WriteAsync(errorJson);
        return;
    }

    res.StatusCode = (int)HttpStatusCode.OK;

    string json = JsonSerializer.Serialize(result.Value);

    using var writer = new StreamWriter(res.OutputStream);
    await writer.WriteAsync(json);
}
public async Task CreateActorsMovies(
    HttpListenerRequest req,
    HttpListenerResponse res,
    Hashtable props
)
{
    using var reader = new StreamReader(req.InputStream);
    string body = await reader.ReadToEndAsync();

    var data = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

    if (data == null ||
        !data.ContainsKey("aid") ||
        !data.ContainsKey("mid"))
    {
        res.StatusCode = (int)HttpStatusCode.BadRequest;

        string errorJson = JsonSerializer.Serialize(new { error = "Missing aid or mid" });

        using var writer = new StreamWriter(res.OutputStream);
        await writer.WriteAsync(errorJson);
        return;
    }

    int aid = int.Parse(data["aid"].ToString());
    int mid = int.Parse(data["mid"].ToString());

    var result = await service.Create(aid, mid);

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
public async Task DeleteActorsMovies(
    HttpListenerRequest req,
    HttpListenerResponse res,
    Hashtable props
)
{
    if (!int.TryParse(req.QueryString["aid"], out var aid) ||
        !int.TryParse(req.QueryString["mid"], out var mid))
    {
        res.StatusCode = (int)HttpStatusCode.BadRequest;

        string errorJson = JsonSerializer.Serialize(new { error = "Invalid or missing aid or mid" });

        using var writer = new StreamWriter(res.OutputStream);
        await writer.WriteAsync(errorJson);
        return;
    }

    var result = await service.Delete(aid, mid);

    if (!result.Success || result.Value == null)
    {
        res.StatusCode = (int)HttpStatusCode.NotFound;

        string errorJson = JsonSerializer.Serialize(new { error = "Relation not found" });

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
