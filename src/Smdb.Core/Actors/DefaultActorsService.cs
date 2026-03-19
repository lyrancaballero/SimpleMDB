namespace Smdb.Core.Actors;

using Shared.Http;
using System.Net;

public class DefaultActorsService : IActorsService
{
    private readonly IActorsRepository repo;

    public DefaultActorsService(IActorsRepository repo)
    {
        this.repo = repo;
    }

    public async Task<Result<PagedResult<Actor>>> ReadActors(int page, int size)
    {
        if (page < 1 || size < 1)
            return new Result<PagedResult<Actor>>(new Exception("Invalid pagination"), (int)HttpStatusCode.BadRequest);

        var result = await repo.ReadActors(page, size);
        return new Result<PagedResult<Actor>>(result!, (int)HttpStatusCode.OK);
    }

    public async Task<Result<Actor>> CreateActor(Actor actor)
    {
        if (string.IsNullOrWhiteSpace(actor.Name))
            return new Result<Actor>(new Exception("Name required"), (int)HttpStatusCode.BadRequest);

        var created = await repo.CreateActor(actor);
        return new Result<Actor>(created!, (int)HttpStatusCode.Created);
    }

    public async Task<Result<Actor>> ReadActor(int id)
    {
        var actor = await repo.ReadActor(id);
        return actor == null
            ? new Result<Actor>(new Exception("Actor not found"), (int)HttpStatusCode.NotFound)
            : new Result<Actor>(actor, (int)HttpStatusCode.OK);
    }

    public async Task<Result<Actor>> UpdateActor(int id, Actor newData)
    {
        var updated = await repo.UpdateActor(id, newData);
        return updated == null
            ? new Result<Actor>(new Exception("Actor not found"), (int)HttpStatusCode.NotFound)
            : new Result<Actor>(updated, (int)HttpStatusCode.OK);
    }

    public async Task<Result<Actor>> DeleteActor(int id)
    {
        var deleted = await repo.DeleteActor(id);
        return deleted == null
            ? new Result<Actor>(new Exception("Actor not found"), (int)HttpStatusCode.NotFound)
            : new Result<Actor>(deleted, (int)HttpStatusCode.OK);
    }
}
