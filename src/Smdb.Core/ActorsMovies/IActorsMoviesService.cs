namespace Smdb.Core.ActorsMovies;

using Shared.Http;

public interface IActorsMoviesService
{
    Task<Result<PagedResult<ActorMovie>>> ReadActorsMovies(int page, int size);
    Task<Result<ActorMovie>> CreateActorMovie(ActorMovie am);
    Task<Result<ActorMovie>> ReadActorMovie(int id);
    Task<Result<ActorMovie>> UpdateActorMovie(int id, ActorMovie newData);
    Task<Result<ActorMovie>> DeleteActorMovie(int id);
}
