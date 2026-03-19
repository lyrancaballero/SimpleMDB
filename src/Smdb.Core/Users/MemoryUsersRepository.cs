namespace Smdb.Core.Users;

using Shared.Http;
using Smdb.Core.Db;

public class MemoryUsersRepository : IUsersRepository
{
    private readonly MemoryDatabase db;

    public MemoryUsersRepository(MemoryDatabase db)
    {
        this.db = db;
    }

    public async Task<PagedResult<User>?> ReadUsers(int page, int size)
    {
        int totalCount = db.Users.Count;
        int start = Math.Clamp((page - 1) * size, 0, totalCount);
        int length = Math.Clamp(size, 0, totalCount - start);

        var values = db.Users.Slice(start, length);
        return await Task.FromResult(new PagedResult<User>(totalCount, values));
    }

    public async Task<User?> CreateUser(User newUser)
    {
        newUser.Id = db.NextUserId();
        db.Users.Add(newUser);
        return await Task.FromResult(newUser);
    }

    public async Task<User?> ReadUser(int id)
    {
        return await Task.FromResult(db.Users.FirstOrDefault(u => u.Id == id));
    }

    public async Task<User?> UpdateUser(int id, User newData)
    {
        var user = db.Users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            user.Username = newData.Username;
            user.Password = newData.Password;
            user.Role = newData.Role;
        }
        return await Task.FromResult(user);
    }

    public async Task<User?> DeleteUser(int id)
    {
        var user = db.Users.FirstOrDefault(u => u.Id == id);
        if (user != null) db.Users.Remove(user);
        return await Task.FromResult(user);
    }
}
