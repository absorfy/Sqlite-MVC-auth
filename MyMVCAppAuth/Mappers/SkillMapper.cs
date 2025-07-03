using MyMVCApp.Models;
using MyMVCAppAuth.Entities;

namespace MyMVCAppAuth.Mappers;

public class SkillMapper
{
    public static SkillEntity ToEntity(SkillViewModel model, ICollection<HeroEntity>? allHeroes = null)
    {
        return new SkillEntity
        {
            Id = model.Id,
            Name = model.Name,
            Level = model.Level,
            CreatedAt = DateTime.Now,
            Heroes = allHeroes
                ?.Where(hero => model.HeroIds.Contains(hero.Id))
                .ToList() ?? []
        };
    }

    public static SkillEntity ToEntity(SkillEntity entity, SkillViewModel model, ICollection<HeroEntity>? allHeroes = null)
    {
        if (entity.Id != model.Id)
        {
            throw new Exception("Skill IDs do not match.");
        }
        
        entity.Name = model.Name;
        entity.Level = model.Level;
        entity.Heroes.Clear();
        entity.Heroes = allHeroes
            ?.Where(hero => model.HeroIds.Contains(hero.Id))
            .ToList() ?? [];
        return entity;
    }

    public static SkillViewModel ToViewModel(SkillEntity entity)
    {
        return new SkillViewModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Level = entity.Level,
            HeroIds = entity.Heroes?.Select(hero => hero.Id).ToList() ?? []
        };
    }
}
