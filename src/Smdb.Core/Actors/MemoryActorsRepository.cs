namespace Smdb.Core.Actors;

using Shared.Http;
using Smdb.Core.Db;

public class MemoryActorsRepository : IActorsRepository
{
    private readonly MemoryDatabase db;

    public MemoryActorsRepository(MemoryDatabase db)
    {
        this.db = db;
    }

    public async Task<PagedResult<Actor>?> ReadActors(int page, int size)
    {
        int totalCount = db.Actors.Count;
        int start = Math.Clamp((page - 1) * size, 0, totalCount);
        int length = Math.Clamp(size, 0, totalCount - start);

        var values = db.Actors.Slice(start, length);
        return await Task.FromResult(new PagedResult<Actor>(totalCount, values));
    }

    public async Task<Actor?> CreateActor(Actor actor)
    {
        actor.Id = db.NextActorId();
        db.Actors.Add(actor);
        return await Task.FromResult(actor);
    }

    public async Task<Actor?> ReadActor(int id)
    {
        return await Task.FromResult(db.Actors.FirstOrDefault(a => a.Id == id));
    }

    public async Task<Actor?> UpdateActor(int id, Actor newData)
    {
        var actor = db.Actors.FirstOrDefault(a => a.Id == id);
        if (actor != null)
        {
            actor.Name = newData.Name;
            actor.BirthYear = newData.BirthYear;
        }
        return await Task.FromResult(actor);
    }

    public async Task<Actor?> DeleteActor(int id)
    {
        var actor = db.Actors.FirstOrDefault(a => a.Id == id);
        if (actor != null) db.Actors.Remove(actor);
        return await Task.FromResult(actor);
    }
}
