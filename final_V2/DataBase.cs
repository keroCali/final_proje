using SQLite;

namespace final_V2;

public class DataBase
{
    private SQLiteAsyncConnection _database;

    async Task Init()
    {
        if (_database is not null) return;

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "ReelsDb");

        _database = new SQLiteAsyncConnection(dbPath);
        await _database.CreateTableAsync<Item>();
        await _database.CreateTableAsync<User>();
        await _database.CreateTableAsync<Follow>();
    }

    public async Task<List<Item>> GetReelsAsync()
    {
        await Init();
        return await _database.Table<Item>().ToListAsync();
    }

    public async Task<List<Item>> GetFriendsActivityAsync(string currentUsername)
    {
        await Init();
        var followed = await _database.Table<Follow>()
            .Where(f => f.FollowerUsername == currentUsername)
            .ToListAsync();
        
        var followedUsernames = followed.Select(f => f.TagetUsername).ToList();
        
        // SQLite-net PCL has limited support for complicated joins/IN clauses in linq, 
        // so we'll fetch and filter if necessary or use a simple query.
        return await _database.Table<Item>()
            .Where(i => followedUsernames.Contains(i.Username))
            .OrderByDescending(i => i.ID)
            .ToListAsync();
    }

    public async Task FollowUserAsync(string follower, string target)
    {
        await Init();
        var exists = await _database.Table<Follow>()
            .Where(f => f.FollowerUsername == follower && f.TagetUsername == target)
            .FirstOrDefaultAsync();
        
        if (exists == null)
        {
            await _database.InsertAsync(new Follow { FollowerUsername = follower, TagetUsername = target });
        }
    }

    public async Task UnfollowUserAsync(string follower, string target)
    {
        await Init();
        var follow = await _database.Table<Follow>()
            .Where(f => f.FollowerUsername == follower && f.TagetUsername == target)
            .FirstOrDefaultAsync();
        if (follow != null)
        {
            await _database.DeleteAsync(follow);
        }
    }

    public async Task<bool> IsFollowingAsync(string follower, string target)
    {
        await Init();
        var follow = await _database.Table<Follow>()
            .Where(f => f.FollowerUsername == follower && f.TagetUsername == target)
            .FirstOrDefaultAsync();
        return follow != null;
    }

    public async Task<List<User>> SearchUsersAsync(string query, string currentUsername)
    {
        await Init();
        return await _database.Table<User>()
            .Where(u => u.Username != currentUsername && u.Username.ToLower().Contains(query.ToLower()))
            .ToListAsync();
    }

    public async Task<int> GetFollowerCountAsync(string username)
    {
        await Init();
        return await _database.Table<Follow>().Where(f => f.TagetUsername == username).CountAsync();
    }

    public async Task<int> GetFollowingCountAsync(string username)
    {
        await Init();
        return await _database.Table<Follow>().Where(f => f.FollowerUsername == username).CountAsync();
    }

    public async Task<Item> GetTopRatedReelAsync(string username)
    {
        await Init();
        return await _database.Table<Item>()
            .Where(i => i.Username == username)
            .OrderByDescending(i => i.Rating)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateUserBioAsync(string username, string newBio)
    {
        await Init();
        var user = await _database.Table<User>().Where(u => u.Username == username).FirstOrDefaultAsync();
        if (user != null)
        {
            user.Bio = newBio;
            await _database.UpdateAsync(user);
        }
    }

    public async Task<List<Item>> GetUserReelsAsync(string username)
    {
        await Init();
        return await _database.Table<Item>().Where(i => i.Username == username).ToListAsync();
    }

    public async Task AddReelsAsync(Item item)
    {
        await Init();
        await _database.InsertAsync(item);
    }

    public async Task<User> LoginAsync(string username, string password)
    {
        await Init();
        return await _database.Table<User>().Where(u => u.Username == username && u.Password == password).FirstOrDefaultAsync();
    }

    public async Task RegisterAsync(User user)
    {
        await Init();
        await _database.InsertAsync(user);
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        await Init();
        return await _database.Table<User>().ToListAsync();
    }
}