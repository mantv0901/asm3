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

        public async Task Add(Order order)
        {
            context.Orders.Add(order);
            await context.SaveChangesAsync();
        }

        public Task<Order> Get(Expression<Func<Order, bool>> ex)
        {
            return context.Orders
                .Include(x=> x.Member)
                .FirstOrDefaultAsync(ex);
        }

        public Task<List<Order>> GetAll(Expression<Func<Order, bool>> ex)
        {
            return context.Orders
                .Include(x=> x.Member)
                .Where(ex).ToListAsync();
        }

        public async Task Update(Order order)
        {
            context.Orders.Update(order);
            await context.SaveChangesAsync();
        }
    }
}
