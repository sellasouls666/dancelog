using dancelog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace dancelog.Pages
{
    public class ListOfStudentsModel : PageModel
    {
        public List<Student> Students { get; set; } = new List<Student>
        {
            
        };
    }
}
