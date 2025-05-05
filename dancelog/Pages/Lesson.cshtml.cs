using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dancelog.Data;
using dancelog.Models;

namespace dancelog.Pages
{
    [Authorize(Roles = "Админ,Учитель")]
    public class LessonModel : PageModel
    {
        
    }
}
