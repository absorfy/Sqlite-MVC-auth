using MyMVCAppAuth.Entities;
using MyMVCAppAuth.Models;

namespace MyMVCAppAuth.Mappers;

public class HeroMapper
{
    public static HeroEntity ToEntity(HeroViewModel model, ICollection<SkillEntity>? allSkills = null)
    {
        return new HeroEntity()
        {
            Id = model.Id,
            Name = model.Name,
            ClassId = model.ClassId,
            ImageUrl = model.ImageUrl,
            CreatedAt = DateTime.Now,
            Skills = allSkills
                ?.Where(skill => model.SkillIds.Contains(skill.Id))
                .ToList() ?? []
        };
    }

    public static HeroEntity ToEntity(HeroEntity entity, HeroViewModel model, ICollection<SkillEntity>? allSkills = null)
    {
        if (entity.Id != model.Id)
        {
            throw new Exception("Hero IDs do not match.");
        }
        
        entity.Name = model.Name;
        entity.ClassId = model.ClassId;
        entity.ImageUrl = model.ImageUrl;
        entity.Skills.Clear();
        entity.Skills = allSkills
            ?.Where(skill => model.SkillIds.Contains(skill.Id))
            .ToList() ?? [];
        
        return entity;
    }

    public static HeroViewModel ToViewModel(HeroEntity entity)
    {
        return new HeroViewModel
        {
            Id = entity.Id,
            Name = entity.Name,
            ClassId = entity.ClassId,
            ImageUrl = entity.ImageUrl,
            SkillIds = entity.Skills?.Select(skill => skill.Id).ToList() ?? []
        };
    }
}