using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Models;

namespace DataLayer.Repository
{
    public interface IOrderRepository
    {
        Task Add(Order order);
        Task Update(Order order);
        Task<Order> Get(Expression<Func<Order, bool>> ex);
        Task<List<Order>> GetAll(Expression<Func<Order, bool>> ex);
    }
}
