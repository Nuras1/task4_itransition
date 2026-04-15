using Microsoft.Data.Sqlite;
using task4.Data;
using task4.Helpers;
using task4.Models;
namespace task4.Services
{
    public class AuthService

    {
        private readonly EmailService _emailService;
        private readonly AppDbContext _context;
        private readonly PasswordHasher _hasher;

        public AuthService(AppDbContext context, PasswordHasher hasher, EmailService emailService)
        {
            _context = context;
            _hasher = hasher;
            _emailService = emailService;
        }

        public async Task< User?> Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == email);

            if (user == null || user.Status == UserStatus.Blocked)
                return null;

            if (!_hasher.VerifyPassword(user.PasswordHash, password))
                return null;

            user.LastLoginTime = DateTime.Now;
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<string?> Register(string name, string email, string password, string baseUrl)
        {
            try
            {
                var token = Guid.NewGuid().ToString();

                var user = new User
                {
                    Name = name,
                    Email = email,
                    PasswordHash = _hasher.HashPassword(password),
                    Status = UserStatus.Unverified,
                    CreatedAt = DateTime.Now,
                    EmailToken = token
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var link = $"{baseUrl}/Account/Confirm?token={token}";

                //await _emailService.Send(email, "Confirm your email",
                //$"Click here: {link}");
                user.Status = UserStatus.Active;

                return null;
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19)
            {
                return "Email already exists";
            }
            catch (Exception ex)
            {
                return ex.Message; 
            }
        }
    }
}
