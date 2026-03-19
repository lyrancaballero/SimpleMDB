namespace Smdb.Core.ActorsMovies;

public class ActorMovie
{
    public int Id { get; set; }
    public int ActorId { get; set; }
    public int MovieId { get; set; }

    public ActorMovie(int id, int actorId, int movieId)
    {
        Id = id;
        ActorId = actorId;
        MovieId = movieId;
    }

    public override string ToString()
    {
        return $"ActorMovie[Id={Id}, ActorId={ActorId}, MovieId={MovieId}]";
    }
}
