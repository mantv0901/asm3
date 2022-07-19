using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Models;
namespace DataLayer.Repository
{
    public interface IProductRepository
    {
        Task Add(Product product);
        Task Update(Product product);
        Task<Product> GetById(Expression<Func<Product, bool>> idExpression);
        Task<List<Product>> GetAll(Expression<Func<Product,bool>> ex);
    }
}
