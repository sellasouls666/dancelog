using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dancelog.Data;
using dancelog.Models.Auth;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace dancelog.Pages.Account
{
    [Authorize]
    public class EditProfileModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditProfileModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Имя")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Фамилия")]
            public string LastName { get; set; }

            [Display(Name = "Отчество")]
            public string MiddleName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
        }

        public IActionResult OnGet()
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToPage("/Account/Login");
            }

            var user = _context.AuthUsers.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return NotFound();
            }

            Input = new InputModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                Email = user.Email
            };

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var user = _context.AuthUsers.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.MiddleName = Input.MiddleName;
            user.Email = Input.Email;

            _context.SaveChanges();

            return RedirectToPage("Profile");
        }
    }
}