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
        void Add(Member member);
        Member Get(Expression <Func<Member,bool>> ex);
        List<Member> GetAll(Expression<Func<Member, bool>> ex);
        void Update(Member member);

    }
}
