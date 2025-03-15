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
    }
}

