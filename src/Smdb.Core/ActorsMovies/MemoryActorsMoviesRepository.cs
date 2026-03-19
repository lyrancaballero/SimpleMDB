namespace Smdb.Core.ActorsMovies;

using Shared.Http;
using Smdb.Core.Db;

public class MemoryActorsMoviesRepository : IActorsMoviesRepository
{
    private readonly MemoryDatabase db;

    public MemoryActorsMoviesRepository(MemoryDatabase db)
    {
        this.db = db;
    }

    public async Task<PagedResult<ActorMovie>?> ReadActorsMovies(int page, int size)
    {
        int totalCount = db.ActorsMovies.Count;
        int start = Math.Clamp((page - 1) * size, 0, totalCount);
        int length = Math.Clamp(size, 0, totalCount - start);

        var values = db.ActorsMovies.Slice(start, length);
        return await Task.FromResult(new PagedResult<ActorMovie>(totalCount, values));
    }

    public async Task<ActorMovie?> CreateActorMovie(ActorMovie am)
    {
        am.Id = db.NextActorMovieId();
        db.ActorsMovies.Add(am);
        return await Task.FromResult(am);
    }

    public async Task<ActorMovie?> ReadActorMovie(int id)
    {
        return await Task.FromResult(db.ActorsMovies.FirstOrDefault(x => x.Id == id));
    }

    public async Task<ActorMovie?> UpdateActorMovie(int id, ActorMovie newData)
    {
        var am = db.ActorsMovies.FirstOrDefault(x => x.Id == id);
        if (am != null)
        {
            am.ActorId = newData.ActorId;
            am.MovieId = newData.MovieId;
        }
        return await Task.FromResult(am);
    }

    public async Task<ActorMovie?> DeleteActorMovie(int id)
    {
        var am = db.ActorsMovies.FirstOrDefault(x => x.Id == id);
        if (am != null) db.ActorsMovies.Remove(am);
        return await Task.FromResult(am);
    }
}
