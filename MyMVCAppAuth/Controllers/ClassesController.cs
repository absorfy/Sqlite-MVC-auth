using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMVCApp.Models;
using MyMVCAppAuth.Data;
using MyMVCAppAuth.Mappers;

namespace MyMVCAppAuth.Controllers
{
    public class ClassesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClassesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Classes
        public Task<IActionResult> Index()
        {
            return Task.FromResult<IActionResult>(View(_context.Classes.Select(ClassMapper.ToViewModel).ToList()));
        }

        // GET: Classes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classEntity = await _context.Classes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (classEntity == null)
            {
                return NotFound();
            }

            return View(ClassMapper.ToViewModel(classEntity));
        }

        // GET: Classes/Create
        public IActionResult Create()
        {
            return View(new ClassViewModel());
        }

        // POST: Classes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("Id,Name,Description,HeroIds")] ClassViewModel classViewModel)
        {
            if (id != classViewModel.Id)
            {
                return NotFound();
            }
            
            await CheckUniqueError(classViewModel);
            if (ModelState.IsValid)
            {
                _context.Add(ClassMapper.ToEntity(classViewModel));
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(classViewModel);
        }

        // GET: Classes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classEntity = (await _context.GetClassesAsync()).Find(c => c.Id == id);
            if (classEntity == null)
            {
                return NotFound();
            }
            return View(ClassMapper.ToViewModel(classEntity));
        }

        // POST: Classes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,HeroIds")] ClassViewModel classViewModel)
        {
            if (id != classViewModel.Id)
            {
                return NotFound();
            }

            await CheckUniqueError(classViewModel);
            if (ModelState.IsValid)
            {
                try
                {
                    var oldClass = await _context.GetClassById(classViewModel.Id);
                    if (oldClass == null) return NotFound();
                    ClassMapper.ToEntity(oldClass, classViewModel, await _context.Heroes.ToListAsync());
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ClassViewModelExists(classViewModel.Id))
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
            return View(classViewModel);
        }

        // GET: Classes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classEntity = await _context.Classes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (classEntity == null)
            {
                return NotFound();
            }

            return View(ClassMapper.ToViewModel(classEntity));
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var classEntity = await _context.Classes.FindAsync(id);
            if (classEntity != null)
            {
                _context.Classes.Remove(classEntity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ClassViewModelExists(int id)
        {
            return await _context.Classes.AnyAsync(e => e.Id == id);
        }

        private async Task CheckUniqueError(ClassViewModel classViewModel)
        {
            if (await _context.Classes.AnyAsync(c => c.Name == classViewModel.Name && c.Id != classViewModel.Id))
            {
                ModelState.AddModelError("Name", "Клас з такою назвою вже існує");
            }
        }
    }
}
