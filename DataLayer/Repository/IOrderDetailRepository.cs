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
        void Add(OrderDetail orderDetail);
        void Update(OrderDetail orderDetail);
        OrderDetail Get(Expression<Func<OrderDetail, bool>> predicate);
        List<OrderDetail> GetAll(Expression<Func<OrderDetail,bool>> expression);
    }
}
