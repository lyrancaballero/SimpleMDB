namespace Smdb.Api.Movies;

using Shared.Http;
using Smdb.Core.Movies;

public class MoviesApiRouter : HttpRouter
{
    public MoviesApiRouter(IMovieService movieService)
    {
        var controller = new MoviesApiController(movieService);

        MapGet("/", controller.ReadMovies);
        MapGet("/view", controller.ReadMovie);
        MapPost("/", controller.CreateMovie);
        MapPut("/update", controller.UpdateMovie);
        MapDelete("/delete", controller.DeleteMovie);
    }
}
