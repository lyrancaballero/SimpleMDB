namespace Smdb.Api.Auth;

using Shared.Http;
using Smdb.Core.Auth;

public class AuthApiRouter : HttpRouter
{
    public AuthApiRouter(IAuthService authService)
    {
        var controller = new AuthApiController(authService);

        MapPost("/login", controller.Login);
    }
}
