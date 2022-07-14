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
        void Add(Product product);
        void Update(Product product);
        Product GetById(Expression<Func<Product, bool>> idExpression);
        List<Product> GetAll(Expression<Func<Product,bool>> ex);
    }
}
