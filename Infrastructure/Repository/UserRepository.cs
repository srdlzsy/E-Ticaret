using Domain.Models;
using Infrastructure.data;

public class UserRepository
{
    private readonly MınıDbContext _context;

    public UserRepository(MınıDbContext context)
    {
        _context = context;
    }

    public bool IsEmailRegistered(string email)
    {
        return _context.Users.Any(u => u.Email == email);
    }

    public User GetUserByEmailAndPassword(string email, string passwordHash)
    {
        return _context.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == passwordHash);
    }

    public User GetUserByEmail(string email)
    {
        return _context.Users.FirstOrDefault(u => u.Email == email);
    }

    public void AddUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void UpdateUser(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }
}
