using dancelog.Data;
using dancelog.Models;
using dancelog.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dancelog.Pages.Account
{
    public class UserModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public UserModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AuthUser User { get; set; }

        public SelectList GroupOptions { get; set; }

        public async Task OnGetAsync(int id)
        {
            // Загружаем группы для выпадающего списка
            var groups = await _context.Groups.ToListAsync();
            GroupOptions = new SelectList(groups, "Id", "Name");

            User = id == 0 ? new AuthUser() : await _context.AuthUsers
                .Include(u => u.Student)
                .FirstOrDefaultAsync(u => u.Id == id);

            // Если редактируем существующего ученика, устанавливаем GroupId
            if (User?.Role == "Ученик" && User.Student != null)
            {
                User.GroupId = User.Student.GroupId; 
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Перезагружаем группы при ошибке валидации
                var groups = await _context.Groups.ToListAsync();
                GroupOptions = new SelectList(groups, "Id", "Name");
                return Page();
            }

            bool isNewUser = User.Id == 0;

            if (isNewUser)
            {
                _context.AuthUsers.Add(User);

                if (User.Role == "Ученик" && User.GroupId.HasValue)
                {
                    Student student = new Student
                    {
                        Surname = User.LastName,
                        Name = User.FirstName,
                        Patronymic = User.MiddleName,
                        GroupId = User.GroupId.Value,
                        Email = User.Email,
                        AuthUser = User
                    };

                    _context.Students.Add(student);
                    User.Student = student;
                }
            }
            else
            {
                var existingUser = await _context.AuthUsers
                    .Include(u => u.Student)
                    .FirstOrDefaultAsync(u => u.Id == User.Id);

                if (existingUser != null)
                {
                    // Обновляем основные данные пользователя
                    existingUser.Email = User.Email;
                    existingUser.LastName = User.LastName;
                    existingUser.FirstName = User.FirstName;
                    existingUser.MiddleName = User.MiddleName;
                    existingUser.Password = User.Password;
                    existingUser.Role = User.Role;

                    // Обработка ученика
                    if (User.Role == "Ученик")
                    {
                        if (!User.GroupId.HasValue)
                        {
                            ModelState.AddModelError("User.GroupId", "Для ученика необходимо указать группу");
                            var groups = await _context.Groups.ToListAsync();
                            GroupOptions = new SelectList(groups, "Id", "Name");
                            return Page();
                        }

                        if (existingUser.Student == null)
                        {
                            existingUser.Student = new Student
                            {
                                Surname = User.LastName,
                                Name = User.FirstName,
                                Patronymic = User.MiddleName,
                                GroupId = User.GroupId.Value,
                                Email = User.Email,
                                AuthUser = existingUser
                            };
                            _context.Students.Add(existingUser.Student);
                        }
                        else
                        {
                            existingUser.Student.GroupId = User.GroupId.Value;
                            _context.Students.Update(existingUser.Student);
                        }
                    }
                    else if (existingUser.Student != null)
                    {
                        // Если роль изменилась с "Ученик" на другую, удаляем запись студента
                        _context.Students.Remove(existingUser.Student);
                        existingUser.Student = null;
                    }

                    _context.AuthUsers.Update(existingUser);
                }
            }

            try
            {
                await _context.SaveChangesAsync();

                // Редирект в зависимости от роли
                return User.Role == "Ученик"
                    ? RedirectToPage("ListOfUsers")
                    : RedirectToPage("ListOfUsers");
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", $"Ошибка при сохранении: {ex.Message}");
                var groups = await _context.Groups.ToListAsync();
                GroupOptions = new SelectList(groups, "Id", "Name");
                return Page();
            }
        }
    }
}