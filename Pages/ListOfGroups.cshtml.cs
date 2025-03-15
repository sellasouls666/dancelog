using dancelog.Data;
using dancelog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace dancelog.Pages
{
    public class ListOfGroupsModel : PageModel
    {
        public List<Group> Groups { get; set; } = new List<Group>();
        public void OnGet()
        {
            Groups = SampleData.GetGroups();
        }

    }
}
