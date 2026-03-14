namespace Smdb.Api.Users;

using Shared.Http;
using Smdb.Core.Users;

public class UsersApiRouter : HttpRouter
{
    public UsersApiRouter(IUserService userService)
    {
        var controller = new UsersApiController(userService);

        MapGet("/", controller.ReadUsers);
        MapGet("/view", controller.ReadUser);
        MapPost("/", controller.CreateUser);
        MapPut("/update", controller.UpdateUser);
        MapDelete("/delete", controller.DeleteUser);
    }
}
