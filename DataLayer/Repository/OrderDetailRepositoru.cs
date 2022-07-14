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

        public void Add(OrderDetail orderDetail)
        {
            context.Add(orderDetail);
            context.SaveChanges();
        }

        public OrderDetail Get(Expression<Func<OrderDetail, bool>> predicate)
        {
            return context.OrderDetails.FirstOrDefault(predicate);
        }

        public List<OrderDetail> GetAll(Expression<Func<OrderDetail, bool>> expression)
        {
            return context.OrderDetails.Where(expression).ToList();
        }

        public void Update(OrderDetail orderDetail)
        {
            context.Update(orderDetail);
            context.SaveChanges();
        }
    }
}
