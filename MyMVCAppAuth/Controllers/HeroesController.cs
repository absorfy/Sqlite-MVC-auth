using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyMVCAppAuth.Data;
using MyMVCAppAuth.Entities;
using MyMVCAppAuth.Mappers;
using MyMVCAppAuth.Models;

namespace MyMVCAppAuth.Controllers
{
    public class HeroesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HeroesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Heroes
        public async Task<IActionResult> Index(PaginateViewModel paginateViewModel)
        {
            var allHeroes = await _context.GetHeroesAsync();

            // Сортування
            IEnumerable<HeroEntity> sortedHeroes = paginateViewModel.OrderBy switch
            {
                HeroesOrderByEnum.Name => paginateViewModel.IsAscending
                    ? allHeroes.OrderBy(h => h.Name)
                    : allHeroes.OrderByDescending(h => h.Name),

                HeroesOrderByEnum.CreatedAt => paginateViewModel.IsAscending
                    ? allHeroes.OrderBy(h => h.CreatedAt)
                    : allHeroes.OrderByDescending(h => h.CreatedAt),

                _ => allHeroes
            };

            // Загальна кількість
            paginateViewModel.TotalCount = sortedHeroes.Count();

            // Кількість сторінок
            paginateViewModel.TotalPages = (int)Math.Ceiling((double)paginateViewModel.TotalCount / paginateViewModel.PageSize);

            // Отримання потрібної сторінки
            var paginatedHeroes = sortedHeroes
                .Skip(paginateViewModel.Page * paginateViewModel.PageSize)
                .Take(paginateViewModel.PageSize)
                .ToList();

            // Дані у ViewData
            PutClassesToViewData(paginatedHeroes.ToArray());
            PutSkillsToViewData(paginatedHeroes.ToArray());

            // Передача пагінації у View
            ViewBag.Pagination = paginateViewModel;

            // Повернення View з ViewModel-ами
            return View(paginatedHeroes.Select(HeroMapper.ToViewModel).ToList());
        }

        // GET: Heroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var heroEntity = (await _context.GetHeroesAsync()).FirstOrDefault(m => m.Id == id);
            if (heroEntity == null)
            {
                return NotFound();
            }
            
            PutClassesToViewData(heroEntity);
            PutSkillsToViewData(heroEntity);
            return View(HeroMapper.ToViewModel(heroEntity));
        }

        private void PutClassesToViewData(params HeroEntity[] heroes)
        {
            var classes = heroes.Select(h => h.Class != null ? ClassMapper.ToViewModel(h.Class) : null).ToList();
            ViewData["Classes"] = classes;
        }
        
        private void PutSkillsToViewData(params HeroEntity[] heroes)
        {
            var skills = heroes.SelectMany(h => h.Skills).Distinct().Select(SkillMapper.ToViewModel).ToList();
            ViewData["Skills"] = skills;
        }
        
        private void PutClassesForSelect(ClassEntity? selected = null)
        {
            var classes = _context.Classes.ToList();
            ViewData["SelectClasses"] = new SelectList(classes, "Id", "Name", selected);
        }

        private async Task PutClassesForSelect(int selectedId)
        {
            PutClassesForSelect((await _context.GetClassesAsync()).FirstOrDefault(c => c.Id == selectedId));
        }
    
        private void PutSkillsForSelect(IEnumerable<SkillEntity>? selected = null)
        {
            var skills = _context.Skills.ToList();
            ViewData["SelectSkills"] = new MultiSelectList(skills, "Id", "Name", selected);
        }
        
        private async Task PutSkillsForSelect(IEnumerable<int> selectedIds)
        {
            PutSkillsForSelect((await _context.GetSkillsAsync()).Where(s => selectedIds.Contains(s.Id)).ToArray());
        }
        
        // GET: Heroes/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            PutClassesForSelect();
            PutSkillsForSelect();
            return View(new HeroViewModel());
        }

        // POST: Heroes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Name,ClassId,HeroImageFile,SkillIds")] HeroViewModel model)
        {
            if (ModelState.IsValid)
            {
                await TrySaveHeroImage(model);
                _context.Heroes.Add(HeroMapper.ToEntity(model, await _context.Skills.ToListAsync()));
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        
            PutClassesForSelect();
            PutSkillsForSelect();
            return View(model);
        }

        public async Task TrySaveHeroImage(HeroViewModel model)
        {
            if (model.HeroImageFile != null)
            {
                var fileName = Guid.NewGuid() + "_" + model.HeroImageFile.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "heroes", fileName);
                await using var stream = new FileStream(filePath, FileMode.Create);
                await model.HeroImageFile.CopyToAsync(stream);
                model.ImageUrl = "/images/heroes/" + fileName;
            }
        }

        // GET: Heroes/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            var hero = (await _context.GetHeroesAsync()).Find(h => h.Id == id);
            if (hero == null)
                return NotFound();
        
            PutClassesForSelect(hero.Class);
            PutSkillsForSelect(hero.Skills.ToArray());
            return View(HeroMapper.ToViewModel(hero));
        }

        // POST: Heroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,Name,ClassId,ImageUrl,HeroImageFile,SkillIds")] HeroViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    await TrySaveHeroImage(model);
                    var oldHero = await _context.GetHeroById(model.Id);
                    if(oldHero == null) return NotFound();
                    HeroMapper.ToEntity(oldHero, model, await _context.Skills.ToListAsync());
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Heroes.Any(h => h.Id == model.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction("Index");
            }

            await PutClassesForSelect(model.ClassId);
            await PutSkillsForSelect(model.SkillIds);
            return View(model);
        }

        // GET: Heroes/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var heroEntity = (await _context.GetHeroesAsync()).FirstOrDefault(m => m.Id == id);
            if (heroEntity == null)
                return NotFound();

            PutClassesToViewData();
            PutSkillsToViewData();
            return View(HeroMapper.ToViewModel(heroEntity));
        }

        // POST: Heroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var heroEntity = await _context.Heroes.FindAsync(id);
            if (heroEntity == null) 
                return RedirectToAction(nameof(Index));
            
            _context.Heroes.Remove(heroEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
