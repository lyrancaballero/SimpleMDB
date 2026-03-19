namespace Smdb.Api.Users;

using Shared.Http;

public class UsersRouter : HttpRouter
{
    public UsersRouter(UsersController ctrl)
    {
        UseParametrizedRouteMatching();

        MapGet("/", ctrl.ReadUsers);
        MapPost("/", HttpUtils.ReadRequestBodyAsText, ctrl.CreateUser);
        MapGet("/:id", ctrl.ReadUser);
        MapPut("/:id", HttpUtils.ReadRequestBodyAsText, ctrl.UpdateUser);
        MapDelete("/:id", ctrl.DeleteUser);
    }
}
