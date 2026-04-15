using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

using task4.Services;

namespace task4.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly AuthService _auth;

    [BindProperty][Required]public string Name { get; set; }

    [BindProperty][Required][EmailAddress]public string Email { get; set; }

    [BindProperty][Required]public string Password { get; set; }

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

        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (error != null)
        {
            ModelState.AddModelError("", error);
            return Page();
        }

        return RedirectToPage("/Account/Login");
    }
}