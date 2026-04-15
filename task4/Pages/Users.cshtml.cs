using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using task4.Data;
using task4.Models;

namespace task4.Pages;

public class UsersModel : PageModel
{
    private readonly AppDbContext _context;

    public UsersModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty(SupportsGet = true)]
    public string SortOrder { get; set; }

    public List<User> Users { get; set; } = new();

    [BindProperty]
    public List<int> SelectedIds { get; set; } = new();

    public void OnGet()
    {
        var query = _context.Users.AsQueryable();

        query = SortOrder == "desc"
            ? query.OrderByDescending(x => x.Email)
            : query.OrderBy(x => x.Email);

        Users = query.ToList();
    }

    private IQueryable<User> GetSelectedUsers()
    {
        return _context.Users.Where(x => SelectedIds.Contains(x.Id));
    }

    private IActionResult? CheckSelection()
    {
        if (SelectedIds == null || !SelectedIds.Any())
        {
            TempData["Message"] = "No users selected";
            return RedirectToPage();
        }

        return null;
    }

    public async Task<IActionResult> OnPostBlock()
    {
        var check = CheckSelection();
        if (check != null) return check;

        foreach (var user in GetSelectedUsers())
            user.Status = UserStatus.Blocked;

        await _context.SaveChangesAsync();

        return HandleSelfAction("Users blocked");
    }

    public async Task<IActionResult> OnPostUnblock()
    {
        var check = CheckSelection();
        if (check != null) return check;

        foreach (var user in GetSelectedUsers())
            user.Status = UserStatus.Active;

        await _context.SaveChangesAsync();

        TempData["Message"] = "Users unblocked";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDelete()
    {
        var check = CheckSelection();
        if (check != null) return check;

        var users = GetSelectedUsers();
        _context.Users.RemoveRange(users);

        await _context.SaveChangesAsync();

        return HandleSelfAction("Users deleted");
    }

    public async Task<IActionResult> OnPostDeleteUnverified()
    {
        var users = _context.Users
            .Where(x => x.Status == UserStatus.Unverified);

        _context.Users.RemoveRange(users);
        await _context.SaveChangesAsync();

        TempData["Message"] = "Unverified users deleted";
        return RedirectToPage();
    }

    private IActionResult HandleSelfAction(string message)
    {
        var currentUserId = HttpContext.Session.GetInt32("UserId");

        if (currentUserId != null && SelectedIds.Contains(currentUserId.Value))
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Account/Login");
        }

        TempData["Message"] = message;
        return RedirectToPage();
    }
}