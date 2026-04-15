using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using task4.Data;
using task4.Models;

namespace task4.Pages.Account;

public class ConfirmModel : PageModel
{
    private readonly AppDbContext _context;

    public ConfirmModel(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGet(string token)
    {
        var user = _context.Users.FirstOrDefault(x => x.EmailToken == token);

        if (user == null)
            return RedirectToPage("/Account/Login");

        user.Status = UserStatus.Active;
        user.EmailToken = null;

        await _context.SaveChangesAsync();

        return RedirectToPage("/Account/Login");
    }
}