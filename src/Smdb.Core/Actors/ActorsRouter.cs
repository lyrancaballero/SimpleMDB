namespace Smdb.Api.Actors;

using Shared.Http;

public class ActorsRouter : HttpRouter
{
    public ActorsRouter(ActorsController ctrl)
    {
        UseParametrizedRouteMatching();

        MapGet("/", ctrl.ReadActors);
        MapPost("/", HttpUtils.ReadRequestBodyAsText, ctrl.CreateActor);
        MapGet("/:id", ctrl.ReadActor);
        MapPut("/:id", HttpUtils.ReadRequestBodyAsText, ctrl.UpdateActor);
        MapDelete("/:id", ctrl.DeleteActor);
    }
}
