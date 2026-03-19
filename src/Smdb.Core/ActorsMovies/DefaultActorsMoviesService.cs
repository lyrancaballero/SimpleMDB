namespace Smdb.Core.ActorsMovies;

using Shared.Http;
using System.Net;

public class DefaultActorsMoviesService : IActorsMoviesService
{
    private readonly IActorsMoviesRepository repo;

    public DefaultActorsMoviesService(IActorsMoviesRepository repo)
    {
        this.repo = repo;
    }

    public async Task<Result<PagedResult<ActorMovie>>> ReadActorsMovies(int page, int size)
    {
        if (page < 1 || size < 1)
            return new Result<PagedResult<ActorMovie>>(new Exception("Invalid pagination"), (int)HttpStatusCode.BadRequest);

        var result = await repo.ReadActorsMovies(page, size);
        return new Result<PagedResult<ActorMovie>>(result!, (int)HttpStatusCode.OK);
    }

    public async Task<Result<ActorMovie>> CreateActorMovie(ActorMovie am)
    {
        if (am.ActorId < 1 || am.MovieId < 1)
            return new Result<ActorMovie>(new Exception("ActorId and MovieId required"), (int)HttpStatusCode.BadRequest);

        var created = await repo.CreateActorMovie(am);
        return new Result<ActorMovie>(created!, (int)HttpStatusCode.Created);
    }

    public async Task<Result<ActorMovie>> ReadActorMovie(int id)
    {
        var am = await repo.ReadActorMovie(id);
        return am == null
            ? new Result<ActorMovie>(new Exception("ActorMovie not found"), (int)HttpStatusCode.NotFound)
            : new Result<ActorMovie>(am, (int)HttpStatusCode.OK);
    }

    public async Task<Result<ActorMovie>> UpdateActorMovie(int id, ActorMovie newData)
    {
        var updated = await repo.UpdateActorMovie(id, newData);
        return updated == null
            ? new Result<ActorMovie>(new Exception("ActorMovie not found"), (int)HttpStatusCode.NotFound)
            : new Result<ActorMovie>(updated, (int)HttpStatusCode.OK);
    }

    public async Task<Result<ActorMovie>> DeleteActorMovie(int id)
    {
        var deleted = await repo.DeleteActorMovie(id);
        return deleted == null
            ? new Result<ActorMovie>(new Exception("ActorMovie not found"), (int)HttpStatusCode.NotFound)
            : new Result<ActorMovie>(deleted, (int)HttpStatusCode.OK);
    }
}
