using dancelog.Data;
using dancelog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace dancelog.Pages
{
    public class ListOfCourseModel : PageModel
    {

        public List<Course> Courses { get; set; } = new List<Course>();
        public void OnGet()
        {
            Courses = SampleData.GetCourses();
        }
    }
}
