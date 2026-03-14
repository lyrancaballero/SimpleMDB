namespace Smdb.Api;
using Shared.Http;
using Smdb.Api.Movies;
using Smdb.Core.Movies;
using Smdb.Core.Users;
using Smdb.Api.Users;
using Smdb.Core.Auth;
using Smdb.Api.Auth;
using Smdb.Core.Actors;
using Smdb.Api.Actors;
using Smdb.Core.ActorsMovies;
using Smdb.Api.ActorsMovies;

public class App : HttpServer
{
// <-- Rest of the code below goes here.
public override void Init()
{
/*
var actorsMoviesService = new DefaultActorsMoviesService(new MemoryActorsMoviesRepository());
var actorsMoviesApiRouter = new ActorsMoviesApiRouter(actorsMoviesService);
router.Map("/api/v1/actors-movies", actorsMoviesApiRouter.HandleAsync);

var actorService = new DefaultActorService(new MemoryActorRepository());
var actorsApiRouter = new ActorsApiRouter(actorService);
router.Map("/api/v1/actors", actorsApiRouter.HandleAsync);

var authService = new DefaultAuthService(new MemoryAuthRepository());
var authApiRouter = new AuthApiRouter(authService);
router.Map("/api/v1/auth", authApiRouter.HandleAsync);
    
var userService = new DefaultUserService(new MemoryUserRepository());
var usersApiRouter = new UsersApiRouter(userService);
router.Map("/api/v1/users", usersApiRouter.HandleAsync);

var movieService = new DefaultMovieService(new MemoryMovieRepository());
var moviesApiRouter = new MoviesApiRouter(movieService);
router.Map("/api/v1/movies", moviesApiRouter.HandleAsync);
*/
var db = new MemoryDatabase();
var movieRepo = new MemoryMovieRepository(db);
var movieServ = new DefaultMovieService(movieRepo);
var movieCtrl = new MoviesController(movieServ);
var movieRouter = new MoviesRouter(movieCtrl);
var apiRouter = new HttpRouter();

router.Use(HttpUtils.StructuredLogging);
router.Use(HttpUtils.CentralizedErrorHandling);
router.Use(HttpUtils.AddResponseCorsHeaders);
router.Use(HttpUtils.DefaultResponse);
router.Use(HttpUtils.ParseRequestUrl);
router.Use(HttpUtils.ParseRequestQueryString);
router.UseParametrizedRouteMatching();

router.UseRouter("/api/v1", apiRouter);
apiRouter.UseRouter("/movies", movieRouter);
}
}