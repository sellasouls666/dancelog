using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dancelog.Data;
using dancelog.Models;
using System.Security.Claims;

namespace dancelog.Pages
{
    [Authorize]
    public class ListOfLessonsModel : PageModel
    {

    }
}