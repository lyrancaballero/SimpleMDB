
namespace Smdb.Api;

using Shared.Http;
using Smdb.Api.Movies;
using Smdb.Core.Movies;
using Smdb.Core.Db;
//Users
using Smdb.Core.Users;
using Smdb.Api.Users;

//Actors
using Smdb.Core.Actors;
using Smdb.Api.Actors;

//ActorsMovies
using Smdb.Core.ActorsMovies;
using Smdb.Api.ActorsMovies;




public class App : HttpServer
{
    public App()
    {

    }

    public override void Init()
    {
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

        //Users Start

        var usersRepo = new MemoryUsersRepository(db);
        var usersServ = new DefaultUsersService(usersRepo);
        var usersCtrl = new UsersController(usersServ);
        var usersRouter = new UsersRouter(usersCtrl);

        apiRouter.UseRouter("/users", usersRouter);

        //Users End

        //Actors Start

        var actorsRepo = new MemoryActorsRepository(db);
        var actorsServ = new DefaultActorsService(actorsRepo);
        var actorsCtrl = new ActorsController(actorsServ);
        var actorsRouter = new ActorsRouter(actorsCtrl);

        apiRouter.UseRouter("/actors", actorsRouter);

        //Actors End


        //ActorsMovies Start

        var amRepo = new MemoryActorsMoviesRepository(db);
        var amServ = new DefaultActorsMoviesService(amRepo);
        var amCtrl = new ActorsMoviesController(amServ);
        var amRouter = new ActorsMoviesRouter(amCtrl);

        apiRouter.UseRouter("/actors-movies", amRouter);

        //ActorsMovies End
    }


}