using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.DAL.Repositories
{
    public class CategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }


    }
}
