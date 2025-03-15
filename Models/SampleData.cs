using System.Collections.Generic;
using dancelog.Models;

namespace dancelog.Data
{
    public class SampleData
    {
        public static List<Course> GetCourses()
        {
            return new List<Course>
            {
                new Course { Id = 1, Name = "Базовый курс бальных танцев" },
                new Course { Id = 2, Name = "Хип-хоп для начинающих" },
                new Course { Id = 3, Name = "Современная хореография" },
                new Course { Id = 4, Name = "Танго: страсть и элегантность" },
                new Course { Id = 5, Name = "Джаз-модерн" },
                new Course { Id = 6, Name = "Сальса и бачата" },
                new Course { Id = 7, Name = "Классический балет" },
                new Course { Id = 8, Name = "Восточные танцы" },
                new Course { Id = 9, Name = "Брейк-данс" },
                new Course { Id = 10, Name = "Актерское мастерство в танце" }
            };
        }

        public static List<Group> GetGroups()
        {
            List<Course> courses = SampleData.GetCourses();

            return new List<Group>
            {
               new Group { Id = 1, Name = "Бальные танцы - Элегантное начало (Начинающие)", Course = courses[0] },
               new Group { Id = 2, Name = "Хип-хоп - Уличный стиль (Продвинутые)", Course = courses[1] },
               new Group { Id = 3, Name = "Современная хореография - Творческая лаборатория (Смешанный уровень)", Course = courses[2] },
               new Group { Id = 4, Name = "Танго - Огонь и страсть (Интенсив)", Course = courses[3] },
               new Group { Id = 5, Name = "Джаз-модерн - Ритм и движение (Средний уровень)", Course = courses[4] },
               new Group { Id = 6, Name = "Сальса и бачата - Латинский вечер (Вечерняя группа)", Course = courses[5] },
               new Group { Id = 7, Name = "Классический балет - Грация и совершенство (Подготовка к концерту)", Course = courses[6] },
               new Group { Id = 8, Name = "Восточные танцы - Танец души (Сценическая практика)", Course = courses[7] },
               new Group { Id = 9, Name = "Брейк-данс - Сила улиц (Силовая подготовка)", Course = courses[8] },
               new Group { Id = 10, Name = "Актерское мастерство в танце - Выразительное движение (Основной курс)", Course = courses[9] }
            };
        }

        public static void AddGroup(Group group)
        {
            var groups = GetGroups(); // Assuming you have this method
                                      // Generate an ID if needed (assuming Id is auto-incrementing)
            int nextId = groups.Any() ? groups.Max(g => g.Id) + 1 : 1;
            group.Id = nextId;
            groups.Add(group);
        }
    }
}

