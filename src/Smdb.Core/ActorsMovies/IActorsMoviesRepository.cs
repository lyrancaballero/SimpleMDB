namespace Smdb.Core.ActorsMovies;

using Shared.Http;

public interface IActorsMoviesRepository
{
    Task<PagedResult<ActorMovie>?> ReadActorsMovies(int page, int size);
    Task<ActorMovie?> CreateActorMovie(ActorMovie am);
    Task<ActorMovie?> ReadActorMovie(int id);
    Task<ActorMovie?> UpdateActorMovie(int id, ActorMovie newData);
    Task<ActorMovie?> DeleteActorMovie(int id);
}
