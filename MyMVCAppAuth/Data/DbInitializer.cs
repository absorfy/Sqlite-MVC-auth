using MyMVCAppAuth.Entities;

namespace MyMVCAppAuth.Data;

public static class DbInitializer
{
    public static void Seed(ApplicationDbContext context)
    {
        SeedClasses(context);
        SeedSkills(context);
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