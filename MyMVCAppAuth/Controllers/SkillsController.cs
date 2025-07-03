using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMVCApp.Mappers;
using MyMVCApp.Models;
using MyMVCAppAuth.Data;

namespace MyMVCAppAuth.Controllers
{
    public class SkillsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SkillsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Skills
        public Task<IActionResult> Index()
        {
            return Task.FromResult<IActionResult>(View(_context.Skills.Select(SkillMapper.ToViewModel).ToList()));
        }

        // GET: Skills/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skillEntity = await _context.Skills
                .FirstOrDefaultAsync(m => m.Id == id);
            if (skillEntity == null)
            {
                return NotFound();
            }

            return View(SkillMapper.ToViewModel(skillEntity));
        }

        // GET: Skills/Create
        public IActionResult Create()
        {
            return View(new SkillViewModel());
        }

        // POST: Skills/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Level")] SkillViewModel skillViewModel)
        {
            await CheckUniqueError(skillViewModel);
            if (ModelState.IsValid)
            {
                _context.Add(SkillMapper.ToEntity(skillViewModel));
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(skillViewModel);
        }

        // GET: Skills/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skillEntity = (await _context.GetSkillsAsync()).Find(s => s.Id == id);
            if (skillEntity == null)
            {
                return NotFound();
            }
            return View(SkillMapper.ToViewModel(skillEntity));
        }

        // POST: Skills/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Level")] SkillViewModel skillViewModel)
        {
            if (id != skillViewModel.Id)
            {
                return NotFound();
            }

            await CheckUniqueError(skillViewModel);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(SkillMapper.ToEntity(skillViewModel, await _context.Heroes.ToListAsync()));
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await SkillEntityExists(skillViewModel.Id))
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
            return View(skillViewModel);
        }

        // GET: Skills/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skillEntity = await _context.Skills
                .FirstOrDefaultAsync(m => m.Id == id);
            if (skillEntity == null)
            {
                return NotFound();
            }

            return View(SkillMapper.ToViewModel(skillEntity));
        }

        // POST: Skills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var skillEntity = await _context.Skills.FindAsync(id);
            if (skillEntity != null)
            {
                _context.Skills.Remove(skillEntity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        private async Task<bool> SkillEntityExists(int id)
        {
            return await _context.Skills.AnyAsync(e => e.Id == id);
        }

        private async Task CheckUniqueError(SkillViewModel skillViewModel)
        {
            if (await _context.Skills.AnyAsync(s => s.Name == skillViewModel.Name && s.Id != skillViewModel.Id))
            {
                ModelState.AddModelError("Name", "Навичка з такою назвою вже існує");
            }
        }
    }
}
