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
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private PRN211_DB_ASMContext context;
        public OrderDetailRepository(PRN211_DB_ASMContext context)
        {
            this.context = context;
        }

        public async Task Add(OrderDetail orderDetail)
        {
            context.Add(orderDetail);
            await context.SaveChangesAsync();
        }

        public Task<OrderDetail> Get(Expression<Func<OrderDetail, bool>> predicate)
        {
            return context.OrderDetails
                .Include(x => x.Order)
                .Include(x => x.Product)
                .FirstOrDefaultAsync(predicate);
        }

        public Task<List<OrderDetail>> GetAll(Expression<Func<OrderDetail, bool>> expression)
        {
            return context.OrderDetails.Where(expression)
                .Include(x=> x.Order)
                .Include(x=> x.Product)
                .ToListAsync();
        }

        public async Task Update(OrderDetail orderDetail)
        {
            context.Update(orderDetail);
            await context.SaveChangesAsync();
        }
    }
}
