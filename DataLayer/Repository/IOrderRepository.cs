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
        void Add(Order order);
        void Update(Order order);
        Order Get(Expression<Func<Order, bool>> ex);
        List<Order> GetAll(Expression<Func<Order, bool>> ex);
    }
}
