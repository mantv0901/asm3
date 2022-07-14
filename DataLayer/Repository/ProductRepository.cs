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

        public void Add(Product product)
        {
            context.Add(product);
            context.SaveChanges();
        }

        public List<Product> GetAll(Expression<Func<Product, bool>> ex)
        {
            return context.Products.Where(ex)
                .Include(x=> x.Category)
                .ToList();
        }

        public Product GetById(Expression<Func<Product, bool>> idExpression)
        {
            return context.Products
                .Include(x=> x.Category)
                .FirstOrDefault(idExpression);
        }

        public void Update(Product product)
        {
            context.Update(product);
            context.SaveChanges();
        }
    }
}
