using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Models;

namespace DataLayer.Repository
{
    public interface IOrderDetailRepository
    {
        Task Add(OrderDetail orderDetail);
        Task Update(OrderDetail orderDetail);
        Task<OrderDetail> Get(Expression<Func<OrderDetail, bool>> predicate);
        Task<List<OrderDetail>> GetAll(Expression<Func<OrderDetail,bool>> expression);
    }
}
