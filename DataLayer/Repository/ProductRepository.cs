using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Models;
using Microsoft.EntityFrameworkCore;
namespace DataLayer.Repository
{
    public class ProductRepository:IProductRepository
    {
        private PRN211_DB_ASMContext context;

        public ProductRepository(PRN211_DB_ASMContext context)
        {
            this.context = context;
        }

        public async Task Add(Product product)
        {
            context.Add(product);
            await context.SaveChangesAsync();
        }

        public Task<List<Product>> GetAll(Expression<Func<Product, bool>> ex)
        {
            return context.Products.Where(ex)
                .Include(x=> x.Category)
                .ToListAsync();
        }

        public Task<Product> GetById(Expression<Func<Product, bool>> idExpression)
        {
            return context.Products
                .Include(x=> x.Category)
                .FirstOrDefaultAsync(idExpression);
        }

        public async Task Update(Product product)
        {
            context.Update(product);
            await context.SaveChangesAsync();
        }
    }
}
