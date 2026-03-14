namespace Smdb.Api.Movies;
using System.Collections;
using System.Collections.Specialized;
using System.Net;
using System.Text.Json;
using Shared.Http;
using Smdb.Core.Movies;

public class MoviesApiController
{
    private IMovieService movieService;

    public MoviesApiController(IMovieService movieService)
    {
        this.movieService = movieService;
    }

    // API methods will go here
    public async Task ReadMovies(
    HttpListenerRequest req,
    HttpListenerResponse res,
    Hashtable props
)
    {
        int page = int.TryParse(req.QueryString["page"], out var p) ? p : 1;
int size = int.TryParse(req.QueryString["size"], out var s) ? s : 10;

var result = await movieService.ReadMovies(page, size);

res.StatusCode = (int)HttpStatusCode.OK;

string json = JsonSerializer.Serialize(result.Value);

using var writer = new StreamWriter(res.OutputStream);
await writer.WriteAsync(json);

    }

public async Task ReadMovie(
    HttpListenerRequest req,
    HttpListenerResponse res,
    Hashtable props
)
    {
        if (!int.TryParse(req.QueryString["id"], out var id))
{
    res.StatusCode = (int)HttpStatusCode.BadRequest;

    string errorJson = JsonSerializer.Serialize(new { error = "Invalid or missing id" });

    using var writer = new StreamWriter(res.OutputStream);
    await writer.WriteAsync(errorJson);
    return;
}
var result = await movieService.ReadMovie(id);

if (!result.Success || result.Value == null)
{
    res.StatusCode = (int)HttpStatusCode.NotFound;

    string errorJson = JsonSerializer.Serialize(new { error = "Movie not found" });

    using var writer = new StreamWriter(res.OutputStream);
    await writer.WriteAsync(errorJson);
    return;
}
res.StatusCode = (int)HttpStatusCode.OK;

string json = JsonSerializer.Serialize(result.Value);

using var writer = new StreamWriter(res.OutputStream);
await writer.WriteAsync(json);

    }
public async Task CreateMovie(
    HttpListenerRequest req,
    HttpListenerResponse res,
    Hashtable props
)
{
    using var reader = new StreamReader(req.InputStream);
    string body = await reader.ReadToEndAsync();

    var data = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

    if (data == null ||
        !data.ContainsKey("title") ||
        !data.ContainsKey("year") ||
        !data.ContainsKey("description"))
    {
        res.StatusCode = (int)HttpStatusCode.BadRequest;

        string errorJson = JsonSerializer.Serialize(new { error = "Missing required fields" });

        using var writer = new StreamWriter(res.OutputStream);
        await writer.WriteAsync(errorJson);
        return;
    }

    string title = data["title"].ToString();
    int year = Convert.ToInt32(data["year"]);
    string description = data["description"].ToString();

    var movie = new Movie(0, title, year, description);

    var result = await movieService.CreateMovie(movie);

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
public async Task UpdateMovie(
    HttpListenerRequest req,
    HttpListenerResponse res,
    Hashtable props
)
{
    if (!int.TryParse(req.QueryString["id"], out var id))
    {
        res.StatusCode = (int)HttpStatusCode.BadRequest;

        string errorJson = JsonSerializer.Serialize(new { error = "Invalid or missing id" });

        using var writer = new StreamWriter(res.OutputStream);
        await writer.WriteAsync(errorJson);
        return;
    }

    using var reader = new StreamReader(req.InputStream);
    string body = await reader.ReadToEndAsync();

    var data = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

    if (data == null ||
        !data.ContainsKey("title") ||
        !data.ContainsKey("year") ||
        !data.ContainsKey("description"))
    {
        res.StatusCode = (int)HttpStatusCode.BadRequest;

        string errorJson = JsonSerializer.Serialize(new { error = "Missing required fields" });

        using var writer = new StreamWriter(res.OutputStream);
        await writer.WriteAsync(errorJson);
        return;
    }

    string title = data["title"].ToString();
    int year = Convert.ToInt32(data["year"]);
    string description = data["description"].ToString();

    var newData = new Movie(id, title, year, description);

    var result = await movieService.UpdateMovie(id, newData);

    if (!result.Success || result.Value == null)
    {
        res.StatusCode = (int)HttpStatusCode.NotFound;

        string errorJson = JsonSerializer.Serialize(new { error = "Movie not found" });

        using var writer = new StreamWriter(res.OutputStream);
        await writer.WriteAsync(errorJson);
        return;
    }

    res.StatusCode = (int)HttpStatusCode.OK;

    string json = JsonSerializer.Serialize(result.Value);

    using var writer = new StreamWriter(res.OutputStream);
    await writer.WriteAsync(json);
}
public async Task DeleteMovie(
    HttpListenerRequest req,
    HttpListenerResponse res,
    Hashtable props
)
{
    if (!int.TryParse(req.QueryString["id"], out var id))
    {
        res.StatusCode = (int)HttpStatusCode.BadRequest;

        string errorJson = JsonSerializer.Serialize(new { error = "Invalid or missing id" });

        using var writer = new StreamWriter(res.OutputStream);
        await writer.WriteAsync(errorJson);
        return;
    }

    var result = await movieService.DeleteMovie(id);

    if (!result.Success || result.Value == null)
    {
        res.StatusCode = (int)HttpStatusCode.NotFound;

        string errorJson = JsonSerializer.Serialize(new { error = "Movie not found" });

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
