using Buzile_TicTacToe.DBContext;
using Buzile_TicTacToe.Interfaces;
using Buzile_TicTacToe.Models;
using Microsoft.EntityFrameworkCore;

namespace Buzile_TicTacToe.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationContext _applicationContext;

    public UserRepository(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task Create(User user, CancellationToken token = default)
    {
        _applicationContext.Set<User>().Add(user);
        await _applicationContext.SaveChangesAsync(token);
    }
    

    public async Task<User> Get(string login, CancellationToken token = default)
    {
       return await _applicationContext.Set<User>().FirstOrDefaultAsync(x=>x.Name == login, token);
    }
    public async Task<User> Get(Guid id, CancellationToken token = default)
    {
        return await _applicationContext.Set<User>().FirstOrDefaultAsync(x=>x.Id == id, token);
    }

    public async Task<bool> UserExists(string login, CancellationToken token)
    {
        return await _applicationContext.Set<User>().AnyAsync(x => x.Name == login, token);
    }
}