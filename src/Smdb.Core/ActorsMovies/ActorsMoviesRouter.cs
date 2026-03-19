namespace Smdb.Api.ActorsMovies;

using Shared.Http;

public class ActorsMoviesRouter : HttpRouter
{
    public ActorsMoviesRouter(ActorsMoviesController ctrl)
    {
        UseParametrizedRouteMatching();

        MapGet("/", ctrl.ReadActorsMovies);
        MapPost("/", HttpUtils.ReadRequestBodyAsText, ctrl.CreateActorMovie);
        MapGet("/:id", ctrl.ReadActorMovie);
        MapPut("/:id", HttpUtils.ReadRequestBodyAsText, ctrl.UpdateActorMovie);
        MapDelete("/:id", ctrl.DeleteActorMovie);
    }
}
