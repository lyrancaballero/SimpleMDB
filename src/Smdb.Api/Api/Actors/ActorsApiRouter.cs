namespace Smdb.Api.Actors;

using Shared.Http;
using Smdb.Core.Actors;

public class ActorsApiRouter : HttpRouter
{
    public ActorsApiRouter(IActorService actorService)
    {
        var controller = new ActorsApiController(actorService);

        MapGet("/", controller.ReadActors);
        MapGet("/view", controller.ReadActor);
        MapPost("/", controller.CreateActor);
        MapPut("/update", controller.UpdateActor);
        MapDelete("/delete", controller.DeleteActor);
    }
}
