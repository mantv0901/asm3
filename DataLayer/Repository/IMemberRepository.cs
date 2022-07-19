using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Models;
namespace DataLayer.Repository
{
    public interface IMemberRepository
    {
        Task Add(Member member);
        Task<Member> Get(Expression <Func<Member,bool>> ex);
        Task<List<Member>> GetAll(Expression<Func<Member, bool>> ex);
        Task Update(Member member);
        Task Delete(Member member);
        //void Get(Expression<Func<Member, bool>> ex);

    }
}
