using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using task4.Services;

namespace task4.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly AuthService _auth;

    [BindProperty] public string Name { get; set; }
    [BindProperty] public string Email { get; set; }
    [BindProperty] public string Password { get; set; }

    public RegisterModel(AuthService auth)
    {
        _auth = auth;
    }

    public async Task<IActionResult> OnPost()
    {
        if (string.IsNullOrWhiteSpace(Name) ||
            string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Password))
        {
            ModelState.AddModelError("", "All fields are required");
            return Page();
        }

        var baseUrl = $"{Request.Scheme}://{Request.Host}";

        var error = await _auth.Register(Name, Email, Password, baseUrl);

        if (error != null)
        {
            ModelState.AddModelError("", error);
            return Page();
        }

        return RedirectToPage("/Account/Login");
    }

}