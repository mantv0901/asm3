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
    public class MemberRepository:IMemberRepository
    {
        private PRN211_DB_ASMContext context;
        public MemberRepository(PRN211_DB_ASMContext context)
        {
            this.context = context;
        }

        public void Add(Member member)
        {
            context.Members.Add(member);
            context.SaveChanges();
        }

        public Member Get(Expression<Func<Member, bool>> ex)
        {
            return context.Members.FirstOrDefault(ex);
        }

        public List<Member> GetAll(Expression<Func<Member, bool>> ex)
        {
            return context.Members.Where(ex).ToList();
        }

        public void Update(Member member)
        {
            context.Members.Update(member);
            context.SaveChanges();
        }
    }
}
