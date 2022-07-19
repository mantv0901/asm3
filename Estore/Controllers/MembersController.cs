using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessLayer.Models;
using DataLayer.Repository;
using Microsoft.AspNetCore.Authorization;

namespace Estore.Controllers
{
    [Authorize(Roles ="Admin")]
    public class MembersController : Controller
    {
        private readonly PRN211_DB_ASMContext _context;
        private readonly IMemberRepository _memberRepository;

        public MembersController(PRN211_DB_ASMContext context, IMemberRepository _memberRepository)
        {
            _context = context;
            this._memberRepository = _memberRepository; 
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            return View(await _memberRepository.GetAll(x=> x.Status == true));
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _memberRepository.Get(x => x.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberId,Email,CompanyName,City,Country,Password")] Member member)
        {
            Member mem = _memberRepository.Get(x => x.Email == member.Email).Result;
            if (mem != null)
            {
                ViewData["mess"] = "Duplicated Email!";
                return View(member);
            }
            if (ModelState.IsValid)
            {
                member.Status = true;
                await _memberRepository.Add(member);
                return RedirectToAction(nameof(Index));
            }
            
            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _memberRepository.Get(x=> x.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemberId,Email,CompanyName,City,Country,Password")] Member member)
        {
            if (id != member.MemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Member mem = await _memberRepository.Get(x=>x.MemberId == id);
                    mem.Email = member.Email;
                    mem.City = member.City;
                    mem.CompanyName = member.CompanyName;
                    mem.Country = member.Country;
                    mem.Password = member.Password;
                    await _memberRepository.Update(mem);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.MemberId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _memberRepository.Get(x=> x.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _memberRepository.Get(x=> x.MemberId ==id);
            //await _memberRepository.Delete(member);
            member.Status = false;
            await _memberRepository.Update(member); 
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            //return _context.Members.Any(e => e.MemberId == id);
            return _memberRepository.Get(x => x.MemberId == id) != null;
        }
    }
}
