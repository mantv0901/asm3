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

        public async Task Add(Member member)
        {
            context.Members.Add(member);
            await context.SaveChangesAsync();
        }

        public  Task<Member> Get(Expression<Func<Member, bool>> ex)
        {
            return context.Members.FirstOrDefaultAsync(ex);
        }

        public Task<List<Member>> GetAll(Expression<Func<Member, bool>> ex)
        {
            return context.Members.Where(ex).ToListAsync();
        }

        public async Task Update(Member member)
        {
            context.Members.Update(member);
            await context.SaveChangesAsync();
        }
        public async Task Delete(Member member)
        {
            context.Members.Remove(member);
            await context.SaveChangesAsync();
        }

       
    }
}
