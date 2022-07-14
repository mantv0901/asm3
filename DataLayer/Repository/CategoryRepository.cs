using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Models;

namespace DataLayer.Repository
{
    public class CategoryRepository:ICategoryRepository
    {
        private PRN211_DB_ASMContext context;
        public CategoryRepository(PRN211_DB_ASMContext context)
        {
            this.context = context;
        }

        public Category Get(int id)
        {
            return context.Categories.FirstOrDefault(x=> x.CategoryId == id);   
        }

        public List<Category> GetAll()
        {
            return context.Categories.ToList();
        }
    }
}
