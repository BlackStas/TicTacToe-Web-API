using Buzile_TicTacToe.Interfaces;
using Buzile_TicTacToe.Models;

namespace Buzile_TicTacToe.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<Guid> Create(string login, CancellationToken token)
    {
        if (await _userRepository.UserExists(login, token))
        {
            throw new Exception($"Пользователь с логином {login} уже создан!");
        }
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = login
        };
        
        await _userRepository.Create(user, token);
        
        return user.Id;
    }

    public async Task<User> Get(string login, CancellationToken token)
    {
        if (!await _userRepository.UserExists(login, token))
        {
            throw new Exception($"Пользователь с логином {login} не зарегистрирован!");
        }

        var user = await _userRepository.Get(login, token);
        return user;
    }
    
}