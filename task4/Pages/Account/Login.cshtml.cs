using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using task4.Services;

namespace task4.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly AuthService _auth;

        [BindProperty]public string Email { get; set; } = "";

        [BindProperty]public string Password { get; set; } = "";

        [BindProperty]public string Name { get; set; } = "";

        public LoginModel(AuthService auth)
        {
            _auth = auth;
        }

        public async Task<IActionResult> OnPost()
        {
            var user = await _auth.Login(Email, Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return Page();
            }

            HttpContext.Session.SetInt32("UserId", user.Id);

            return RedirectToPage("/Users");
        }

    }
}