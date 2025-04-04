using dancelog.Data;
using dancelog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace dancelog.Pages
{
    public class ListOfGroupsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public List<Group> Groups { get; set; }

        public ListOfGroupsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Groups = _context.Groups.ToList();
        }

    }
}
