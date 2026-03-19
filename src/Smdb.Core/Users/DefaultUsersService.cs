namespace Smdb.Core.Users;

using Shared.Http;
using System.Net;

public class DefaultUsersService : IUsersService
{
    private readonly IUsersRepository repo;

    public DefaultUsersService(IUsersRepository repo)
    {
        this.repo = repo;
    }

    public async Task<Result<PagedResult<User>>> ReadUsers(int page, int size)
    {
        if (page < 1)
            return new Result<PagedResult<User>>(new Exception("Page must be >= 1"), (int)HttpStatusCode.BadRequest);

        if (size < 1)
            return new Result<PagedResult<User>>(new Exception("Size must be >= 1"), (int)HttpStatusCode.BadRequest);

        var result = await repo.ReadUsers(page, size);
        return result == null
            ? new Result<PagedResult<User>>(new Exception("Could not read users"), (int)HttpStatusCode.NotFound)
            : new Result<PagedResult<User>>(result, (int)HttpStatusCode.OK);
    }

    public async Task<Result<User>> CreateUser(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Username))
            return new Result<User>(new Exception("Username required"), (int)HttpStatusCode.BadRequest);

        var created = await repo.CreateUser(user);
        return new Result<User>(created!, (int)HttpStatusCode.Created);
    }

    public async Task<Result<User>> ReadUser(int id)
    {
        var user = await repo.ReadUser(id);
        return user == null
            ? new Result<User>(new Exception("User not found"), (int)HttpStatusCode.NotFound)
            : new Result<User>(user, (int)HttpStatusCode.OK);
    }

    public async Task<Result<User>> UpdateUser(int id, User newData)
    {
        var updated = await repo.UpdateUser(id, newData);
        return updated == null
            ? new Result<User>(new Exception("User not found"), (int)HttpStatusCode.NotFound)
            : new Result<User>(updated, (int)HttpStatusCode.OK);
    }

    public async Task<Result<User>> DeleteUser(int id)
    {
        var deleted = await repo.DeleteUser(id);
        return deleted == null
            ? new Result<User>(new Exception("User not found"), (int)HttpStatusCode.NotFound)
            : new Result<User>(deleted, (int)HttpStatusCode.OK);
    }
}
