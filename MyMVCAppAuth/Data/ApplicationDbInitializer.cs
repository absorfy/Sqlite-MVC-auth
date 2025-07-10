using MyMVCAppAuth.Entities;

namespace MyMVCAppAuth.Data;

public static class ApplicationDbInitializer
{
    public static void Seed(ApplicationDbContext context)
    {
        SeedClasses(context);
        SeedSkills(context);
        SeedHeroes(context);
    }

    private static void SeedHeroes(ApplicationDbContext context)
    {
        if (context.Heroes.Any())
        {
            return;
        }

        var random = new Random();
        var classIds = context.Classes.Select(c => c.Id).ToList();
        var skillIds = context.Skills.Select(s => s.Id).ToList();

        var names = new[]
        {
            "Thorin", "Elandra", "Magnus", "Sylva", "Drogan", "Alina", "Korrin", "Seraphine", "Baldric", "Nyssa",
            "Lucan", "Vesper", "Orin", "Freya", "Kael", "Riven", "Tamsin", "Garrick", "Zara", "Malric"
        };

        var heroes = new List<HeroEntity>();

        foreach (var name in names)
        {
            // Випадково вибираємо унікальні SkillId
            var selectedSkillIds = skillIds
                .OrderBy(_ => random.Next())
                .Take(random.Next(1, 4)) // 1–3 навички
                .ToList();

            var selectedSkills = context.Skills
                .Where(s => selectedSkillIds.Contains(s.Id))
                .ToList();

            var hero = new HeroEntity
            {
                Name = name,
                ClassId = classIds[random.Next(classIds.Count)],
                CreatedAt = DateTime.UtcNow,
                Skills = selectedSkills
            };

            heroes.Add(hero);
        }

        context.Heroes.AddRange(heroes);
        context.SaveChanges();
    }
    
    private static void SeedClasses(ApplicationDbContext context)
    {
        if (context.Classes.Any())
            return;
        
        context.Classes.AddRange(new ClassEntity { Name = "Mage", Description = "A master of arcane arts." },
                                 new ClassEntity { Name = "Warrior", Description = "A strong and brave fighter." },
                                 new ClassEntity { Name = "Rogue", Description = "A stealthy and agile combatant." });;
        context.SaveChanges();
    }

    private static void SeedSkills(ApplicationDbContext context)
    {
        if (context.Skills.Any())
            return;
        
        context.Skills.AddRange(new SkillEntity { Name = "Fireball", Level = 5 },
                                 new SkillEntity { Name = "Shield Bash", Level = 2 },
                                 new SkillEntity { Name = "Backstab", Level = 3 });
        
        context.SaveChanges();
    }
}