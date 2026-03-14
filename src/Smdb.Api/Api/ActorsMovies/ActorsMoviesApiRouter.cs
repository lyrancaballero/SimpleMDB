namespace Smdb.Api.ActorsMovies;

using Shared.Http;
using Smdb.Core.ActorsMovies;

public class ActorsMoviesApiRouter : HttpRouter
{
    public ActorsMoviesApiRouter(IActorsMoviesService service)
    {
        var controller = new ActorsMoviesApiController(service);

        MapGet("/", controller.ReadActorsMoviesByActor);
        MapGet("/view", controller.ReadActorsMoviesByMovie);
        MapPost("/", controller.CreateActorsMovies);
        MapDelete("/", controller.DeleteActorsMovies);
    }
}
