using Buzile_TicTacToe.Models;
using Buzile_TicTacToe.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Buzile_TicTacToe.Tests.Integration;

public class TestsApplicationContext : ApplicationContext
{
    public DbSet<User> Users { get; set; }

    public TestsApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        
    }
}