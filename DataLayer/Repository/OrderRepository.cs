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
    public class OrderRepository:IOrderRepository
    {
        private PRN211_DB_ASMContext context;
        public OrderRepository(PRN211_DB_ASMContext context)
        {
            this.context = context;
        }

        public void Add(Order order)
        {
            context.Orders.Add(order);
            context.SaveChanges();
        }

        public Order Get(Expression<Func<Order, bool>> ex)
        {
            return context.Orders.Find(ex);
        }

        public List<Order> GetAll(Expression<Func<Order, bool>> ex)
        {
            return context.Orders.Where(ex).ToList();
        }

        public void Update(Order order)
        {
            context.Orders.Update(order);
            context.SaveChanges();
        }
    }
}
